using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGame : MonoBehaviour {

	private static BrainbowGame instance;
	private int score;
	private float startTime;
	private float nextUpdate;
	private bool timing;

	public GameObject gameoverCanvas;
	public List<GameObject> foods;
	public Transform[] spawnPoints;
	public Text scoreText;
	public Text timerText;
	public float timeRemaining;
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

	public static BrainbowGame GetInstance() {
		return instance;
	}

	void Start(){
		score = 0;
		startTime = Time.time;
		timeRemaining = 45;
		timerText.text = "Time: " + timeRemaining;
		for(int i = 0; i < 4; ++i) {
			SpawnFood(spawnPoints[i]);
		}
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
		newFood.transform.SetParent (GameObject.Find ("FruitSpawnPanel").transform);
		newFood.transform.localPosition = spawnPos.localPosition;
		newFood.transform.localScale = new Vector3(25, 25, 0);
		newFood.GetComponent<SpriteRenderer>().sortingOrder = 1;
		foods.RemoveAt(randomIndex);
	}

	void GameOver() {
		timerText.gameObject.SetActive(false);
		gameoverCanvas.gameObject.SetActive(true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text>();
		gameoverScore.text = "Good job! You fed your monster: " + score + " healthy foods!";
	}
}