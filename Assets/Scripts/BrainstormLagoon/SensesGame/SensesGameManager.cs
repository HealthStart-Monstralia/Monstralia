using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SensesGameManager : AbstractGameManager {

	private static SensesGameManager instance;
	private int difficultyLevel;
	private int score;
	private int scoreGoal = 10;
	private bool gameOver = false;
	private string[] senses = {"touch", "taste", "smell", "see", "hear"};
	private Dictionary<int, int> sensesSetup;
	private int numSenses;
	private List<GameObject> activeSenses;
	private GameObject currentSenseToMatch;
	private float scale = 40;
	private  float timeLimit = 30;

	public bool gameStarted = false;
	public Slider scoreGauge;
	public Text timerText;
	public Timer timer;
	public Canvas gameOverCanvas;
	public List<Sprite> sensesSprites;
	public List<GameObject> see;
	public List<GameObject> smell;
	public List<GameObject> hear;
	public List<GameObject> taste;
	public List<GameObject> touch;
	public List<GameObject> allSenses;
	public Transform[] senseSpawnLocs;
	public Transform senseSpawnParent;
	public GameObject senseToMatchSprite;
	public Text senseToMatchText;
//	public AudioClip waterTip;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		difficultyLevel = GameManager.GetInstance().GetLevel("MonsterSenses");

		sensesSetup = new Dictionary<int, int>()
		{
			{1, 2},
			{2, 3},
			{3, 4},
			{4, 4},
			{5, 4}
		};
	}

	public static SensesGameManager GetInstance() {
		return instance;
	}

	// Use this for initialization
	void Start () {
		PregameSetup();
	}

	void PregameSetup ()
	{
		score = 0;
		scoreGauge.maxValue = scoreGoal;
		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(this.timeLimit);
		}

		numSenses = sensesSetup[difficultyLevel];
		activeSenses = new List<GameObject> ();
		UpdateScoreGauge ();

		StartCoroutine(DisplayGo ());
	}

	public IEnumerator DisplayGo () {
		GameManager.GetInstance ().Countdown ();
		yield return new WaitForSeconds (5.0f);
		PostCountdownSetup ();
	}

	void PostCountdownSetup ()
	{
//		if(difficultyLevel == 1){
//			SoundManager.GetInstance().PlayVoiceOverClip(waterTip);
//
//		}
		for(int i = 0; i < numSenses; ++i) {
			senseSpawnLocs[i].gameObject.SetActive(true);
		}

		string activeSense = ChooseActiveSense();
		ChooseSenses (activeSense, numSenses-1);
		SpawnSenses (scale);

		senseToMatchText.enabled = true;
		timer.StartTimer ();
		StartGame();
	}

	private void StartGame () {
		scoreGauge.gameObject.SetActive(true);
		timerText.gameObject.SetActive(true);
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

	private string ChooseActiveSense() {
		string senseToMatch = senses[Random.Range(0, senses.Length)];

		senseToMatchText.text = "Which image can you " + senseToMatch + "?";

		for(int i = 0; i < sensesSprites.Count; ++i) {
			if(sensesSprites[i].name.ToLower() == senseToMatch) {
				senseToMatchSprite.GetComponent<SpriteRenderer>().sprite = sensesSprites[i];
				senseToMatchSprite.SetActive(true);
			}
		}
			
		if(senseToMatch == "see") {
			int randomIndex = Random.Range(0, see.Count);
			currentSenseToMatch = see[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "hear") {
			int randomIndex = Random.Range(0, hear.Count);
			currentSenseToMatch = hear[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "taste") {
			int randomIndex = Random.Range(0, taste.Count);
			currentSenseToMatch = taste[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "touch") {
			int randomIndex = Random.Range(0, touch.Count);
			currentSenseToMatch = touch[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "smell") {
			int randomIndex = Random.Range(0, smell.Count);
			currentSenseToMatch = smell[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}

		return senseToMatch;
	}

	private void ChooseSenses(string activeSense, int num){
		int senseCount = 0;

		while(senseCount < num){
			int randomIndex = Random.Range(0, allSenses.Count);
			GameObject newEmotion = allSenses[randomIndex];
			if(!activeSenses.Contains(newEmotion) && newEmotion.GetComponent<SenseObjectBehavior>().sense.ToString().ToLower() != activeSense.ToLower()){
				activeSenses.Add(newEmotion);
				++senseCount;
			}

		}
	}

	private void SpawnSenses(float scale) {
		int spawnCount = 0;
		List<GameObject> copy = new List<GameObject>(activeSenses);
		int numSensesToSpawn = activeSenses.Count;
		while(spawnCount < numSensesToSpawn) {
			int rndIndex = Random.Range(0, copy.Count);
			GameObject newEmotion = Instantiate(copy[rndIndex]);
			newEmotion.name = copy[rndIndex].name;
			newEmotion.transform.SetParent(senseSpawnParent);
			newEmotion.transform.localPosition = senseSpawnLocs[spawnCount].localPosition;
			newEmotion.transform.localScale = new Vector3(scale, scale, 1f);
			newEmotion.GetComponent<SpriteRenderer>().sortingOrder = 2;

			copy.RemoveAt(rndIndex);
			++spawnCount;
		}

	}

	public void CheckSense(GameObject other) {
		if(other.name == currentSenseToMatch.name){
			++score;
			UpdateScoreGauge();

			for(int i = 0; i < activeSenses.Count; ++i) {
				GameObject tmp = senseSpawnParent.FindChild(activeSenses[i].name).gameObject;
				Destroy(tmp);
			}

			activeSenses.Clear();
			string activeSense = ChooseActiveSense();
			ChooseSenses(activeSense, numSenses-1);
			SpawnSenses(scale);
		}

	}

	private void UpdateScoreGauge(){
		scoreGauge.value =  score;
	}

	override public void GameOver(){
		if (!gameOver) {
			gameOver = true;
			//GameManager.GetInstance().AddLagoonReviewGame("MonsterEmotionsReviewGame");
			if (difficultyLevel == 1) {
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

			GameManager.GetInstance ().LevelUp ("MonsterSenses");

			DisplayGameOverPopup ();
		}
	}

	public void DisplayGameOverPopup () {
		//Debug.Log ("In DisplayGameOverPopup");
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " senses!";
	}
}
