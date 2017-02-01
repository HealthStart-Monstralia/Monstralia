using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager {

	//private string currentEmotionToMatch;
	private static EmotionsGameManager instance;
	private int score = 0;
	private List<GameObject> activeEmotions;
	private GameObject currentEmotionToMatch;
	private float scale = 40f;

	public Text scoreText;
	public List<GameObject> emotions;
	public List<GameObject> blueEmotions;
	public List<GameObject> greenEmotions;
	public List<GameObject> redEmotions;
	public List<GameObject> yellowEmotions;
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

		if(GameManager.GetInstance().getMonster().Contains("Blue")) {
			emotions = blueEmotions;
		}
		else if(GameManager.GetInstance().getMonster().Contains("Green")) {
			emotions = greenEmotions;
		}
		else if(GameManager.GetInstance().getMonster().Contains("Red")) {
			emotions = redEmotions;
		}
		else if(GameManager.GetInstance().getMonster().Contains("Yellow")) {
			emotions = yellowEmotions;
		}
	}

	public static EmotionsGameManager GetInstance() {
		return instance;
	}



	// Use this for initialization
	void Start () {
		activeEmotions = new List<GameObject>();

		UpdateScore();
		ChooseEmotions();
		SpawnEmotions(scale);
		ChooseActiveEmotion();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(score >= 10)
			GameOver();
	}

	public void CheckEmotion(GameObject emotion){
		if(emotion.name == currentEmotionToMatch.name){
			++score;
			UpdateScore();

			Destroy(currentEmotionToMatch);

			for(int i = 0; i < emotionSpawnLocs.Length; ++i) {
				GameObject tmp = emotionSpawnParent.FindChild(activeEmotions[i].name).gameObject;
				Destroy(tmp);
			}

			activeEmotions.Clear();
			ChooseEmotions();
			ChooseActiveEmotion();
			SpawnEmotions(scale);
		}
			
	}

	private void ChooseEmotions(){
		int emotionCount = 0;
		while(emotionCount < 4){
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
			newEmotion.GetComponent<SpriteRenderer>().sortingOrder = 1;
			++spawnCount;
			Debug.Log("Spawn count: " + spawnCount);
		}

	}

	private void ChooseActiveEmotion() {
		int tmp = Random.Range(0, activeEmotions.Count);
		currentEmotionToMatch = Instantiate(activeEmotions[tmp]);
		currentEmotionToMatch.name = activeEmotions[tmp].name;
		currentEmotionToMatch.transform.SetParent(emotionToMatchSpawnParent);
		currentEmotionToMatch.transform.localPosition = new Vector3(0f, 0f, 0f);
		currentEmotionToMatch.transform.localScale = new Vector3(40f, 40f, 1f);
		currentEmotionToMatch.GetComponent<SpriteRenderer>().sortingOrder = 1;

		currentEmotionToMatch.GetComponent<BoxCollider2D>().enabled = false;
		currentEmotionToMatch.GetComponent<EmotionBehavior>().enabled = false;

//		}

	}

	private void UpdateScore(){
		scoreText.text = "Score: " + score;
	}
		

	override public void GameOver(){
		DisplayGameOverPopup();
	}

	public void DisplayGameOverPopup () {
		//Debug.Log ("In DisplayGameOverPopup");
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " emotions!";
	}


}
