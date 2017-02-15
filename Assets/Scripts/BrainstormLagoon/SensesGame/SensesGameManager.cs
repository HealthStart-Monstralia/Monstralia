using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SensesGameManager : AbstractGameManager {

	private static SensesGameManager instance;
	private int difficultyLevel;
	private int score;
	private bool gameOver = false;
	private string[] senses = {"feel", "taste", "smell", "see", "hear"};
	private Dictionary<int, int> sensesSetup;
	private int numSenses;
	private List<GameObject> activeSenses;
	private GameObject currentSenseToMatch;
	public float scale = 40;

	public Text scoreText;
	public Canvas gameOverCanvas;
	public List<Sprite> sensesSprites;
	public List<GameObject> see;
	public List<GameObject> smell;
	public List<GameObject> hear;
	public List<GameObject> taste;
	public List<GameObject> feel;
	public List<GameObject> allSenses;
	public Transform[] senseSpawnLocs;
	public Transform senseSpawnParent;
	public Transform senseToMatchSpawnParent;

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
		Debug.Log("difficultyLEvel: " + difficultyLevel);
		numSenses = sensesSetup[difficultyLevel];
		activeSenses = new List<GameObject> ();
		UpdateScore ();

		for(int i = 0; i < numSenses; ++i) {
			senseSpawnLocs[i].gameObject.SetActive(true);
		}

		string activeSense = ChooseActiveSense();
		ChooseSenses (activeSense, numSenses-1);
		SpawnSenses (scale);
//		ChooseActiveEmotion ();
	}
	
	// Update is called once per frame
	void Update () {
		if(score >= 10 && !gameOver)
			GameOver();
	}

	private string ChooseActiveSense() {
		string senseToMatch = senses[Random.Range(0, senses.Length)];

		if(senseToMatch == "see") {
			int randomIndex = Random.Range(0, see.Count);
			currentSenseToMatch = see[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "hear") {
			int randomIndex = Random.Range(0, see.Count);
			currentSenseToMatch = hear[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "taste") {
			int randomIndex = Random.Range(0, see.Count);
			currentSenseToMatch = taste[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "feel") {
			int randomIndex = Random.Range(0, see.Count);
			currentSenseToMatch = feel[randomIndex];
			activeSenses.Add(currentSenseToMatch);
		}
		else if(senseToMatch == "smell") {
			int randomIndex = Random.Range(0, see.Count);
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
			if(!activeSenses.Contains(newEmotion) && newEmotion.GetComponent<SenseObjectBehavior>().sense.ToString() != activeSense){
				activeSenses.Add(newEmotion);
				++senseCount;
			}

		}
	}

	private void SpawnSenses(float scale) {
		int spawnCount = 0;
		for(int i = 0; i < activeSenses.Count; ++i) {
			GameObject newEmotion = Instantiate(activeSenses[i]);
			newEmotion.name = activeSenses[i].name;
			newEmotion.transform.SetParent(senseSpawnParent);
			newEmotion.transform.localPosition = senseSpawnLocs[i].localPosition;
			newEmotion.transform.localScale = new Vector3(scale, scale, 1f);
			newEmotion.GetComponent<SpriteRenderer>().sortingOrder = 2;
			++spawnCount;
		}

	}

	private void UpdateScore(){
		scoreText.text = "Score: " + score;
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

		GameManager.GetInstance().LevelUp("MonsterSenses");

		DisplayGameOverPopup();
	}

	public void DisplayGameOverPopup () {
		//Debug.Log ("In DisplayGameOverPopup");
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " senses!";
	}
}
