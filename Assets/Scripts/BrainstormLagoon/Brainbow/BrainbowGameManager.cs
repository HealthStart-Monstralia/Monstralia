using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : AbstractGameManager {

	private static BrainbowGameManager instance;
	private int score;
	private BrainbowFood activeFood;
	private bool gameStarted;
	private int difficultyLevel;
	private Dictionary<int, int> scoreGoals;
	private bool animIsPlaying = false;
	
	public Canvas gameoverCanvas;
	public Canvas stickerPopupCanvas;
	public List<GameObject> foods;
	public List<GameObject> inGameFoods = new List<GameObject>();
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
	public GameObject endGameAnimation;
	public GameObject subtitlePanel;
	
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
				// Animation.
				if(!animIsPlaying) {
					EndGameTearDown();
				}
			}
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	public void StartGame() {
		gameStarted = true;
		for(int i = 0; i < 4; ++i) {
			SpawnFood(spawnPoints[i]);
		}
		timer.StartTimer();
	}

	public void DisplayGo () {
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
		newFood.name = foods[randomIndex].name;
		newFood.GetComponent<BrainbowFood>().SetOrigin(spawnPos);
		newFood.GetComponent<BrainbowFood>().Spawn(spawnPos, spawnParent, foodScale);
		SetActiveFood(newFood.GetComponent<BrainbowFood>());
		inGameFoods.Add (newFood);
		foods.RemoveAt(randomIndex);
	}

	public void SetActiveFood(BrainbowFood food) {
		activeFood = food;
	}

	override public void GameOver() {
		if(score >= scoreGoals[difficultyLevel]) {
			if(difficultyLevel == 1) {
				stickerPopupCanvas.gameObject.SetActive(true);
				GameManager.GetInstance().ActivateSticker("BrainstormLagoon", "Brainbow");
			}
			GameManager.GetInstance().LevelUp("Brainbow");
		}

		if(difficultyLevel >= 1) {
			DisplayGameOverCanvas ();
		}
	}

	void EndGameTearDown ()
	{
		gameStarted = false;
		timer.StopTimer();

		if (activeFood != null) {
			activeFood.StopMoving ();
		}

		timerText.gameObject.SetActive (false);

		StartCoroutine(RunEndGameAnimation());
	}

	IEnumerator RunEndGameAnimation() {
		animIsPlaying = true;

		foreach (GameObject food in inGameFoods) {
			food.GetComponent<Collider2D>().enabled = true;
		}

		GameObject animation = (GameObject)Instantiate(endGameAnimation);
		animation.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameManager.instance.getMonster());
		yield return new WaitForSeconds (endGameAnimation.gameObject.GetComponent<Animator> ().runtimeAnimatorController.animationClips [0].length);
		GameOver ();
	}

	public void DisplayGameOverCanvas () {
		gameoverCanvas.gameObject.SetActive (true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text> ();
		gameoverScore.text = "Good job! You fed your monster: " + score + " healthy foods!";
	}
}