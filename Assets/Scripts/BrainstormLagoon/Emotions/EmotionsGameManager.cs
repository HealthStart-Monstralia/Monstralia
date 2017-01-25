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
	//private string[] emotions = {"worried", "happy", "afraid", "thoughtful"};

	public Text scoreText;
	public List<GameObject> emotions;
	public Transform[] emotionSpawnLocs;
	public Transform emotionSpawnParent;
	public Canvas gameOverCanvas;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}

	public static EmotionsGameManager GetInstance() {
		return instance;
	}



	// Use this for initialization
	void Start () {
		activeEmotions = new List<GameObject>();


		UpdateScore();
		ChooseEmotion();
		SpawnEmotions(40f);
		ChooseActiveEmotion();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(score >= 10)
			GameOver();
	}

	public void CheckEmotion(GameObject emotion){
		Debug.Log("IN CHECK EMOTION " + currentEmotionToMatch.name);
		if(emotion.name == currentEmotionToMatch.name){
			Debug.Log("----------MATCH-----------");
			++score;
			UpdateScore();
			// generate new currentEmotionToMatch
			// currentEmotionToMatch = ChooseEmotion();
			ChooseActiveEmotion();
		}
			
	}

	private void ChooseEmotion(){
		int emotionCount = 0;
		while(emotionCount < 4){
			int randomIndex = Random.Range(0, emotions.Count);
			GameObject newEmotion = emotions[randomIndex];
			if(!activeEmotions.Contains(newEmotion)){
				activeEmotions.Add(newEmotion);
				++emotionCount;
			}
		//GameObject.Find("EmotionsMatchPanel").GetComponentInChildren<Text>().text = emotions[Random.Range(0,4)];
		//currentEmotionToMatch = GameObject.Find("EmotionsMatchPanel").GetComponentInChildren<Text>().text;
		}
	}

	private void SpawnEmotions(float scale) {
		Debug.Log("In Spawn, size of active: " + activeEmotions.Count);
		for(int i = 0; i < activeEmotions.Count; ++i) {
			GameObject newEmotion = Instantiate(activeEmotions[i]);
			newEmotion.name = activeEmotions[i].name;
			newEmotion.transform.SetParent(emotionSpawnParent);
			newEmotion.transform.localPosition = emotionSpawnLocs[i].localPosition;
			newEmotion.transform.localScale = new Vector3(scale, scale, 1f);
			newEmotion.GetComponent<SpriteRenderer>().sortingOrder = 1;
		}

	}

	private void ChooseActiveEmotion() {
//		if(GameObject.Find ("EmotionsMatchPanel").transform.childCount > 0)
//			Destroy(currentEmotionToMatch);

//		if(activeEmotions.Count > 0) {
			int tmp = Random.Range(0, activeEmotions.Count);
			currentEmotionToMatch = Instantiate(activeEmotions[tmp]);
			currentEmotionToMatch.name = activeEmotions[tmp].name;
		Debug.Log("CurrentEmotionToMatch " + currentEmotionToMatch.name);
			currentEmotionToMatch.transform.SetParent(emotionSpawnParent);
			currentEmotionToMatch.transform.localPosition = GameObject.Find("EmotionsMatchPanel").transform.localPosition;
			currentEmotionToMatch.transform.localScale = new Vector3(27f, 27f, 1f);
			currentEmotionToMatch.GetComponent<SpriteRenderer>().sortingOrder = 1;

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
