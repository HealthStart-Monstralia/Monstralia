using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager<EmotionsGameManager> {
    [HideInInspector] public EmotionsGenerator generator;
    public VoiceOversData voData;
    public Transform monsterLocation;
	public float timeLimit = 30;
	public bool gameStarted = false;
    public bool isTutorialRunning = false;
    public bool inputAllowed = false;
    [HideInInspector] public bool isDrawingCards = false;

	public ScoreGauge scoreGauge;
	public TimerClock timerClock;
	public GameObject subtitlePanel;
	public Transform[] emotionSpawnLocs;
	public GameObject backButton;
	public float waitDuration = 3f;

	public AudioClip[] answerSounds;

	public GameObject tutorialHand;
	public Canvas tutorialCanvas;

    private int score;
    private int scoreGoal = 3;
    private List<GameObject> primaryEmotions;
    private List<GameObject> secondaryEmotions;
    private List<GameObject> activeEmotions;
    private GameObject currentEmotionToMatch;
    private int difficultyLevel;
    private Coroutine tutorialCoroutine, drawingCoroutine;

    public override void PregameSetup () {
        generator = GetComponent<EmotionsGenerator> ();
        tutorialCanvas.gameObject.SetActive (false);
        tutorialHand.SetActive (false);
        generator.cardHand.gameObject.SetActive (false);
        difficultyLevel = GameManager.Instance.GetLevel (DataType.Minigame.MonsterEmotions);

        print ("loc: " + monsterLocation.position);
        if (!generator.monster)
            generator.CreateMonster ();
        generator.ChangeMonsterEmotion (DataType.MonsterEmotions.Happy);
        if (GameManager.Instance.GetPendingTutorial (DataType.Minigame.MonsterEmotions)) {
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {
            switch (difficultyLevel) {
                case 2:
                    scoreGoal = 5;
                    break;
                case 3:
                    scoreGoal = 7;
                    break;
                default:
                    scoreGoal = 3;
                    break;
            }

            score = 0;
            if (timerClock != null) {
                timerClock.SetTimeLimit (timeLimit);
                timerClock.StopTimer ();
            }

            UpdateScoreGauge ();
            timerClock.gameObject.SetActive (true);
            generator.cardHand.gameObject.SetActive (true);
            generator.cardHand.SpawnIn ();
            StartCoroutine (DuringCountdown ());
            StartCountdown (PostCountdownSetup);
        }
	}

	public IEnumerator DuringCountdown() {
        DrawCards (0.5f);
		yield return new WaitForSeconds (3.0f);
        generator.ChangeMonsterEmotion (generator.currentTargetEmotion);
	}

	IEnumerator RunTutorial () { 
		print ("RunTutorial");
		isTutorialRunning = true;
		tutorialCanvas.gameObject.SetActive (true);
		inputAllowed = false;
		scoreGauge.gameObject.SetActive (false);
		timerClock.gameObject.SetActive (false);

		yield return new WaitForSeconds(0.5f);
		subtitlePanel.SetActive (true);

		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Monster Feelings!", null);
		SoundManager.Instance.StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("1_tutorial_start");
		SoundManager.Instance.PlayVoiceOverClip(tutorial1);
        
        float secsToRemove = 6f;
        yield return new WaitForSeconds(tutorial1.length - secsToRemove);
        generator.ChangeMonsterEmotion (DataType.MonsterEmotions.Afraid);
        generator.cardHand.gameObject.SetActive (true);
        subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

        float secsToRemoveAgain = 4f;
        yield return new WaitForSeconds (secsToRemove - secsToRemoveAgain);
        TutorialDrawCards ();
        
        yield return new WaitForSeconds (secsToRemoveAgain);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("EM_HandMoveMonster");
		yield return new WaitForSeconds(1.75f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display (generator.currentTargetEmotion.ToString(), null);
		SoundManager.Instance.PlaySFXClip (answerSounds [1]);
		yield return new WaitForSeconds(2.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		yield return new WaitForSeconds(1.0f);

        AudioClip nowyoutry = voData.FindVO ("nowyoutry");
        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Now you try!", nowyoutry);
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
        if (isDrawingCards)
            StopCoroutine (drawingCoroutine);

        yield return new WaitForSeconds (1.5f);
        generator.ChangeMonsterEmotion (DataType.MonsterEmotions.Joyous);
        generator.RemoveCards ();
        AudioClip letsplay = voData.FindVO ("letsplay");
        SoundManager.Instance.StopPlayingVoiceOver ();
        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Let's play!", letsplay);
		yield return new WaitForSeconds(letsplay.length);
        if (generator.cardHand.gameObject.activeSelf)
            generator.cardHand.ExitAnimation ();
        tutorialCanvas.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		PregameSetup ();
	}

	private void PostCountdownSetup() {
		StartGame();
	}

    private void StartGame () {
        scoreGauge.gameObject.SetActive (true);

        timerClock.StopTimer ();
        timerClock.StartTimer ();

        gameStarted = true;
        inputAllowed = true;
    }

    IEnumerator PostGame () {
        print ("PostGame");
        gameStarted = false;
        StopCoroutine (drawingCoroutine);
        inputAllowed = false;
        yield return new WaitForSeconds (1.0f);
        generator.RemoveCards ();
        AudioClip end = voData.FindVO ("end");

        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Great job! You matched " + score + " emotions!", end);
        yield return new WaitForSeconds (3.0f);
        if (generator.cardHand.gameObject.activeSelf)
            generator.cardHand.ExitAnimation ();
        yield return new WaitForSeconds (1.0f);

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

    public void CheckEmotion (DataType.MonsterEmotions emotion) {
        PauseGame ();
        if (emotion == generator.currentTargetEmotion) {
            SoundManager.Instance.PlaySFXClip (answerSounds[1]);
            if (isTutorialRunning) {
                TutorialFinished ();
            } else {
                ++score;
                UpdateScoreGauge ();
                if (gameStarted) {
                    DrawCards (waitDuration);
                    if (score >= scoreGoal) {
                        StartCoroutine (PostGame ());
                    }
                }
            }

        } else {
            StartCoroutine (WrongAnswerWait (waitDuration));
        }
    }

    public void DrawCards(float waitPeriod) {
        isDrawingCards = true;
        drawingCoroutine = StartCoroutine (generator.CreateNextEmotions (waitPeriod));
    }

    public void TutorialDrawCards () {
        isDrawingCards = true;
        drawingCoroutine = StartCoroutine (generator.CreateTutorialCards ());
    }

    public void ContinueGame() {
        if (gameStarted) {
            generator.ChangeMonsterEmotion (generator.currentTargetEmotion);
            inputAllowed = true;
            timerClock.StartTimer ();
        }
    }

    public void PauseGame() {
        timerClock.StopTimer ();
        inputAllowed = false;
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
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / scoreGoal);
    }

    public void SkipReviewButton (GameObject button) {
        SkipReview ();
        Destroy (button);
    }

    public void SkipReview () {
        StopCoroutine (tutorialCoroutine);
        TutorialFinished ();
    }
}
