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
	private bool runningTutorial = false;
	private Text nowYouTryText;
	private BrainbowFood banana;
	private Transform bananaOrigin;
	private bool gameOver = false;
	private bool isInputAllowed = false;
	private Coroutine tutorialCoroutine;

	private float waterSpawnTime = 0.0f;
	private bool spawnWater = false;

	public Text gameTitle;
	public Canvas mainCanvas;
	public Canvas reviewCanvas;
	public Canvas instructionPopup;
	public Canvas gameoverCanvas;
	public Canvas stickerPopupCanvas;
	public List<GameObject> foods;
	public List<GameObject> inGameFoods = new List<GameObject>();
	public Transform[] spawnPoints;
	public Transform spawnParent;
	public int foodScale;
	public Slider scoreGauge;
	public Text timerText;
	public float timeLimit;
	public Timer timer;

	public GameObject endGameAnimation;
	public GameObject subtitlePanel;
	public Transform tutorialOrigin;

	public GameObject waterBottle;
	public GameObject[] waterSpawnLocations;
	public List<GameObject> waterBottleList = new List<GameObject> ();

	[HideInInspector] public GameManager.MonsterType typeOfMonster;
	[HideInInspector] public BBMonster monsterObject;
	public GameObject[] monsterList;
	public GameObject spawnPoint;

	public AudioClip[] correctMatchClips;
	public AudioClip[] wrongMatchClips;
	public AudioClip munchSound;
	public AudioClip backgroundMusic;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	public AudioClip intro;
	public AudioClip instructions;
	public AudioClip nowYouTry;
	public AudioClip letsPlay;
	public AudioClip waterTip;
	public AudioClip level1Complete;
	public AudioClip stickerbook;
	public AudioClip reviewGame;
	public AudioClip finalFeedback;
	
	void Awake() {
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
			difficultyLevel = GameManager.GetInstance ().GetLevel ("Brainbow");

			scoreGoals = new Dictionary<int, int> () {
				{ 1, 8 },
				{ 2, 12 },
				{ 3, 20 },
				{ 4, 20 },
				{ 5, 20 }
			};
		}
	}

	public static BrainbowGameManager GetInstance() {
		return instance;
	}

	void Start(){
		SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
		//create enums for each part of the island that represents the games to avoid using numbers to access the arrays
		//in GameManager. Ex: brainstormLagoonTutorial[BrainstormLagoon.BRAINBOW]


		if (GameManager.GetInstance ().LagoonReview) {
			StartReview ();
		}
		else {
			PregameSetup ();
		}
	}

	void Update() {
		if(runningTutorial && score == 1) {
			StartCoroutine(TutorialTearDown());
		}

		if(gameStarted) {
			if(score == 20 || timer.TimeRemaining() <= 0.0f) {
				// Animation.
				if(!animIsPlaying) {
					EndGameTearDown();
				}
			}

			if(waterSpawnTime > 0f && spawnWater) {
				if (waterBottleList.Count < 1) {
					waterSpawnTime -= Time.deltaTime;
				}
			}

			// Create water bottle when waterSpawnTime is 0
			else if (waterSpawnTime <= 0f && spawnWater) {
				if (spawnWater && waterBottleList.Count < 1) {
					waterSpawnTime = Random.Range (10f, 15f);
					CreateWater ();
				}
			}
		}
	}

	void FixedUpdate() {
		if(gameStarted) {
			timerText.text = "Time: " + timer.TimeRemaining();
		}
	}

	void StartReview() {
		SoundManager.GetInstance().PlayVoiceOverClip(reviewGame);
		reviewCanvas.gameObject.SetActive(true);
//		reviewCanvas = (Canvas)Instantiate (GameManager.GetInstance().ChooseLagoonReviewGame());
//		reviewCanvas.transform.SetParent (GameObject.Find ("Canvas").transform);
//		reviewCanvas.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

	public void PregameSetup () {
		score = 0;
		
		scoreGauge.maxValue = scoreGoals[difficultyLevel];

		typeOfMonster = GameManager.GetMonsterType ();
		CreateMonster ();
		monsterObject.PlaySpawn ();

		UpdateScoreGauge();

		if (GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.BRAINBOW]) {
			tutorialCoroutine = StartCoroutine (RunTutorial ());
		}
		else {
			StartGame ();
		}
	}

	IEnumerator RunTutorial() {
		runningTutorial = true;
		isInputAllowed = false;
		instructionPopup.gameObject.SetActive(true);
		SoundManager.GetInstance().PlayVoiceOverClip(intro);
		yield return new WaitForSeconds(6f);
		SoundManager.GetInstance().PlayVoiceOverClip(instructions);
		yield return new WaitForSeconds(3.5f);	// Replace with generic time of intro audio (after splitting track)
		GameObject redOutline = instructionPopup.gameObject.transform.Find ("RedFlashingOutline").gameObject;
		redOutline.SetActive(true);

		yield return new WaitForSeconds(instructions.length-4.5f);

		Animation anim = instructionPopup.gameObject.transform.Find ("TutorialAnimation").gameObject.GetComponent<Animation> ();
		anim.Play ("DragToStripe");

		yield return new WaitForSeconds(anim.clip.length);
		anim.gameObject.SetActive (false);

		GameObject banana = instructionPopup.transform.Find ("Banana").gameObject;
		banana.SetActive(true);
//		banana.GetComponent<SpriteRenderer> ().enabled = true;
//		banana.GetComponent<PolygonCollider2D> ().enabled = true;

		//redOutline.SetActive(false);
		if (runningTutorial) {
			subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Now You Try!", nowYouTry);
			isInputAllowed = true;
			bananaOrigin = tutorialOrigin;
			banana.GetComponent<BrainbowFood> ().SetOrigin (bananaOrigin);
		}
	}

	IEnumerator TutorialTearDown ()	{
		StopCoroutine (tutorialCoroutine);
		isInputAllowed = false;
		score = 0;
		UpdateScoreGauge();
		runningTutorial = false;
		subtitlePanel.GetComponent<SubtitlePanel>().Display("Perfect!", letsPlay, true);
		yield return new WaitForSeconds(letsPlay.length);
		subtitlePanel.GetComponent<SubtitlePanel>().Hide ();
		instructionPopup.gameObject.SetActive(false);
		gameTitle.gameObject.SetActive(false);
		StartGame ();
	}

	public bool IsRunningTutorial() {
		return runningTutorial;
	}

	public void SkipTutorialButton(GameObject button) {
		SkipTutorial ();
		Destroy (button);
	}

	public void SkipTutorial() {
		StopCoroutine (tutorialCoroutine);
		StartCoroutine (TutorialTearDown ());
	}

	public void StartGame() {
		scoreGauge.gameObject.SetActive(true);
		timerText.gameObject.SetActive(true);
		timer.SetTimeLimit (timeLimit);
		timerText.text = "Time: " + timer.TimeRemaining();
		waterSpawnTime = Random.Range(5, 10);

		StartCoroutine (DisplayGo());
	}

	public void CreateWater() {
		print ("Create Water");
		int selection = Random.Range (0, 4);
		GameObject water = Instantiate(
			waterBottle,
			mainCanvas.transform
		);
		water.transform.position = waterSpawnLocations [selection].transform.position;
	}

	public IEnumerator DisplayGo () {
		GameManager.GetInstance ().Countdown ();
		yield return new WaitForSeconds (5.0f);
		PostCountdownSetup ();
	}

	void PostCountdownSetup ()
	{
		isInputAllowed = true;
		if(difficultyLevel == 1){
			SoundManager.GetInstance().PlayVoiceOverClip(waterTip);

		}
		gameStarted = true;
		for (int i = 0; i < 4; ++i) {
			SpawnFood (spawnPoints [i]);
		}
		timer.StartTimer ();
		spawnWater = true;
	}

	public void Replace(GameObject toReplace) {
		++score;
		if(!runningTutorial) {
			UpdateScoreGauge();
			if(toReplace.GetComponent<Food>() != null && foods.Count > 0) {
				SpawnFood(toReplace.GetComponent<BrainbowFood>().GetOrigin());
			}
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
		spawnWater = false;
		if(score >= scoreGoals[difficultyLevel]) {
			GameManager.GetInstance().AddLagoonReviewGame("BrainbowReviewGame");
			if(difficultyLevel == 1) {
				SoundManager.GetInstance().PlayVoiceOverClip(stickerbook);
				stickerPopupCanvas.gameObject.SetActive(true);
				GameManager.GetInstance ().ActivateBrainstormLagoonReview();

				if(GameManager.GetInstance().LagoonFirstSticker) {
					stickerPopupCanvas.transform.Find("BackButton").gameObject.SetActive(false);
					stickerPopupCanvas.transform.Find("StickerbookButton").gameObject.SetActive(true);
					GameManager.GetInstance().LagoonFirstSticker = false;
				}
				else {
					stickerPopupCanvas.transform.Find("StickerbookButton").gameObject.SetActive(false);
					stickerPopupCanvas.transform.Find("BackButton").gameObject.SetActive(true);
				}

				GameManager.GetInstance().ActivateSticker("BrainstormLagoon", "Brainbow");
				GameManager.GetInstance ().LagoonTutorial[(int)Constants.BrainstormLagoonLevels.BRAINBOW] = false;
			}
			GameManager.GetInstance().LevelUp("Brainbow");
		}

		if(difficultyLevel > 1 || score < scoreGoals[difficultyLevel]) {
			DisplayGameOverCanvas ();
		}
	}

	void EndGameTearDown () {
		subtitlePanel.GetComponent<SubtitlePanel>().Hide ();
		gameStarted = false;
		timer.StopTimer();
		gameOver = true;

		if(activeFood != null) {
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

		SoundManager.GetInstance().PlayVoiceOverClip(finalFeedback);
		/*
		GameObject animation = (GameObject)Instantiate(endGameAnimation);
		animation.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameManager.GetInstance().getMonster());
		*/
		monsterObject.PlayEat ();

		//Animator monsterAnim = monsterObject.gameObject.GetComponent<Animator> ();

		yield return new WaitForSeconds (14f);
		//yield return new WaitForSeconds (endGameAnimation.gameObject.GetComponent<Animator> ().runtimeAnimatorController.animationClips [0].length);

		GameOver ();
	}

	public void DisplayGameOverCanvas () {
		gameoverCanvas.gameObject.SetActive (true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text> ();
		gameoverScore.text = "Good job! You fed your monster " + score + " healthy brain foods!";
	}

	void UpdateScoreGauge() {
		scoreGauge.value = score;
	}

	public bool GameStarted() {
		return gameStarted;
	}

	public bool isGameOver() {
		return gameOver;
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
			spawnPoint.transform.position,
			Quaternion.identity) as GameObject;
		monsterObject = monsterSpawn.GetComponent<BBMonster> ();
	}

	public bool GetIsInputAllowed () {
		return isInputAllowed;
	}

}