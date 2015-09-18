using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : MonoBehaviour {

	private static BrainbowGameManager instance;
	private int score;
	private float nextUpdate;
	private bool timing;
	private Food activeFood;

	public GameObject gameoverCanvas;
	public List<GameObject> foods;
	public Transform[] spawnPoints;
	public Transform spawnParent;
	public Text scoreText;
	public Text timerText;
	public float timeRemaining;
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
		timeRemaining = 45;
		timerText.text = "Time: " + timeRemaining;
		SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
	}

	void Update() {
		scoreText.text = "Score: " + score;

		if(score == 20 || timeRemaining <= 0f) {
			GameOver();
		}

		if(timing) {
			timeRemaining -= Time.deltaTime;
			if(Time.time > nextUpdate) {
				nextUpdate = Time.time+1;
				timerText.text = "Time: " + (int)timeRemaining;
			}
		}
	}

	public void StartGame() {
		timing = true;
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
		newFood.GetComponent<Food>().Spawn(spawnPos, spawnParent);
		newFood.transform.SetParent (GameObject.Find ("FruitSpawnPanel").transform);
		newFood.transform.localPosition = spawnPos.localPosition;
		newFood.transform.localScale = new Vector3(25, 25, 0);
		newFood.GetComponent<SpriteRenderer>().sortingOrder = 1;
		foods.RemoveAt(randomIndex);
	}

	public void SetActiveFood(Food food) {
		activeFood = food;
	}

	void GameOver() {
		activeFood.StopMoving();
		timerText.gameObject.SetActive(false);
		gameoverCanvas.gameObject.SetActive(true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text>();
		gameoverScore.text = "Good job! You fed your monster: " + score + " healthy foods!";
	}
}