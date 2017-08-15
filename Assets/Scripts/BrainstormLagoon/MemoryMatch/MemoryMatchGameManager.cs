using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryMatchGameManager : AbstractGameManager {
	private static MemoryMatchGameManager instance;
	private bool gameStarted;
	private bool gameStartup;
	private int score;
	private GameObject currentFoodToMatch;
	private int numDishes;
	private int difficultyLevel;
	private List<GameObject> activeFoods;
	private List<Food> matchedFoods;
	private bool stickerCanvasIsUp;
	private bool runningTutorial = false;
	private bool rotate = false;
	private float stopRotateTime;
	private Coroutine tutorialCoroutine;

	public Canvas reviewCanvas;
	public Canvas instructionPopup;
	public Canvas gameOverCanvas;

	public Canvas mainCanvas;

	public Timer timer;
	public float[] timeLimit;
	public Text timerText;

	public Transform foodToMatchSpawnPos;
	public float foodScale;
	public float foodToMatchScale;
	public List<GameObject> foods;
	public GameObject dish;
	public List<GameObject> dishes;

	public Slider scoreGauge;
	public SubtitlePanel subtitlePanel;
	[HideInInspector] public bool animIsPlaying = false;
	[HideInInspector] public bool inputAllowed = false;

	public Transform rotationTarget;
	public float rotationalSpeed;

	public GameManager.MonsterType typeOfMonster;
	[HideInInspector] public MMMonster monsterObject;
	public GameObject[] monsterList;

	public DishObject[] tutorialDishes;
	public AudioClip[] wrongMatchClips;
	public AudioClip munchClip;

	public AudioClip instructions;
	public AudioClip letsPlay;
	public AudioClip nowYouTry;
	public bool hasSpawned;

	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		if (!GameManager.GetInstance ()) {
			SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
			switchScene.loadScene ("Start");
		} else {

			difficultyLevel = GameManager.GetInstance ().GetLevel ("MemoryMatch");
			typeOfMonster = GameManager.GetMonsterType ();
			CreateMonster ();
			monsterObject.PlaySpawn ();
			RetrieveFoodsFromManager ();

			if(GameManager.GetInstance().LagoonReview) {
				//StartReview();
				PregameSetup ();
			}
			else {
				PregameSetup ();
			}
		}
	}

	public void PregameSetup () {
		activeFoods = new List<GameObject> ();
		matchedFoods = new List<Food> ();
		inputAllowed = false;

		if (GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.MEMORY_MATCH]) {
			tutorialCoroutine = StartCoroutine (RunTutorial ());
		}
		else {
			score = 0;

			switch (difficultyLevel) {
			case (1):
				numDishes = 3;
				timer.SetTimeLimit (timeLimit[difficultyLevel - 1]);
				break;
			case (2):
				numDishes = 4;
				timer.SetTimeLimit (timeLimit[difficultyLevel - 1]);
				break;
			case (3):
				numDishes = 6;
				timer.SetTimeLimit (timeLimit[difficultyLevel - 1]);
				break;
			default:
				numDishes = 6;
				timer.SetTimeLimit (60f);
				break;
			}

			scoreGauge.maxValue = numDishes;

			UpdateScoreGauge ();
			StartGame ();
		}
	}

	public void StartReview() {
		Debug.Log ("starting memory match review");
        //		reviewCanvas = GameManager.GetInstance().ChooseLagoonReviewGame();
        //		reviewCanvas.gameObject.SetActive(true);
        //ReviewManager.GetInstance().needsReview = true;
        //ReviewManager.GetInstance().levelToReview = "MemoryMatchReviewGame";

    }

	void Update () {
		if(runningTutorial) {
			if(score == 1) {
				StartCoroutine(TutorialTearDown());
			}
		}

		if(gameStarted){
			if(score >= numDishes || timer.TimeRemaining () <= 0) {
				if(!animIsPlaying) {
					StartCoroutine(RunEndGameAnimation());
				// GameOver();
				}
			}
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}

		if(hasSpawned && rotate && difficultyLevel > 1) {
			RotateDishes();
			if(Time.time > stopRotateTime) {
				rotate = false;
			}
		}
	}

	public static MemoryMatchGameManager GetInstance() {
		return instance;
	}

	IEnumerator RunTutorial () {
		print ("RunTutorial");
		runningTutorial = true;
		instructionPopup.gameObject.SetActive(true);

		DishObject tutDish1 = tutorialDishes[0];
		DishObject tutDish2 = tutorialDishes[1];
		DishObject tutDish3 = tutorialDishes[2];

		GameObject tutFood1 = tutorialDishes[0].transform.Find ("Banana").gameObject;
		GameObject tutFood2 = tutorialDishes[1].transform.Find ("Raspberry").gameObject;
		GameObject tutFood3 = tutorialDishes[2].transform.Find ("Brocolli").gameObject;
		currentFoodToMatch = tutFood1;

		tutorialDishes [0].SetFood (tutFood1);
		tutorialDishes [1].SetFood (tutFood2);
		tutorialDishes [2].SetFood (tutFood3);

		tutFood1.transform.localPosition = new Vector3 (0, 1.25f, 0);
		tutFood2.transform.localPosition = new Vector3 (0, 1.25f, 0);
		tutFood3.transform.localPosition = new Vector3 (0, 1.25f, 0);

		tutFood1.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		tutFood2.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		tutFood3.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);

		yield return new WaitForSeconds (1f);

		SoundManager.GetInstance ().PlayVoiceOverClip (instructions);

		yield return new WaitForSeconds (1f);

		tutDish1.SpawnLids(true);
		yield return new WaitForSeconds(0.25f);
		tutDish2.SpawnLids(true);
		yield return new WaitForSeconds(0.25f);
		tutDish3.SpawnLids(true);

		yield return new WaitForSeconds(1.15f);

		// Dish lift.
		foreach (DishObject dish in tutorialDishes) {
			dish.OpenLid ();
		}
		yield return new WaitForSeconds(3.5f);

		// Dish close.
		foreach (DishObject dish in tutorialDishes) {
			dish.CloseLid ();
		}

		yield return new WaitForSeconds(2f);
		instructionPopup.gameObject.transform.Find ("panelbanana").gameObject.SetActive(true);

		yield return new WaitForSeconds(3f);
		Animator handAnim = instructionPopup.gameObject.transform.Find ("TutorialAnimation").gameObject.transform.Find ("Hand").gameObject.GetComponent<Animator>();
		handAnim.Play("mmhand_5_12");
		yield return new WaitForSeconds(4f);
		SoundManager.GetInstance ().PlayCorrectSFX ();
		tutorialDishes[0].OpenLid();
		yield return new WaitForSeconds(2f);
		tutorialDishes [0].CloseLid ();

		yield return new WaitForSeconds (instructions.length - 14f);
		subtitlePanel.Display ("Now you try!", nowYouTry);
		inputAllowed = true;
		handAnim.gameObject.SetActive (false);

		for (int i = 0; i < tutorialDishes.Length; ++i) {
			tutorialDishes [i].GetComponent<Collider2D> ().enabled = true;
		}
//		StartGame ();
	}

	public void SkipTutorialButton(GameObject button) {
		SkipTutorial ();
		Destroy (button);
	}

	public void SkipTutorial() {
		StopCoroutine (tutorialCoroutine);
		StartCoroutine (TutorialTearDown ());
	}

	IEnumerator TutorialTearDown() {
		print ("TutorialTearDown");
		runningTutorial = false;
		score = 0;
		subtitlePanel.Display("Perfect!", letsPlay);
		yield return new WaitForSeconds(2.0f);
		subtitlePanel.Hide ();
		instructionPopup.gameObject.SetActive (false);
		GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.MEMORY_MATCH] = false;
		PregameSetup ();
	}

	public void StartGame() {
		scoreGauge.gameObject.SetActive (true);
		timerText.gameObject.SetActive (true);

		//timer.SetTimeLimit (timeLimit);
		timerText.text = "Time: " + timer.TimeRemaining();
		UpdateScoreGauge ();

		gameStartup = true;

		SpawnDishes();
		hasSpawned = true;
		SelectFoods();

		List<GameObject> copy = new List<GameObject>(activeFoods);

		ChooseFoodToMatch();

		for(int i = 0; i < numDishes; ++i) {
			DishObject dishComponent = dishes[i].GetComponent<DishObject>();
			GameObject newFood = SpawnFood(copy, true, dishComponent.lid.transform, dishComponent.dish.transform, foodScale);
			dishComponent.SetFood(newFood);
		}

		StartCoroutine(RevealDishes());
	}

	void SpawnDishes() {
		//Mathf.Cos/Sin use radians, so the dishes are positioned 2pi/numDishes apart
		float dishPositionAngleDelta = (2*Mathf.PI)/numDishes;
        bool reduceSize = false;
        if (numDishes > 4)
            reduceSize = true;

		for(int i = 0; i < numDishes; ++i) {
			GameObject newDish = Instantiate(dish);
			float offset = 200f;
			newDish.transform.SetParent (mainCanvas.transform);
			newDish.transform.localPosition = new Vector3(
				offset * Mathf.Cos (dishPositionAngleDelta*i + Mathf.PI / 2), 
				offset * Mathf.Sin (dishPositionAngleDelta*i + Mathf.PI / 2), 
				0);
			dishes[i] = newDish;

            // Reduce size of dish depending on number of dishes
            if (reduceSize) {
                newDish.transform.localScale = newDish.transform.localScale * 0.8f;
            }
		}
	}

	IEnumerator RevealDishes() {
		rotate = true;
		float rotateTimeDelta = Random.Range (3,6);
		stopRotateTime = Time.time + rotateTimeDelta;

		/*
		for(int i = 0; i < numDishes; ++i) {
			dishes[i].GetComponent<DishObject>().OpenLid();
		}
		*/
		yield return new WaitForSeconds(rotateTimeDelta);

		for (int i = 0; i < numDishes; ++i) {
			dishes [i].GetComponent<DishObject> ().SpawnLids (true);
			yield return new WaitForSeconds(0.25f);
		}

		gameStartup = false;

		if(!runningTutorial) {
			GameManager.GetInstance ().Countdown ();

			yield return new WaitForSeconds (4.0f);
			gameStarted = true;
			inputAllowed = true;
			timer.StartTimer();
		}
	}

	void ResetDishes() {
		for(int i = 0; i < difficultyLevel*3; ++i) {
			GameObject dish = dishes[i];
			dish.GetComponent<DishObject>().Reset ();
		}
	}

	public void RotateDishes() {
		Vector3 zAxis = Vector3.forward; //<0, 0, 1>;

		for(int i = 0; i < numDishes; ++i) {
			GameObject d = dishes[i];
			Quaternion startRotation = d.transform.rotation;

			d.transform.RotateAround(rotationTarget.position, zAxis, rotationalSpeed);
			d.transform.rotation = startRotation;
		}
	}

	void RetrieveFoodsFromManager() {
		foods = GameManager.GetInstance ().GetComponent<FoodList> ().goodFoods;
	}

	void SelectFoods() {
		int foodCount = 0;
		while(foodCount < numDishes){
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
			UpdateScoreGauge();
		}

		if(GameObject.Find ("ToMatchSpawnPos").transform.childCount > 0)
			Destroy(currentFoodToMatch);

		if(activeFoods.Count > 0) {
			currentFoodToMatch = SpawnFood(activeFoods, false, foodToMatchSpawnPos, foodToMatchSpawnPos, foodToMatchScale);
			currentFoodToMatch.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
			currentFoodToMatch.GetComponent<SpriteRenderer> ().sortingOrder = 5;
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
		newFood.GetComponent<Collider2D> ().enabled = false;
		//newFood.GetComponent<SpriteRenderer> ().sortingOrder = 4;
		return newFood;
	}

	public Food GetFoodToMatch() {
		return currentFoodToMatch.GetComponent<Food>();
	}

	public void AddToMatchedList(Food food) {
		print ("AddToMatchedList: " + food);
		matchedFoods.Add (food);
		print ("matchedFoods: " + matchedFoods);
		matchedFoods.Add (food);
	}

	IEnumerator RunEndGameAnimation(){
		animIsPlaying = true;
		timer.StopTimer();

		for(int i = 0; i < numDishes; ++i) {
			if(dishes[i].GetComponent<DishObject>().IsMatched ()) {
				Destroy(dishes[i].GetComponent<DishObject>().foodObject.gameObject);
				SoundManager.GetInstance().PlaySFXClip(munchClip);
				dishes[i].GetComponent<DishObject>().Shake(true);
				monsterObject.PlayEat ();
				yield return new WaitForSeconds (1.2f);
			}
		}
		monsterObject.PlayDance ();
		yield return new WaitForSeconds (1f);
		GameOver ();
	}

	public override void GameOver() {
		gameStarted = false;
		timerText.gameObject.SetActive(false);
		scoreGauge.gameObject.SetActive(false);
		if (currentFoodToMatch)
			currentFoodToMatch.SetActive (false);

		if (score >= numDishes) {
			GameManager.GetInstance ().AddLagoonReviewGame ("MemoryMatchReviewGame");

			if (difficultyLevel == 1) {
				UnlockSticker (StickerManager.StickerType.Hippocampus);	// Calling from AbstractGameManager
			} else {
				DisplayGameOverPopup ();
			}

			GameManager.GetInstance ().LevelUp ("MemoryMatch");
		} else {
			if(difficultyLevel >= 1) {
				//GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.MEMORY_MATCH] = false;
				DisplayGameOverPopup();
			}
		}
	}

	public void DisplayGameOverPopup () {
		Debug.Log ("In DisplayGameOverPopup");
		gameOverCanvas.gameObject.SetActive(true);
		Text gameOverText = gameOverCanvas.GetComponentInChildren<Text> ();
		if (score > 1) {
			gameOverText.text = "Great job! You matched " + score + " healthy foods!";
		} else if (score == 1) {
			gameOverText.text = "Great job! You matched " + score + " healthy food!";
		} else {
			gameOverText.text = "You matched " + score + " healthy foods! Let's try again!";
		}
	}

	public bool isGameStarted() {
		return gameStarted;
	}

	public bool isRunningTutorial() {
		return runningTutorial;
	}

	void UpdateScoreGauge() {
		scoreGauge.value = score;
	}

	public void SubtractTime(float delta) {
		timer.SubtractTime(delta);
	}

	void CreateMonster() {
		// Blue = 0, Green = 1, Red = 2, Yellow = 3

		switch (typeOfMonster) {
		case GameManager.MonsterType.Blue:
			InstantiateMonster (monsterList [(int)GameManager.MonsterType.Blue]);
			break;
		case GameManager.MonsterType.Green:
			InstantiateMonster (monsterList [(int)GameManager.MonsterType.Green]);
			break;
		case GameManager.MonsterType.Red:
			InstantiateMonster (monsterList [(int)GameManager.MonsterType.Red]);
			break;
		case GameManager.MonsterType.Yellow:
			InstantiateMonster (monsterList [(int)GameManager.MonsterType.Yellow]);
			break;
		}
	}

	void InstantiateMonster(GameObject monster) {
		GameObject monsterSpawn = Instantiate (
			monster, 
			Vector3.zero,
			Quaternion.identity) as GameObject;
		monsterObject = monsterSpawn.GetComponent<MMMonster> ();
	}
}
