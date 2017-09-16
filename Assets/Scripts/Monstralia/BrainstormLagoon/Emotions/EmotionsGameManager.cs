using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager {
	public enum MonsterEmotions	{
		Happy = 0,
		Afraid = 1,
		Disgusted = 2,
		Joyous = 3,
		Mad = 4,
		Sad = 5,
		Thoughtful = 6,
		Worried = 7
	};
			
	private static EmotionsGameManager instance;
	private int score;
	private int scoreGoal = 3;
	private List<GameObject> primaryEmotions;
	private List<GameObject> secondaryEmotions;
	private List<GameObject> activeEmotions;
	private GameObject currentEmotionToMatch;
	private float scale = 40f;
	private float toMatchScale = 70f;
	private int difficultyLevel;
	private Dictionary<int, Tuple<int, int>> emotionsSetup;
	private Tuple<int, int> numEmotions;
	private bool gameOver = false;
	private Coroutine tutorialCoroutine;

    public VoiceOversData voData;
	public float timeLimit = 30;
	public bool gameStarted = false;
	public Text timerText;
	public Slider scoreGauge;
	public Timer timer;
	public GameManager.MonsterType monsterType;
	public List<GameObject> blueEmotions;
	public List<GameObject> greenEmotions;
	public List<GameObject> redEmotions;
	public List<GameObject> yellowEmotions;
	public GameObject subtitlePanel;
	public Transform[] emotionSpawnLocs;
	public Transform emotionSpawnParent;
	public Transform emotionToMatchSpawnParent;
	public Canvas gameOverCanvas;
	public GameObject backButton;
	public float waitDuration = 3f;
	public static bool inputAllowed = false;
	public AudioClip[] answerSounds;
	public AudioClip[] instructionsVO;
	public bool isTutorialRunning = false;
	public GameObject tutorialHand;
	public Canvas tutorialCanvas;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

        CheckForGameManager ();

		tutorialCanvas.gameObject.SetActive (false);
		tutorialHand.SetActive (false);

		secondaryEmotions = new List<GameObject> ();

		// Checks if game manager exists, if not default values are chosen
		if (GameManager.GetInstance ()) {
			monsterType = GameManager.GetMonsterType ();

			if (monsterType == GameManager.MonsterType.Blue) {
				primaryEmotions = blueEmotions;
			} else {
				secondaryEmotions.AddRange (blueEmotions);
			}
			if (monsterType == GameManager.MonsterType.Green) {
				primaryEmotions = greenEmotions;
			} else {
				secondaryEmotions.AddRange (greenEmotions);
			}
			if (monsterType == GameManager.MonsterType.Red) {
				primaryEmotions = redEmotions;
			} else {
				secondaryEmotions.AddRange (redEmotions);
			}
			if (monsterType == GameManager.MonsterType.Yellow) {
				primaryEmotions = yellowEmotions;
			} else {
				secondaryEmotions.AddRange (yellowEmotions);
			}

			difficultyLevel = GameManager.GetInstance ().GetLevel (DataType.Minigame.MonsterEmotions);
		} else {
			monsterType = GameManager.GetMonsterType ();
			primaryEmotions = greenEmotions;
			secondaryEmotions.AddRange (blueEmotions);
			secondaryEmotions.AddRange (redEmotions);
			secondaryEmotions.AddRange (yellowEmotions);
			difficultyLevel = 1;
		}

		emotionsSetup = new Dictionary<int, Tuple<int, int>> () {
			{ 1, new Tuple<int, int> (2, 0) },
			{ 2, new Tuple<int, int> (3, 0) },
			{ 3, new Tuple<int, int> (4, 0) },
			{ 4, new Tuple<int, int> (3, 1) },
			{ 5, new Tuple<int, int> (2, 2) }
		};
	}

	public static EmotionsGameManager GetInstance() {
		return instance;
	}


	public override void PregameSetup () {
        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.MonsterEmotions)) {
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
            scoreGauge.maxValue = scoreGoal;
            if (timer != null) {
                timerText.gameObject.SetActive (true);
                timer.SetTimeLimit (this.timeLimit);
                timer.StopTimer ();
            }

            numEmotions = emotionsSetup[difficultyLevel];
            activeEmotions = new List<GameObject> ();
            UpdateScoreGauge ();

            timerText.text = "Time: " + timer.TimeRemaining ();
            StartCoroutine (DisplayGo ());
        }
	}

	public IEnumerator DisplayGo() {
		GameManager.GetInstance ().Countdown ();
		yield return new WaitForSeconds (5.0f);
		PostCountdownSetup ();
	}

	IEnumerator RunTutorial () { 
		print ("RunTutorial");
		isTutorialRunning = true;
		tutorialCanvas.gameObject.SetActive (true);
		inputAllowed = false;
		scoreGauge.gameObject.SetActive (false);
		timer.gameObject.SetActive (false);

		yield return new WaitForSeconds(0.5f);
		subtitlePanel.SetActive (true);

		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Monster Feelings!", null);
		SoundManager.GetInstance().StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("tutorial1");
		SoundManager.GetInstance().PlayVoiceOverClip(tutorial1);

        float secsToRemove = 7f;
		yield return new WaitForSeconds(tutorial1.length - secsToRemove);

		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		GameObject matchEmotion = tutorialCanvas.gameObject.transform.Find ("Tutorial Emotions").Find ("Green_Joyous").gameObject;

		currentEmotionToMatch = Instantiate(matchEmotion);
		currentEmotionToMatch.name = matchEmotion.name;
		currentEmotionToMatch.transform.SetParent(emotionToMatchSpawnParent);
		currentEmotionToMatch.transform.localPosition = new Vector3(0f, 0f, 0f);
		currentEmotionToMatch.transform.localScale = new Vector3(toMatchScale, toMatchScale, 1f);
		currentEmotionToMatch.GetComponent<BoxCollider2D>().enabled = false;
		currentEmotionToMatch.GetComponent<EmotionBehavior>().enabled = false;

		//SoundManager.GetInstance().StopPlayingVoiceOver();
		//SoundManager.GetInstance().PlayVoiceOverClip(instructionsVO[1]);
		//yield return new WaitForSeconds(instructionsVO[1].length - 2.5f);
        yield return new WaitForSeconds (secsToRemove);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("EM_HandMoveMonster");
		yield return new WaitForSeconds(1.75f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Joyous", null);
		SoundManager.GetInstance ().PlaySFXClip (answerSounds [1]);
		yield return new WaitForSeconds(2.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		yield return new WaitForSeconds(1.0f);

        AudioClip nowyoutry = voData.FindVO ("nowyoutry");
        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Now you try!", nowyoutry);
		inputAllowed = true;
	}

	public void TutorialFinished() {
		inputAllowed = false;
		tutorialHand.SetActive (false);
		GameManager.GetInstance ().CompleteTutorial(DataType.Minigame.MonsterEmotions);
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

	IEnumerator TutorialTearDown() {
		print ("TutorialTearDown");
        yield return new WaitForSeconds (1.5f);
        AudioClip letsplay = voData.FindVO ("letsplay");
        SoundManager.GetInstance ().StopPlayingVoiceOver ();
        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Let's play!", letsplay);
		yield return new WaitForSeconds(letsplay.length);

		if (currentEmotionToMatch)
			Destroy(currentEmotionToMatch);
		isTutorialRunning = false;
		tutorialCanvas.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		PregameSetup ();
	}

	public void SkipReviewButton(GameObject button) {
		SkipReview ();
		Destroy (button);
	}

	public void SkipReview() {
		StopCoroutine (tutorialCoroutine);
        TutorialFinished ();
    }

	private void PostCountdownSetup() {
		for(int i = 0; i < numEmotions.first + numEmotions.second; ++i) {
			emotionSpawnLocs[i].gameObject.SetActive(true);
		}

		ChooseEmotions (primaryEmotions, numEmotions.first);
		ChooseEmotions (secondaryEmotions, numEmotions.second);
		SpawnEmotions (scale);
		ChooseActiveEmotion ();
	
		StartGame();
	}

	private void StartGame () {
		scoreGauge.gameObject.SetActive(true);

		timer.StopTimer ();
		timer.StartTimer();
		print (timer);
		gameStarted = true;
		inputAllowed = true;
	}

	// Update is called once per frame
	void Update () {
		//print ("timer.TimeRemaining (): " + timer.TimeRemaining () + " | " + "gameStarted: " + gameStarted + " | " + "score: " + score);
		if (gameStarted && (timer.TimeRemaining () <= 0.0f || score >= scoreGoal)) {
			print ("Postgame calling");
			StartCoroutine(PostGame ());
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	public void CheckEmotion(GameObject emotion){
		timer.StopTimer ();
		inputAllowed = false;
		if (emotion.name == currentEmotionToMatch.name) {
			SoundManager.GetInstance ().PlaySFXClip (answerSounds [1]);
			//SoundManager.GetInstance ().PlayVoiceOverClip (answerSounds [1]);
			if (isTutorialRunning) {
				TutorialFinished ();
			} else {
				++score;
				UpdateScoreGauge ();
				StartCoroutine (CreateNextEmotions (waitDuration));
			}

		} else {
			StartCoroutine (WrongAnswerWait (waitDuration));
		}
	}

	public IEnumerator CreateNextEmotions(float duration) {
		yield return new WaitForSeconds (duration + 0.5f);

		Destroy(currentEmotionToMatch);

		for(int i = 0; i < activeEmotions.Count; ++i) {
			GameObject tmp = emotionSpawnParent.Find(activeEmotions[i].name).gameObject;
			Destroy(tmp.gameObject);
		}

		activeEmotions.Clear();
		ChooseEmotions(primaryEmotions, numEmotions.first);
		ChooseEmotions(secondaryEmotions, numEmotions.second);
		ChooseActiveEmotion();
		SpawnEmotions(scale);
		timer.StartTimer ();
		print (timer);
		inputAllowed = true;
	}

	public IEnumerator WrongAnswerWait (float duration) {
		yield return new WaitForSeconds (duration);
		timer.StartTimer ();
		inputAllowed = true;
	}

	private void ChooseEmotions(List<GameObject> emotions, int num){
		int emotionCount = 0;
		while(emotionCount < num){
			int randomIndex = Random.Range(0, emotions.Count);
			GameObject newEmotion = emotions[randomIndex];
			if(!activeEmotions.Contains(newEmotion)){
				activeEmotions.Add(newEmotion);
				++emotionCount;
			}

		}
	}

	private void SpawnEmotions(float scale) {
		int spawnCount = 0;
		for(int i = 0; i < activeEmotions.Count; ++i) {
			GameObject newEmotion = Instantiate(activeEmotions[i]);
			newEmotion.name = activeEmotions[i].name;
			newEmotion.transform.SetParent(emotionSpawnParent);
			newEmotion.transform.localPosition = emotionSpawnLocs[i].localPosition;
			newEmotion.transform.localScale = new Vector3(scale, scale, 1f);
			newEmotion.GetComponent<SpriteRenderer>().sortingOrder = 2;
			++spawnCount;
		}

	}

	private void ChooseActiveEmotion() {
		int tmp = Random.Range(0, numEmotions.first);
		currentEmotionToMatch = Instantiate(activeEmotions[tmp]);
		currentEmotionToMatch.name = activeEmotions[tmp].name;
		currentEmotionToMatch.transform.SetParent(emotionToMatchSpawnParent);
		currentEmotionToMatch.transform.localPosition = new Vector3(0f, 0f, 0f);
		currentEmotionToMatch.transform.localScale = new Vector3(toMatchScale, toMatchScale, 1f);
		currentEmotionToMatch.GetComponent<SpriteRenderer>().sortingOrder = 1;

		currentEmotionToMatch.GetComponent<BoxCollider2D>().enabled = false;
		currentEmotionToMatch.GetComponent<EmotionBehavior>().enabled = false;

	}

	private void UpdateScoreGauge(){
		scoreGauge.value = score;
	}

	IEnumerator PostGame () {
		print ("PostGame");
		gameStarted = false;
		gameOver = true;
		inputAllowed = false;
		yield return new WaitForSeconds (1.0f);

        AudioClip end = voData.FindVO ("end");

        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Great job! You matched " + score + " emotions!", end);
        yield return new WaitForSeconds (3.0f);
        GameOver ();
	}

	override public void GameOver(){
		if (gameOver) {
			print ("GameOver");
			backButton.SetActive (true);
			if (difficultyLevel == 1) {
				UnlockSticker ();
			} else {
				DisplayGameOverPopup ();
			}
			
			GameManager.GetInstance ().LevelUp (DataType.Minigame.MonsterEmotions);
		}
	}

	public void DisplayGameOverPopup () {
        //Debug.Log ("In DisplayGameOverPopup");
        subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

        gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " emotions!";
	}
}
