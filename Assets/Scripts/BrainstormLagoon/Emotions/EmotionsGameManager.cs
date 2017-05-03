using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager {

	private static EmotionsGameManager instance;
	private int score;
	private int scoreGoal = 10;
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
	private float timeLimit = 30;

	public bool gameStarted = false;
	public Text timerText;
	public Slider scoreGauge;
	public Timer timer;
	public List<GameObject> blueEmotions;
	public List<GameObject> greenEmotions;
	public List<GameObject> redEmotions;
	public List<GameObject> yellowEmotions;
	public GameObject subtitlePanel;
	public Transform[] emotionSpawnLocs;
	public Transform emotionSpawnParent;
	public Transform emotionToMatchSpawnParent;
	public Canvas gameOverCanvas;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		secondaryEmotions = new List<GameObject>();


		// Checks if game manager exists, if not default values are chosen for testing simplicity
		if (GameManager.GetInstance ()) {

			if (GameManager.GetInstance ().getMonster ().Contains ("Blue")) {
				primaryEmotions = blueEmotions;
			} else {
				secondaryEmotions.AddRange (blueEmotions);
			}
			if (GameManager.GetInstance ().getMonster ().Contains ("Green")) {
				primaryEmotions = greenEmotions;
			} else {
				secondaryEmotions.AddRange (greenEmotions);
			}
			if (GameManager.GetInstance ().getMonster ().Contains ("Red")) {
				primaryEmotions = redEmotions;
			} else {
				secondaryEmotions.AddRange (redEmotions);
			}
			if (GameManager.GetInstance ().getMonster ().Contains ("Yellow")) {
				primaryEmotions = yellowEmotions;
			} else {
				secondaryEmotions.AddRange (yellowEmotions);
			}

			difficultyLevel = GameManager.GetInstance ().GetLevel ("MonsterEmotions");
		} else {
			primaryEmotions = greenEmotions;
			secondaryEmotions.AddRange (blueEmotions);
			secondaryEmotions.AddRange (redEmotions);
			secondaryEmotions.AddRange (yellowEmotions);
			difficultyLevel = 1;
		}
		emotionsSetup = new Dictionary<int, Tuple<int, int>>()
		{
			{1, new Tuple<int, int>(2, 0)},
			{2, new Tuple<int, int>(3, 0)},
			{3, new Tuple<int, int>(4, 0)},
			{4, new Tuple<int, int>(3, 1)},
			{5, new Tuple<int, int>(2, 2)}
		};
	}

	public static EmotionsGameManager GetInstance() {
		return instance;
	}



	// Use this for initialization
	void Start () {

//		if(GameManager.GetInstance().LagoonReview) {
//			StartReview();
//		}
//		else {
			PregameSetup();
//		}

	}

	void PregameSetup ()
	{
		score = 0;
		scoreGauge.maxValue = scoreGoal;
		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(this.timeLimit);
		}

		numEmotions = emotionsSetup[difficultyLevel];
		activeEmotions = new List<GameObject> ();
		UpdateScoreGauge ();

		StartCoroutine(DisplayGo());
	}

	public IEnumerator DisplayGo() {
		StartCoroutine(gameObject.GetComponent<Countdown>().RunCountdown());
		yield return new WaitForSeconds (5.0f);
		PostCountdownSetup ();
	}

	private void PostCountdownSetup() {
		for(int i = 0; i < numEmotions.first + numEmotions.second; ++i) {
			emotionSpawnLocs[i].gameObject.SetActive(true);
		}

		ChooseEmotions (primaryEmotions, numEmotions.first);
		ChooseEmotions(secondaryEmotions, numEmotions.second);
		SpawnEmotions (scale);
		ChooseActiveEmotion ();

		timer.StartTimer();
		StartGame();
	}

	private void StartGame () {
		scoreGauge.gameObject.SetActive(true);
		timerText.gameObject.SetActive(true);
		subtitlePanel.SetActive (true); // Added by CT
		gameStarted = true;
	}

	// Update is called once per frame
	void Update () {
		
		if((score >= 10 && !gameOver) || timer.TimeRemaining() <= 0.0f)
			GameOver();
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	public void CheckEmotion(GameObject emotion){
		if(emotion.name == currentEmotionToMatch.name){
			++score;
			UpdateScoreGauge();

			Destroy(currentEmotionToMatch);

			for(int i = 0; i < activeEmotions.Count; ++i) {
				GameObject tmp = emotionSpawnParent.FindChild(activeEmotions[i].name).gameObject;
				Destroy(tmp.gameObject);
			}

			activeEmotions.Clear();
			ChooseEmotions(primaryEmotions, numEmotions.first);
			ChooseEmotions(secondaryEmotions, numEmotions.second);
			ChooseActiveEmotion();
			SpawnEmotions(scale);
		}
			
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

//		}

	}

	private void UpdateScoreGauge(){
		scoreGauge.value = score;
	}
		

	override public void GameOver(){
		gameOver = true;
		//GameManager.GetInstance().AddLagoonReviewGame("MonsterEmotionsReviewGame");
		if(difficultyLevel == 1) {
//			stickerPopupCanvas.gameObject.SetActive(true);
//			GameManager.GetInstance().ActivateBrainstormLagoonReview();
//			if(GameManager.GetInstance().LagoonFirstSticker) {
//				stickerPopupCanvas.transform.FindChild("StickerbookButton").gameObject.SetActive(true);
//				GameManager.GetInstance().LagoonFirstSticker = false;
//				Debug.Log ("This was Brainstorm Lagoon's first sticker");
//			}
//			else {
//				Debug.Log ("This was not Brainstorm Lagoon's first sticker");
//				stickerPopupCanvas.transform.FindChild("BackButton").gameObject.SetActive(true);
//			}
//			GameManager.GetInstance().ActivateSticker("BrainstormLagoon", "");
//			GameManager.GetInstance ().LagoonTutorial[(int)Constants.BrainstormLagoonLevels.MONSTER_EMOTIONS] = false;
		}
			
		GameManager.GetInstance().LevelUp("MonsterEmotions");

		DisplayGameOverPopup();
	}

	public void DisplayGameOverPopup () {
		//Debug.Log ("In DisplayGameOverPopup");
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " emotions!";
	}


}
