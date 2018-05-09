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
    public AudioClip[] goodjobClips;
    public AudioClip[] wrongClips;

    private GameObject monster;
    private int score;
    private int scoreGoal = 3;
    private int difficultyLevel;
    private Coroutine tutorialCoroutine, drawingCardsCoroutine;
    private EmotionsGenerator generator;
    private bool tutorialCardsDrawn = false;

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

            AudioClip start;

            if (Random.Range(0,1) < 0.5f) {
                start = voData.FindVO ("emotions_start");
            } else {
                start = voData.FindVO ("matchemotions");
            }

            SoundManager.Instance.PlayVoiceOverClip (start);

            StartCoroutine (DuringCountdown ());
        }
	}

	public IEnumerator DuringCountdown() {
        DrawCards (0.5f);
        yield return new WaitForSeconds (0.5f);

        SubtitlePanel.Instance.Display ("Find the emotion that match mine!", null);
        yield return new WaitForSeconds (2.5f);
        
        StartCountdown (PostCountdownSetup);
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

		SoundManager.Instance.StopPlayingVoiceOver();
        AudioClip tutorial1a = voData.FindVO ("1a_tutorial_start");
        AudioClip tutorial1b = voData.FindVO ("1b_tutorial_start");
        AudioClip tutorial1c = voData.FindVO ("1c_tutorial_start");
        AudioClip tutorial1d = voData.FindVO ("1d_tutorial_start");
        AudioClip tutorial1e = voData.FindVO ("1e_tutorial_start");

        SubtitlePanel.Instance.Display ("Welcome to Monster Feelings!", tutorial1a, false, tutorial1a.length);
        generator.cardHand.gameObject.SetActive (true);
        yield return new WaitForSeconds(tutorial1a.length);

        generator.SelectTutorialEmotions ();
        SubtitlePanel.Instance.Display ("Our emotions keep us safe and help us take care of one another!", tutorial1b, false, tutorial1b.length + 0.5f);
        yield return new WaitForSeconds (tutorial1b.length + 0.5f);

        SubtitlePanel.Instance.Display ("I'm going to make a face!", tutorial1c, false, tutorial1c.length + 1.0f);
        yield return new WaitForSeconds (tutorial1c.length);

        ChangeMonsterEmotion (generator.GetSelectedEmotion ());
        yield return new WaitForSeconds (1f);

        generator.MoveDeckToScreen ();
        SubtitlePanel.Instance.Display ("Click on the monster's face that matches the feeling you see on my face!", tutorial1d, false, tutorial1d.length);
        yield return new WaitForSeconds (1f);

        tutorialCardsDrawn = true;
        TutorialDrawCards ();
        yield return new WaitForSeconds (tutorial1d.length - 1f);

        generator.MoveDeckOutOfScreen ();
        SubtitlePanel.Instance.Display ("Click the card above me, like this!", tutorial1e, false, tutorial1e.length);
        yield return new WaitForSeconds (tutorial1e.length);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("EM_HandMoveMonster");
		yield return new WaitForSeconds(1.75f);

		SubtitlePanel.Instance.Display (generator.GetSelectedEmotion().ToString(), null);
        SoundManager.Instance.PlayCorrectSFX ();
        yield return new WaitForSeconds(2.0f);

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
        yield return new WaitForSeconds (0.5f);

        if (tutorialCardsDrawn)
            generator.MoveDeckToScreen ();
        yield return new WaitForSeconds (1.0f);

        if (monster)
            ChangeMonsterEmotion (DataType.MonsterEmotions.Joyous);
        generator.RemoveCards ();
        AudioClip letsplay = voData.FindVO ("letsplay");
        SoundManager.Instance.StopPlayingVoiceOver ();
        SubtitlePanel.Instance.Display ("Perfect! Let's play!", letsplay);
		yield return new WaitForSeconds(letsplay.length);

        if (tutorialCardsDrawn)
            generator.MoveDeckOutOfScreen ();

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
        yield return new WaitForSeconds (0.5f);

        generator.MoveDeckToScreen ();
        yield return new WaitForSeconds (0.5f);

        ChangeMonsterEmotion (DataType.MonsterEmotions.Happy);
        generator.RemoveCards ();

        AudioClip end = voData.FindVO ("emotions_end");
        string endText = "Nice job! You matched the emotions correctly!";
        if (!GameManager.Instance.GetIsStickerUnlocked(typeOfGame)) {
            end = voData.FindVO ("emotions_end3");
            endText = "Great job! Recognizing emotions happens mainly in your brain's hippocampus!";
        }
        else {
            if (Random.Range (0f, 1f) <= 0.5f) {
                end = voData.FindVO ("emotions_end2");
                endText = "You helped me recognize others' emotions!";
            }
        }

        yield return new WaitForSeconds (0.5f);

        SubtitlePanel.Instance.Display (endText, end, true, end.length);
        yield return new WaitForSeconds (0.5f);

        generator.MoveDeckOutOfScreen ();
        if (generator.cardHand.gameObject.activeSelf)
            generator.cardHand.ExitAnimation ();
        yield return new WaitForSeconds (end.length - 1.0f);

        if (score >= scoreGoal) {
            if (GameManager.Instance.GetLevel (typeOfGame) == 1) {
                MilestoneManager.Instance.UnlockMilestone (DataType.Milestone.MonsterEmotions1);
            } else if (GameManager.Instance.GetLevel (typeOfGame) == 3) {
                MilestoneManager.Instance.UnlockMilestone (DataType.Milestone.MonsterEmotions3);
            }

            if (difficultyLevel == 1) {
                GameOver (DataType.GameEnd.EarnedSticker);
                SoundManager.Instance.AddToVOQueue (voData.FindVO ("emotion_sticker"));
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
            SubtitlePanel.Instance.Display (emotion.ToString ());
            SoundManager.Instance.AddToVOQueue (clip);
            if (emotion == generator.GetSelectedEmotion ()) {
                TimerClock.Instance.StopTimer ();
                SoundManager.Instance.PlayCorrectSFX ();
                if (Random.Range (0, 1f) < 0.3f) {
                    SoundManager.Instance.AddToVOQueue (goodjobClips.GetRandomItem ());
                }
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
                SoundManager.Instance.PlayIncorrectSFX ();
                SoundManager.Instance.AddToVOQueue (wrongClips.GetRandomItem ());
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
        else {
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
        monster.transform.localScale = new Vector3 (0.6f, 0.6f, 0.6f);
        monster.gameObject.AddComponent<Animator> ();
        monster.GetComponent<Monster> ().IdleAnimationOn = false;
        monster.GetComponent<Monster> ().AllowMonsterTickle = false;
    }

    public void ChangeMonsterEmotion (DataType.MonsterEmotions emo) {
        monster.GetComponent<Monster> ().ChangeEmotions (emo);
    }
}
