using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : MonoBehaviour {

	private static BrainbowGameManager instance;
	private int score;
	private BrainbowFood activeFood;
	private bool gameStarted;
	private int difficultyLevel;
	private Dictionary<int, int> scoreGoals;
	
	public Canvas gameoverCanvas;
	public Canvas stickerPopupCanvas;
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
	public AudioClip incorrectSound;
	
	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		difficultyLevel = GameManager.GetInstance().GetLevel("Brainbow");

		scoreGoals = new Dictionary<int, int>()
		{
			{1, 8},
			{2, 12},
			{3, 20}
		};
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
		scoreText.text = "Score: " + score;
		timerText.text = "Time: " + timer.TimeRemaining();
		SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
	}

	void Update() {
		if(gameStarted) {
			if(score == 20 || timer.TimeRemaining() <= 0.0f) {
				GameOver();
			}
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	void StartGame() {
		DisplayGo ();	
		gameStarted = true;
		for(int i = 0; i < 4; ++i) {
			SpawnFood(spawnPoints[i]);
		}
		timer.StartTimer();
	}

	void DisplayGo ()
	{
		StartCoroutine(gameObject.GetComponent<Countdown>().RunCountdown());
	}

	public void Replace(GameObject toReplace) {
		++score;
		scoreText.text = "Score: " + score;
		if(toReplace.GetComponent<Food>() != null && foods.Count > 0) {
			SpawnFood(toReplace.GetComponent<BrainbowFood>().GetOrigin());
		}
	}

	void SpawnFood(Transform spawnPos) {
		int randomIndex = Random.Range (0, foods.Count);
		GameObject newFood = Instantiate(foods[randomIndex]);
		newFood.GetComponent<BrainbowFood>().SetOrigin(spawnPos);
		newFood.GetComponent<BrainbowFood>().Spawn(spawnPos, spawnParent, foodScale);
		SetActiveFood(newFood.GetComponent<BrainbowFood>());
		foods.RemoveAt(randomIndex);
	}

	public void SetActiveFood(BrainbowFood food) {
		activeFood = food;
	}

	void GameOver() {
		gameStarted = false;
		if(score >= scoreGoals[difficultyLevel]) {
			if(difficultyLevel == 1) {
				stickerPopupCanvas.gameObject.SetActive(true);
				GameManager.GetInstance().ActivateSticker("BrainstormLagoon", "Brainbow");
			}
			GameManager.GetInstance().LevelUp("Brainbow");
		}

		timer.StopTimer();
		activeFood.StopMoving();
		timerText.gameObject.SetActive(false);

		if(difficultyLevel > 1) {
			DisplayGameOverCanvas ();
		}
	}

	public void DisplayGameOverCanvas () {
		gameoverCanvas.gameObject.SetActive (true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text> ();
		gameoverScore.text = "Good job! You fed your monster: " + score + " healthy foods!";
	}
}