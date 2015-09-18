using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryMatchGameManager : MonoBehaviour {

	private static MemoryMatchGameManager instance;
	private bool gameStart;
	private int score;
	private GameObject currentFoodToMatch;

	public Transform foodToMatchSpawnPos;
	public Transform[] foodSpawnPos;
	public Transform[] foodParentPos;
	public float foodScale;
	public float foodToMatchScale;
	public List<GameObject> foods;
	public Timer timer;
	public float timeLimit;
	public Text timerText;
	public Text scoreText;

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(timeLimit);
		}
	}

	// Update is called once per frame
	void Update () {
		if(gameStart){
			scoreText.text = "Score: " + score;
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	public static MemoryMatchGameManager GetInstance() {
		return instance;
	}

	public void StartGame() {
		gameStart = true;
		timer.StartTimer();

		currentFoodToMatch = SpawnFood(false, foodToMatchSpawnPos, foodToMatchSpawnPos, foodToMatchScale);
		print("food to match: " + currentFoodToMatch.name);
		for(int i = 0; i < foodSpawnPos.Length; ++i) {
			SpawnFood(true, foodSpawnPos[i], foodParentPos[i], foodScale);
		}
	}

	GameObject SpawnFood(bool setAnchor, Transform spawnPos, Transform parent, float scale) {
		int randomIndex = Random.Range (0, foods.Count);
		GameObject newFood = Instantiate(foods[randomIndex]);
		newFood.GetComponent<Food>().SetOrigin(spawnPos);
		newFood.GetComponent<Food>().Spawn(spawnPos, parent, scale);
		if(setAnchor) {
			newFood.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
			newFood.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
		}
		return newFood;

	}
}
