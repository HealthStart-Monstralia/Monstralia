using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryMatchGameManager : MonoBehaviour {

	private static MemoryMatchGameManager instance;
	private bool gameStarted;
	private bool gameStartup;
	private int score;
	private GameObject currentFoodToMatch;
	private int difficultyLevel;
	private List<GameObject> activeFoods;
	private List<Food> matchedFoods;
	private bool stickerCanvasIsUp;
	private bool runningTutorial = false;

	public Canvas instructionPopup;
	public Transform foodToMatchSpawnPos;
	public Transform[] foodSpawnPos;
	public Transform[] foodParentPos;
	public float foodScale;
	public float foodToMatchScale;
	public List<GameObject> foods;
	public List<GameObject> dishes;
	public Timer timer;
	public float timeLimit;
	public Text timerText;
	public Text scoreText;
	public Canvas gameOverCanvas;
	public Canvas stickerPopupCanvas;
	public AudioClip correctSound;
	public GameObject subtitlePanel;
	public bool animIsPlaying = false;
	public AudioClip munchClip;
	public Transform target;
	public float speed;
	public AudioClip[] wrongMatchClips;
	
	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		difficultyLevel = GameManager.GetInstance ().GetLevel ("MemoryMatch");
	}

	void Start () {
		score = 0;

		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(timeLimit);
		}

		UpdateScoreText ();
		activeFoods = new List<GameObject> ();
		matchedFoods = new List<Food> ();

		if(GameManager.GetInstance().brainstormLagoonTutorial[(int)Constants.BrainstormLagoonLevels.MEMORY_MATCH]) {
			StartCoroutine(RunTutorial());
		}
		else {
			StartGame ();
		}

	}

	// Update is called once per frame
	void Update () {
		if(runningTutorial) {
			if(score == 1) {
				StartCoroutine(TutorialTearDown());
			}
		}


		if(gameStarted){
			if(score >= difficultyLevel*3 || timer.TimeRemaining () < 0) {
				if(!animIsPlaying)
					StartCoroutine(RunEndGameAnimation());
				// GameOver();
			}
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	public static MemoryMatchGameManager GetInstance() {
		return instance;
	}

	IEnumerator RunTutorial () {
		runningTutorial = true;
		instructionPopup.gameObject.SetActive(true);
		yield return new WaitForSeconds(5.0f);
		instructionPopup.gameObject.SetActive(false);
		subtitlePanel.GetComponent<SubtitlePanel>().Display("Now you try!", correctSound);

		StartGame ();
	}

	IEnumerator TutorialTearDown() {
		score = 0;
		subtitlePanel.GetComponent<SubtitlePanel>().Display("Great Job! Let's play MemoryMatch!", correctSound);
		yield return new WaitForSeconds(2.0f);
		ResetDishes();
		StartGame ();
	}

	public void StartGame() {
		gameStartup = true;

		SpawnDishes();

		SelectFoods();

		List<GameObject> copy = new List<GameObject>(activeFoods);

		ChooseFoodToMatch();

		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject newFood = SpawnFood(copy, true, foodSpawnPos[i], foodParentPos[i], foodScale);
			dishes[i].GetComponent<DishBehavior>().SetFood(newFood.GetComponent<Food>());
		}

		StartCoroutine(RevealDishes());
	}

	void SpawnDishes() {
		for(int i = 0; i < difficultyLevel*3; ++i) {
			dishes[i].SetActive(true);
		}
	}

	IEnumerator RevealDishes() {
		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject d = dishes[i];
			Animation animation = d.GetComponent<DishBehavior>().top.GetComponent<Animation>();
			d.GetComponent<DishBehavior>().top.GetComponent<Animation>().Play (animation["DishTopRevealLift"].name);
		}
		yield return new WaitForSeconds(3.0f);

		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject d = dishes[i];
			Animation animation = d.GetComponent<DishBehavior>().top.GetComponent<Animation>();
			d.GetComponent<DishBehavior>().top.GetComponent<Animation>().Play (animation["DishTopRevealClose"].name);
		} 

		gameStartup = false;

		if(!runningTutorial) {
			StartCoroutine(gameObject.GetComponent<Countdown>().RunCountdown());

			yield return new WaitForSeconds (4.0f);
			gameStarted = true;

			timer.StartTimer();
		}
	}

	void ResetDishes() {
		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject dish = dishes[i];
			dish.GetComponent<DishBehavior>().Reset ();
		}
	}

	void SelectFoods() {
		int foodCount = 0;
		while(foodCount < difficultyLevel*3){
			int randomIndex = Random.Range(0, foods.Count);
			GameObject newFood = foods[randomIndex];
			if(!activeFoods.Contains(newFood)){
				activeFoods.Add(newFood);
				++foodCount;
			}

		}
	}

	public void ChooseFoodToMatch() {
		if(!gameStartup) {
			++score;
			UpdateScoreText();
		}

		if(GameObject.Find ("ToMatchSpawnPos").transform.childCount > 0)
			Destroy(currentFoodToMatch);

		if(activeFoods.Count > 0) {
			currentFoodToMatch = SpawnFood(activeFoods, false, foodToMatchSpawnPos, foodToMatchSpawnPos, foodToMatchScale);
		}
	}

	GameObject SpawnFood(List<GameObject> foodsList, bool setAnchor, Transform spawnPos, Transform parent, float scale) {
		int randomIndex = Random.Range (0, foodsList.Count);
		GameObject newFood = Instantiate(foodsList[randomIndex]);
		newFood.name = foodsList[randomIndex].name;
		newFood.GetComponent<Food>().Spawn(spawnPos, parent, scale);
		if(setAnchor) {
			newFood.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
			newFood.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);

		}
		foodsList.RemoveAt(randomIndex);
		return newFood;
	}

	public Food GetFoodToMatch() {
		return currentFoodToMatch.GetComponent<Food>();
	}

	public void AddToMatchedList(Food food) {
		matchedFoods.Add (food);
	}

	IEnumerator RunEndGameAnimation(){
		animIsPlaying = true;
		timer.StopTimer();

		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject d = dishes[i];
			Animation animation = d.GetComponent<DishBehavior>().bottom.GetComponent<Animation>();
			Destroy(d.GetComponent<DishBehavior>().bottom.GetComponentInChildren<Food>().gameObject);
			SoundManager.GetInstance().PlaySFXClip(munchClip);
			d.GetComponent<DishBehavior>().bottom.GetComponent<Animation>().Play (animation["DishBottomShake"].name);
			yield return new WaitForSeconds (1.2f);
		}
		yield return new WaitForSeconds (1f);
		GameOver ();
	}
	

	void GameOver() {
		gameStarted = false;
		if(score >= difficultyLevel*3) {
			if(difficultyLevel == 1) {
				stickerPopupCanvas.gameObject.SetActive(true);
				GameManager.GetInstance().ActivateSticker("BrainstormLagoon", "Hippocampus");
			}
			GameManager.GetInstance().LevelUp("MemoryMatch");
		}

		// timer.StopTimer();
		timerText.gameObject.SetActive(false);
		scoreText.gameObject.SetActive(false);
		if(difficultyLevel > 1) {
			DisplayGameOverPopup();
		}
	}

	public void DisplayGameOverPopup () {
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		gameOverText.text = "Great job! You matched " + score + " healthy foods!";
	}

	public bool isGameStarted() {
		return gameStarted;
	}

	public bool isRunningTutorial() {
		return runningTutorial;
	}

	void UpdateScoreText() {
		scoreText.text = "Score: " + score;
	}
}
