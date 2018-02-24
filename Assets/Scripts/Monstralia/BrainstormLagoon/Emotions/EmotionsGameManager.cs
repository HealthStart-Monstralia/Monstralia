using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager<EmotionsGameManager> {
    public VoiceOversData voData;
    public Transform monsterLocation;
	public float timeLimit = 30;
	public bool gameStarted = false;
    public bool isTutorialRunning = false;
    public bool inputAllowed = false;

	public Transform[] emotionSpawnLocs;
	public GameObject backButton;
	public float waitDuration = 2.0f;

	public GameObject tutorialHand;
	public Canvas tutorialCanvas;

    private GameObject monster;
    private int score;
    private int scoreGoal = 3;
    private int difficultyLevel;
    private Coroutine tutorialCoroutine, drawingCardsCoroutine;
    private EmotionsGenerator generator;

    public override void PregameSetup () {
        generator = GetComponent<EmotionsGenerator> ();
        EmotionCard.CheckEmotion = CheckEmotion;
        tutorialCanvas.gameObject.SetActive (false);
        tutorialHand.SetActive (false);
        generator.cardHand.gameObject.SetActive (false);
        generator.SetSlots (GameManager.Instance.GetLevel (typeOfGame) + 1);
        difficultyLevel = GameManager.Instance.GetLevel (DataType.Minigame.MonsterEmotions);


        if (GameManager.Instance.GetPendingTutorial (DataType.Minigame.MonsterEmotions)) {
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {
            switch (difficultyLevel) {
                case 2:
                    scoreGoal = 5;
                    timeLimit = 30;
                    break;
                case 3:
                    scoreGoal = 7;
                    timeLimit = 45;
                    break;
                default:
                    scoreGoal = 3;
                    break;
            }

            if (difficultyLevel > 2)
                generator.allowOtherMonsterCards = true;

            if (!monster)
                CreateMonster ();
            ChangeMonsterEmotion (DataType.MonsterEmotions.Happy);

            score = 0;
            if (TimerClock.Instance != null) {
                TimerClock.Instance.SetTimeLimit (timeLimit);
                TimerClock.Instance.StopTimer ();
            }

            UpdateScoreGauge ();
            TimerClock.Instance.gameObject.SetActive (true);
            generator.cardHand.gameObject.SetActive (true);
            generator.cardHand.SpawnIn ();
            StartCoroutine (DuringCountdown ());
            StartCountdown (PostCountdownSetup);
        }
	}

	public IEnumerator DuringCountdown() {
        DrawCards (0.5f);
		yield return new WaitForSeconds (3.0f);
        ChangeMonsterEmotion (generator.GetSelectedEmotion ());
	}

    #region Tutorial

    IEnumerator RunTutorial () { 
		print ("RunTutorial");
		isTutorialRunning = true;
		tutorialCanvas.gameObject.SetActive (true);
		inputAllowed = false;
        ScoreGauge.Instance.gameObject.SetActive (false);
        TimerClock.Instance.gameObject.SetActive (false);
		yield return new WaitForSeconds(0.5f);

        if (!monster)
            CreateMonster ();
        ChangeMonsterEmotion (DataType.MonsterEmotions.Happy);

        SubtitlePanel.Instance.Display ("Welcome to Monster Feelings!", null);
		SoundManager.Instance.StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("1_tutorial_start");
		SoundManager.Instance.PlayVoiceOverClip(tutorial1);
        
        float secsToRemove = 7f;
        yield return new WaitForSeconds(tutorial1.length - secsToRemove);

        generator.SelectTutorialEmotions ();
        ChangeMonsterEmotion (generator.GetSelectedEmotion ());
        generator.cardHand.gameObject.SetActive (true);
        SubtitlePanel.Instance.Hide ();
        float secsToRemoveAgain = 4f;
        yield return new WaitForSeconds (secsToRemove - secsToRemoveAgain);

        TutorialDrawCards ();
        yield return new WaitForSeconds (secsToRemoveAgain);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("EM_HandMoveMonster");
		yield return new WaitForSeconds(1.75f);

		SubtitlePanel.Instance.Display (generator.GetSelectedEmotion().ToString(), null);
        SoundManager.Instance.PlayCorrectSFX ();
        yield return new WaitForSeconds(2.0f);

		SubtitlePanel.Instance.Hide ();
		yield return new WaitForSeconds(1.0f);

        AudioClip nowyoutry = voData.FindVO ("nowyoutry");
        SubtitlePanel.Instance.Display ("Now you try!", nowyoutry);
		inputAllowed = true;
	}

	public void TutorialFinished() {
		inputAllowed = false;

        isTutorialRunning = false;
        tutorialHand.SetActive (false);
		GameManager.Instance.CompleteTutorial(DataType.Minigame.MonsterEmotions);
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

	IEnumerator TutorialTearDown() {
		print ("TutorialTearDown");
        if (generator.isDrawingCards)
            StopCoroutine (drawingCardsCoroutine);

        yield return new WaitForSeconds (1.5f);
        if (monster)
            ChangeMonsterEmotion (DataType.MonsterEmotions.Joyous);
        generator.RemoveCards ();
        AudioClip letsplay = voData.FindVO ("letsplay");
        SoundManager.Instance.StopPlayingVoiceOver ();
        SubtitlePanel.Instance.Display ("Let's play!", letsplay);
		yield return new WaitForSeconds(letsplay.length);
        if (generator.cardHand.gameObject.activeSelf)
            generator.cardHand.ExitAnimation ();
        tutorialCanvas.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.0f);
		SubtitlePanel.Instance.Hide ();

		PregameSetup ();
	}

    #endregion

    private void PostCountdownSetup() {
		StartGame();
	}

    private void StartGame () {
        ScoreGauge.Instance.gameObject.SetActive (true);

        TimerClock.Instance.StopTimer ();
        TimerClock.Instance.StartTimer ();

        gameStarted = true;
        inputAllowed = true;
    }

    IEnumerator PostGame () {
        print ("PostGame");
        gameStarted = false;
        StopCoroutine (drawingCardsCoroutine);
        inputAllowed = false;
        yield return new WaitForSeconds (1.0f);

        ChangeMonsterEmotion (DataType.MonsterEmotions.Happy);
        generator.RemoveCards ();
        AudioClip end = voData.FindVO ("end");
        yield return new WaitForSeconds (0.5f);

        SubtitlePanel.Instance.Display ("Great job! You matched " + score + " emotions!", end);
        yield return new WaitForSeconds (1.0f);

        if (generator.cardHand.gameObject.activeSelf)
            generator.cardHand.ExitAnimation ();
        yield return new WaitForSeconds (3.0f);

        if (score >= scoreGoal) {
            if (difficultyLevel == 1) {
                GameOver (DataType.GameEnd.EarnedSticker);
            } else {
                GameOver (DataType.GameEnd.CompletedLevel);
            }
        } else {
            GameOver (DataType.GameEnd.FailedLevel);
        }
    }

    public void OnOutOfTime() {
        StartCoroutine (PostGame ());
    }

    public void CheckEmotion (DataType.MonsterEmotions emotion, AudioClip clip) {
        if (inputAllowed && (isTutorialRunning || gameStarted)) {
            inputAllowed = false;
            SubtitlePanel.Instance.Display (emotion.ToString (), clip);
            if (emotion == generator.GetSelectedEmotion ()) {
                TimerClock.Instance.StopTimer ();
                SoundManager.Instance.PlayCorrectSFX ();
                if (isTutorialRunning) {
                    TutorialFinished ();
                } else {
                    ++score;
                    UpdateScoreGauge ();
                    if (score >= scoreGoal) {
                        StartCoroutine (PostGame ());
                    }
                    else {
                        DrawCards (waitDuration);
                    }
                }

            } else {
                StartCoroutine (WrongAnswerWait (waitDuration));
            }
        }
    }

    public void DrawCards(float waitPeriod) {
        drawingCardsCoroutine = StartCoroutine (generator.CreateNextEmotions (waitPeriod, ContinueGame));
    }

    public void TutorialDrawCards () {
        drawingCardsCoroutine = StartCoroutine (generator.CreateTutorialCards ());
    }

    public void ContinueGame() {
        if (gameStarted) {
            ChangeMonsterEmotion (generator.GetSelectedEmotion ());
            inputAllowed = true;
            TimerClock.Instance.StartTimer ();
        }
    }

	public IEnumerator WrongAnswerWait (float duration) {
		yield return new WaitForSeconds (duration);
        if (!isTutorialRunning)
            ContinueGame ();
        else if (gameStarted) {
            inputAllowed = true;
        }
            
	}

    void UpdateScoreGauge () {
        if (ScoreGauge.Instance.gameObject.activeSelf)
            ScoreGauge.Instance.SetProgressTransition ((float)score / scoreGoal);
    }

    public void SkipReviewButton (GameObject button) {
        SkipReview ();
        Destroy (button);
    }

    public void SkipReview () {
        StopCoroutine (tutorialCoroutine);
        TutorialFinished ();
    }

    public void CreateMonster () {
        Vector2 pos = monsterLocation.position;
        monster = Instantiate (GameManager.Instance.GetPlayerMonsterObject (), pos, Quaternion.identity);
        monster.transform.position = pos;
        monster.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        monster.gameObject.AddComponent<Animator> ();
        monster.GetComponent<Monster> ().IdleAnimationOn = false;
        monster.GetComponent<Monster> ().AllowMonsterTickle = false;
    }

    public void ChangeMonsterEmotion (DataType.MonsterEmotions emo) {
        monster.GetComponent<Monster> ().ChangeEmotions (emo);
    }
}
