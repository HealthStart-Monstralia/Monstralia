using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : AbstractGameManager {
    [System.Serializable]
    public struct BrainbowLevelConfig {
        public int scoreGoal;
        public int numOfFoodSlots;
        public int maxFoodItems;
        public float timeLimit;
    }

    [Header ("Brainbow")]
    public VoiceOversData voData;
    public BrainbowFoodPanel foodPanel;
	public Canvas instructionPopup;
    public GameObject waterNotification;
	public GameObject tutorialHand;
	public GameObject tutorialBanana;
	public GameObject tutorialPlayerBanana;
	public GameObject tutorialSpot;
    public GameObject[] stripes;
	public List<GameObject> foods;
	public List<GameObject> inGameFoods = new List<GameObject>();
    public List<GameObject> restrictedFoods;
	public float foodScale;
	public BrainbowLevelConfig levelOne, levelTwo, levelThree;

    [HideInInspector] public bool inputAllowed = false;

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

    private static BrainbowGameManager instance;
    [SerializeField] private int score = 0;
    private int scoreGoal = 10;
    private BrainbowFood activeFood;
    private bool gameStarted;
    private int difficultyLevel;
    private bool runningTutorial = false;
    private Text nowYouTryText;
    private BrainbowFood banana;
    private Transform bananaOrigin;
    private bool gameOver = false;
    private Coroutine tutorialCoroutine;
    private ScoreGauge scoreGauge;
    private TimerClock timer;
    private List<GameObject> redFoodsList = new List<GameObject> ();
    private List<GameObject> yellowFoodsList = new List<GameObject> ();
    private List<GameObject> greenFoodsList = new List<GameObject> ();
    private List<GameObject> purpleFoodsList = new List<GameObject> ();

    private float waterSpawnTime = 0.0f;
    private bool spawnWater = false;

    void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
        CheckForGameManager ();

        SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
        instructionPopup.gameObject.SetActive (false);
        tutorialHand.SetActive (false);
        waterNotification.SetActive (false);
        foodPanel.DeactivateGameObject ();
    }

	public static BrainbowGameManager GetInstance() {
		return instance;
	}

	void Update() {
		if(runningTutorial && score == 1) {
			StartCoroutine(TutorialTearDown());
		}

		if (gameStarted) {
			if (score >= GetLevelConfig().maxFoodItems) {
                // Animation
                print ("WIN");
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
        difficultyLevel = GameManager.GetInstance ().GetLevel (DataType.Minigame.Brainbow);
        StartCoroutine (TurnOnRainbows ());
        timer = TimerClock.GetInstance ();
        scoreGauge = ScoreGauge.GetInstance ();
        scoreGauge.gameObject.SetActive (false);
        timer.gameObject.SetActive (false);

        scoreGoal = GetLevelConfig ().scoreGoal;
        
        CreateMonster ();
        monsterObject.PlaySpawn ();


        ChooseFoodsFromManager ();
        foodPanel.gameObject.SetActive (true);


        StartCoroutine (StartSpawningFoods ());
        gameStarted = true;

        /*

        difficultyLevel = GameManager.GetInstance ().GetLevel (DataType.Minigame.Brainbow);

        if (GameManager.GetInstance ().GetPendingTutorial(DataType.Minigame.Brainbow)) {
			tutorialCoroutine = StartCoroutine (RunTutorial ());
		}
		else {
			StartGame ();
		}
        */
    }

    IEnumerator TurnOnRainbows() {
        yield return new WaitForSeconds (0.5f);
        stripes[0].SetActive (true);
        yield return new WaitForSeconds (0.25f);
        stripes[1].SetActive (true);
        yield return new WaitForSeconds (0.25f);
        if (difficultyLevel > 1) {
            stripes[2].SetActive (true);
            yield return new WaitForSeconds (0.25f);
            if (difficultyLevel > 2) {
                stripes[3].SetActive (true);
            }
        }

    }

    IEnumerator StartSpawningFoods() {
        int numSlots = GetLevelConfig ().numOfFoodSlots;
        yield return new WaitForSeconds (0.55f);
        foodPanel.TurnOnNumOfSlots (numSlots);
        for (int i = 0; i < numSlots; ++i) {
            SpawnFood (foodPanel.slots[i]);
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
		timer.SetTimeLimit (GetLevelConfig().timeLimit);
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

		timer.StartTimer ();
		spawnWater = true;
	}

	public void Replace(Transform toReplace) {
		++score;
		if (!runningTutorial) {
			UpdateScoreGauge();
			if (foods.Count > 0) {
				SpawnFood(toReplace);
			}
		}
	}

	void ChooseFoodsFromManager() {
        // Retrieve food list from GameManager and sort them into colors
        SortBrainbowFoods ();

        // Pick five random foods from each category and store them in a food pool for SpawnFood
        AddFoodsToList (redFoodsList);
		AddFoodsToList (yellowFoodsList);
        if (difficultyLevel > 1) {
            AddFoodsToList (greenFoodsList);
            if (difficultyLevel > 2) {
                AddFoodsToList (purpleFoodsList);
            }
        }
	}

    // Sort foods into categories
    void SortBrainbowFoods () {
        FoodList foodList = GameManager.GetInstance ().GetComponent<FoodList> ();
        Food foodComponent;

        // Remove any restricted foods
        List<GameObject> brainbowFoods = foodList.goodFoods;
        foreach (GameObject food in restrictedFoods) {
            brainbowFoods.Remove (food);
        }

        // Sort food into corresponding color categories
        foreach (GameObject food in brainbowFoods) {
            foodComponent = food.GetComponent<Food> ();
            if (foodComponent.foodType == Food.TypeOfFood.Fruit || foodComponent.foodType == Food.TypeOfFood.Vegetable) {
                switch (foodComponent.color) {
                    case Colorable.Color.Red:
                        redFoodsList.Add (food);
                        break;

                    case Colorable.Color.Yellow:
                        yellowFoodsList.Add (food);
                        break;

                    case Colorable.Color.Green:
                        greenFoodsList.Add (food);
                        break;

                    case Colorable.Color.Purple:
                        purpleFoodsList.Add (food);
                        break;
                }
            }
        }
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

    // Tell food panel to spawn a random food at given transform
	void SpawnFood(Transform spawnPos) {
        // Grab a random food item from food pool
		int randomIndex = Random.Range (0, foods.Count);

        // Create random food at given transform
        GameObject newFood = foodPanel.CreateItemAtSlot (foods[randomIndex], spawnPos);
        // Name the created food item and give it a BrainbowFoodItem component
        newFood.name = foods[randomIndex].name;
		BrainbowFoodItem brainbowComponent = newFood.AddComponent<BrainbowFoodItem> ();

		newFood.GetComponent<Food>().Spawn(spawnPos, spawnPos, foodScale);
        inGameFoods.Add (newFood);

        // Remove created food item from food pool
		foods.RemoveAt(randomIndex);
    }

    override public void GameOver() {
        SoundManager.GetInstance ().PlayCorrectSFX ();
		spawnWater = false;
		if (score >= scoreGoal) {
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
            foodPanel.Deactivate ();

            if (activeFood != null) {
                activeFood.StopMoving ();
            }

            timer.gameObject.SetActive (false);

            StartCoroutine (RunEndGameAnimation ());
        }
	}

	IEnumerator RunEndGameAnimation() {
		foreach (GameObject food in inGameFoods) {
			food.GetComponent<Collider2D>().enabled = true;
		}

		SoundManager.GetInstance().AddToVOQueue(voData.FindVO("yay"));
		monsterObject.PlayEat ();

		yield return new WaitForSeconds (14f);
		GameOver ();
	}

    void UpdateScoreGauge () {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / scoreGoal);
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

    BrainbowLevelConfig GetLevelConfig() {
        switch (difficultyLevel) {
            case 1:
                return levelOne;
            case 2:
                return levelTwo;
            case 3:
                return levelThree;
            default:
                return levelOne;
        }
    }
}