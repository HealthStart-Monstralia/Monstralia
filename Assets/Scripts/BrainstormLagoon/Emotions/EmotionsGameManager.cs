using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EmotionsGameManager : AbstractGameManager {

	private string currentEmotionToMatch;
	private int score = 0;
	private string[] emotions = {"red", "blue", "yellow", "green"};

	public Text scoreText;
	public Canvas gameOverCanvas;

	// Use this for initialization
	void Start () {
		UpdateScore();
		ChooseEmotion();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(score >= 10)
			GameOver();
	}

	public void CheckEmotion(string emotion){
		if(emotion == currentEmotionToMatch){
			++score;
			UpdateScore();
			// generate new currentEmotionToMatch
			// currentEmotionToMatch = ChooseEmotion();
			ChooseEmotion();
		}
			
	}

	private void ChooseEmotion(){
		GameObject.Find("EmotionsMatchPanel").GetComponentInChildren<Text>().text = emotions[Random.Range(0,4)];
		currentEmotionToMatch = GameObject.Find("EmotionsMatchPanel").GetComponentInChildren<Text>().text;
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
