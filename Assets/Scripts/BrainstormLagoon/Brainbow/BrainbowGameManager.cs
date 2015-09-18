using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : MonoBehaviour {

	private static BrainbowGameManager instance;
	private int score;
	private Food activeFood;
	private bool gameStarted;

	public GameObject gameoverCanvas;
	public List<GameObject> foods;
	public Transform[] spawnPoints;
	public Transform spawnParent;
	public int foodScale;
	public Text scoreText;
	public Text timerText;
	public float timeLimit;
	public Timer timer;
	public AudioClip backgroundMusic;
	public AudioClip correctSound;
	public AudioClip wrongSound;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}

	public static BrainbowGameManager GetInstance() {
		return instance;
	}

	void Start(){
		score = 0;

		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(this.timeLimit);
		}
		timerText.text = "Time: " + timer.TimeRemaining();
		SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
	}

	void Update() {
		if(gameStarted) {
			scoreText.text = "Score: " + score;

			if(score == 20 || timer.TimeRemaining() <= 0f) {
				GameOver();
			}

			timerText.text = "Time: " + (int)timer.TimeRemaining();
		}
	}

	public void StartGame() {
		gameStarted = true;
		timer.StartTimer();
		for(int i = 0; i < 4; ++i) {
			SpawnFood(spawnPoints[i]);
		}
	}

	public void Replace(GameObject toReplace) {
		++score;
		if(toReplace.GetComponent<Food>() != null && foods.Count > 0) {
			SpawnFood(toReplace.GetComponent<Food>().GetOrigin());
		}
	}

	void SpawnFood(Transform spawnPos) {
		int randomIndex = Random.Range (0, foods.Count);
		GameObject newFood = Instantiate(foods[randomIndex]);
		newFood.GetComponent<Food>().SetOrigin(spawnPos);
		newFood.GetComponent<Food>().Spawn(spawnPos, spawnParent, foodScale);
		SetActiveFood(newFood.GetComponent<Food>());
		foods.RemoveAt(randomIndex);
	}

	public void SetActiveFood(Food food) {
		activeFood = food;
	}

	void GameOver() {
		timer.StopTimer();
		activeFood.StopMoving();
		timerText.gameObject.SetActive(false);
		gameoverCanvas.gameObject.SetActive(true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text>();
		gameoverScore.text = "Good job! You fed your monster: " + score + " healthy foods!";
	}
}