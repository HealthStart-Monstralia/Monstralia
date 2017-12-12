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
	private Coroutine tutorialCoroutine;

	private float waterSpawnTime = 0.0f;
	private bool spawnWater = false;

    [Header ("Brainbow")]
    public VoiceOversData voData;
	public Canvas mainCanvas;
	public Canvas instructionPopup;
    public GameObject waterNotification;
	public GameObject tutorialHand;
	public GameObject tutorialBanana;
	public GameObject tutorialPlayerBanana;
	public GameObject tutorialSpot;
	public Canvas gameoverCanvas;
	public List<GameObject> foods;
	public List<GameObject> inGameFoods = new List<GameObject>();
    public List<GameObject> restrictedFoods;
	public Transform[] spawnPoints;
	public Transform spawnParent;
	public int foodScale;
	public LayerMask foodLayerMask;
	public ScoreGauge scoreGauge;
	public float timeLimit;
	public Timer timer;
    [HideInInspector] public bool inputAllowed = false;

    public GameObject endGameAnimation;
	public SubtitlePanel subtitlePanel;
	public Transform tutorialOrigin;

	public GameObject waterBottle;
	public GameObject[] waterSpawnLocations;
	public List<GameObject> waterBottleList = new List<GameObject> ();

	[HideInInspector] public BBMonster monsterObject;
	public GameObject[] monsterList;
	public GameObject spawnPoint;

	public AudioClip[] correctMatchClips;
	public AudioClip[] wrongMatchClips;
	public AudioClip munchSound;
	public AudioClip backgroundMusic;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	public AudioClip level1Complete;
	
	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
        CheckForGameManager ();

		difficultyLevel = GameManager.GetInstance ().GetLevel (DataType.Minigame.Brainbow);
		scoreGoals = new Dictionary<int, int> () {
			{ 1, 8 },
			{ 2, 12 },
			{ 3, 20 }
		};

        SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
        instructionPopup.gameObject.SetActive (false);
        tutorialHand.SetActive (false);
        waterNotification.SetActive (false);
        timer.gameObject.SetActive (false);
        ChooseFoodsFromManager ();
    }

	public static BrainbowGameManager GetInstance() {
		return instance;
	}

	void Update() {
		if(runningTutorial && score == 1) {
			StartCoroutine(TutorialTearDown());
		}

		if (gameStarted) {
			if (score == 20) {
				// Animation
				EndGameTearDown();
			}

			if (waterSpawnTime > 0f && spawnWater) {
				if (waterBottleList.Count < 1) {
					waterSpawnTime -= Time.deltaTime;
				}
			}

			// Create water bottle when waterSpawnTime is 0
			else if (waterSpawnTime <= 0f && spawnWater && waterBottleList.Count < 1) {
				waterSpawnTime = Random.Range (10f, 15f);
				CreateWater ();
			}
		}
	}

    private void OnEnable () {
        // Subscribe to events
        Timer.OutOfTime += OnOutOfTime;
    }

    private void OnDisable () {
        // Unsubscribe to events
        Timer.OutOfTime -= OnOutOfTime;
    }

    // Event
    void OnOutOfTime () { EndGameTearDown (); }

    public override void PregameSetup () {
		score = 0;
		UpdateScoreGauge();

		CreateMonster ();
		monsterObject.PlaySpawn ();

		if (GameManager.GetInstance ().GetPendingTutorial(DataType.Minigame.Brainbow)) {
			tutorialCoroutine = StartCoroutine (RunTutorial ());
		}
		else {
			StartGame ();
		}
	}

	IEnumerator RunTutorial() {
		runningTutorial = true;
        inputAllowed = false;

        instructionPopup.gameObject.SetActive(true);
        yield return new WaitForSeconds (1.5f);
        AudioClip tutorial1 = voData.FindVO("1_tutorial_start");
        SoundManager.GetInstance ().PlayVoiceOverClip (tutorial1);
        yield return new WaitForSeconds(tutorial1.length);

        AudioClip tutorial2 = voData.FindVO ("2_tutorial_goodfood");
        SoundManager.GetInstance ().PlayVoiceOverClip (tutorial2);
        yield return new WaitForSeconds (tutorial2.length - 0.5f);
        GameObject redOutline = instructionPopup.gameObject.transform.Find ("RedFlashingOutline").gameObject;
		redOutline.SetActive(true);
        yield return new WaitForSeconds (1f);

        AudioClip tutorial3 = voData.FindVO ("3_tutorial_likethis");
        SoundManager.GetInstance ().PlayVoiceOverClip (tutorial3);
        yield return new WaitForSeconds (tutorial3.length - 1f);
		tutorialHand.SetActive (true);

		tutorialHand.GetComponent<Animator> ().Play ("DragBananaToStripe");

		yield return new WaitForSeconds(5f);
		tutorialHand.SetActive (false);
		tutorialBanana.SetActive (false);
		redOutline.SetActive(false);

		if (runningTutorial) {
            subtitlePanel.Display ("Now You Try!", voData.FindVO ("4_tutorial_tryit"));
			tutorialPlayerBanana.SetActive(true);
            inputAllowed = true;
            bananaOrigin = tutorialOrigin;
			print (tutorialPlayerBanana);
			tutorialPlayerBanana.GetComponent<BrainbowFood> ().SetOrigin (bananaOrigin);
		}
	}

	IEnumerator TutorialTearDown ()	{
		StopCoroutine (tutorialCoroutine);
        GameManager.GetInstance ().CompleteTutorial (DataType.Minigame.Brainbow);
        inputAllowed = false;
        score = 0;
		UpdateScoreGauge();
		runningTutorial = false;

        AudioClip letsPlay = voData.FindVO ("letsplay");
        subtitlePanel.Display("Perfect!", letsPlay, true);
		yield return new WaitForSeconds(letsPlay.length + 1f);

		subtitlePanel.Hide ();
        waterNotification.SetActive (true);
        AudioClip water = voData.FindVO ("water");
        SoundManager.GetInstance ().PlayVoiceOverClip (water);

        yield return new WaitForSeconds (water.length + 1f);
        waterNotification.GetComponent<Animator> ().SetBool ("Active", true);
        yield return new WaitForSeconds (1f);
        instructionPopup.gameObject.SetActive(false);
		StartGame ();
	}

	public bool GetRunningTutorial() {
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
		timer.gameObject.SetActive(true);
		timer.SetTimeLimit (timeLimit);
		waterSpawnTime = Random.Range(5, 10);

		StartCoroutine (DisplayGo());
	}

	public void CreateWater() {
		print ("Create Water");
		int selection = Random.Range (0, 4);
		GameObject water = Instantiate(
			waterBottle,
			waterSpawnLocations [selection].transform
		);

		water.transform.localPosition = new Vector3 (0f,0f,0f);
		water.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
	}

	public IEnumerator DisplayGo () {
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().Countdown ();
		yield return new WaitForSeconds (4.0f);
		PostCountdownSetup ();
	}

	void PostCountdownSetup () {
        inputAllowed = true;
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

	void ChooseFoodsFromManager() {
		FoodList listOfFoods = GameManager.GetInstance ().GetComponent<FoodList> ();
		List<GameObject> redFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Red));
		List<GameObject> yellowFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Yellow));
		List<GameObject> greenFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Green));
		List<GameObject> purpleFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Purple));

        // Go through restrictedFoods list and remove it from the pool of food to choose from
        foreach (GameObject food in restrictedFoods) {
            switch (food.GetComponent<Food> ().color) {
                case Colorable.Color.Red:
                    RemoveRestrictedFoods (redFoodsList, food);
                    break;
                case Colorable.Color.Yellow:
                    RemoveRestrictedFoods (yellowFoodsList, food);
                    break;
                case Colorable.Color.Green:
                    RemoveRestrictedFoods (greenFoodsList, food);
                    break;
                case Colorable.Color.Purple:
                    RemoveRestrictedFoods (purpleFoodsList, food);
                    break;
            }
        }

		AddFoodsToList (redFoodsList);
		AddFoodsToList (yellowFoodsList);
		AddFoodsToList (greenFoodsList);
		AddFoodsToList (purpleFoodsList);
	}

    void RemoveRestrictedFoods(List<GameObject> inputlist, GameObject objToRemove) {
        inputlist.Remove (objToRemove);
        print ("Removed: " + objToRemove);
    }

	void AddFoodsToList(List<GameObject> goList) {
		int randomIndex;

        // Go through given list choose 5 different foods
        for (int i = 0; i < 5; i++) {
            randomIndex = Random.Range (0, goList.Count);
            foods.Add (goList[randomIndex]);
            goList.RemoveAt (randomIndex);
		}
	}

	void SpawnFood(Transform spawnPos) {
		int randomIndex = Random.Range (0, foods.Count);
		GameObject newFood = Instantiate(foods[randomIndex]);
		newFood.name = foods[randomIndex].name;
		BrainbowFood brainbowComponent = newFood.AddComponent<BrainbowFood> ();
		brainbowComponent.SetOrigin(spawnPos);
		newFood.GetComponent<Food>().Spawn(spawnPos, spawnParent, foodScale);
		SetActiveFood(brainbowComponent);
		inGameFoods.Add (newFood);
		foods.RemoveAt(randomIndex);
	}

	public void SetActiveFood(BrainbowFood food) {
		activeFood = food;
	}

	override public void GameOver() {
        SoundManager.GetInstance ().PlayCorrectSFX ();
		spawnWater = false;
		if (score >= scoreGoals[difficultyLevel]) {
            GameManager.GetInstance ().LevelUp (DataType.Minigame.Brainbow);
            if (difficultyLevel == 1) {
                UnlockSticker ();
            } else {
                GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.CompletedLevel);
            }
        }
        else {
            GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.FailedLevel);
        }
    }

	void EndGameTearDown () {
        if (gameStarted) {
            subtitlePanel.Hide ();
            gameStarted = false;
            inputAllowed = false;

            timer.StopTimer ();
            gameOver = true;

            if (activeFood != null) {
                activeFood.StopMoving ();
            }

            timer.gameObject.SetActive (false);

            StartCoroutine (RunEndGameAnimation ());
        }
	}

	IEnumerator RunEndGameAnimation() {
		animIsPlaying = true;

		foreach (GameObject food in inGameFoods) {
			food.GetComponent<Collider2D>().enabled = true;
		}

		SoundManager.GetInstance().AddToVOQueue(voData.FindVO("yay"));
		monsterObject.PlayEat ();

		yield return new WaitForSeconds (14f);
		GameOver ();
	}

    /*
	public void DisplayGameOverCanvas () {
		gameoverCanvas.gameObject.SetActive (true);
		Text gameoverScore = gameoverCanvas.GetComponentInChildren<Text> ();
		gameoverScore.text = "Good job! You fed your monster " + score + " healthy brain foods!";
	}
    */

    void UpdateScoreGauge () {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / scoreGoals[difficultyLevel]);
    }

    public bool GetGameStarted() { return gameStarted; }

	public bool GetGameOver() { return gameOver; }

	void CreateMonster() {
        // Blue = 0, Green = 1, Red = 2, Yellow = 3
        DataType.MonsterType typeOfMonster = GameManager.GetInstance ().GetMonsterType ();

        switch (typeOfMonster) {
		case DataType.MonsterType.Blue:
			InstantiateMonster (monsterList [(int)DataType.MonsterType.Blue]);
			break;
		case DataType.MonsterType.Green:
			InstantiateMonster (monsterList [(int)DataType.MonsterType.Green]);
			break;
		case DataType.MonsterType.Red:
			InstantiateMonster (monsterList [(int)DataType.MonsterType.Red]);
			break;
		case DataType.MonsterType.Yellow:
			InstantiateMonster (monsterList [(int)DataType.MonsterType.Yellow]);
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

	public void ShowSubtitles(string subtitle, AudioClip clip = null, bool queue = false) {
		subtitlePanel.Display (subtitle, clip, queue);
	}

	public void HideSubtitles() {
		subtitlePanel.Hide ();
	}
}