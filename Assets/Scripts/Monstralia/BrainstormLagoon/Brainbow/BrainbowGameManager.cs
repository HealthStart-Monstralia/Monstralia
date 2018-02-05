using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowGameManager : AbstractGameManager<BrainbowGameManager> {
    [System.Serializable]
    public struct BrainbowLevelConfig {
        public int scoreGoal;
        public int numOfFoodSlots;
        public int maxFoodItems;
        public float waterTimeBoost;
        public float timeLimit;
    }

    [Header ("Brainbow")]
    public VoiceOversData voData;
    public BrainbowFoodPanel foodPanel;
    public BrainbowTutorialManager tutorialManager;
    public BrainbowStripe[] stripes;
	public List<GameObject> foods;
    public List<GameObject> restrictedFoods;
	public float foodScale;
    public bool skipTutorial = false;
    public BrainbowLevelConfig levelOne, levelTwo, levelThree;
    [HideInInspector] public bool inputAllowed = false;
    public BrainbowWaterManager waterManager;
	public SubtitlePanel subtitlePanel;
    public List<BrainbowFoodItem> activeFoods = new List<BrainbowFoodItem> ();

    [HideInInspector] public Monster monsterObject;

	public AudioClip[] correctMatchClips;
	public AudioClip[] wrongMatchClips;
	public AudioClip munchSound;
	public AudioClip ambientSound;
	public AudioClip correctSound;
	public AudioClip incorrectSound;

    // Events
    public delegate void GameAction ();
    public static event GameAction OnGameStart, OnGameEnd;

    [SerializeField] private int score = 0;
    private int scoreGoal = 10;
    private bool gameStarted;
    private int difficultyLevel;
    private Text nowYouTryText;
    private BrainbowFoodItem banana;
    private Transform bananaOrigin;

    private ScoreGauge scoreGauge;
    private TimerClock timer;
    private List<GameObject> redFoodsList = new List<GameObject> ();
    private List<GameObject> yellowFoodsList = new List<GameObject> ();
    private List<GameObject> greenFoodsList = new List<GameObject> ();
    private List<GameObject> purpleFoodsList = new List<GameObject> ();

    // Event
    public void OnOutOfTime () { EndGameTearDown (); }

    public override void PregameSetup () {
        SoundManager.Instance.ChangeAmbientSound (ambientSound);
        foodPanel.DeactivateGameObject ();

        if (skipTutorial) GameManager.Instance.CompleteTutorial (DataType.Minigame.Brainbow);
        difficultyLevel = GameManager.Instance.GetLevel (DataType.Minigame.Brainbow);
        timer = TimerClock.Instance;
        scoreGauge = ScoreGauge.Instance;
        scoreGauge.gameObject.SetActive (false);
        timer.gameObject.SetActive (false);
        scoreGoal = GetLevelConfig ().scoreGoal;
        waterManager.waterTimeBoost = GetLevelConfig ().waterTimeBoost;

        monsterObject = monsterCreator.SpawnMonster (
            GameManager.Instance.GetPlayerMonsterObject ());
        monsterObject.transform.localPosition = monsterCreator.transform.position;
        monsterObject.transform.localScale = Vector3.one * 0.4f;
        monsterObject.spriteRenderer.sortingOrder = 0;
        monsterObject.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        if (GameManager.Instance.GetPendingTutorial (DataType.Minigame.Brainbow)) {
            tutorialManager.gameObject.SetActive (true);
            tutorialManager.StartTutorial ();
        } else {
            StartGame ();
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

	public void StartGame() {
		scoreGauge.gameObject.SetActive(true);
		timer.gameObject.SetActive(true);
		timer.SetTimeLimit (GetLevelConfig().timeLimit);
        foodPanel.gameObject.SetActive (true);
        activeFoods.Clear ();
        StartCoroutine (StartSpawningFoods ());
        StartCoroutine (TurnOnRainbows ());
        ChooseFoodsFromManager ();
        StartCoroutine (PreCountdownSetup ());
    }

    IEnumerator PreCountdownSetup () {
        yield return new WaitForSeconds (2.0f);
        StartCountdown (PostCountdownSetup);
    }

	void PostCountdownSetup () {
        inputAllowed = true;
		gameStarted = true;
        monsterObject.ChangeEmotions (DataType.MonsterEmotions.Happy);
        timer.StartTimer ();
        OnGameStart ();
    }

	public void ScoreAndReplace(Transform toReplace) {
        // Update score
        if (tutorialManager.isRunningTutorial) {
            tutorialManager.tutorialCoroutine = StartCoroutine (tutorialManager.TutorialEnd ());
        }
        else {
            ++score;
            UpdateScoreGauge ();

            // If any remain in food pool, create food to replace last food
            if (foods.Count > 0) {
                SpawnFood (toReplace);
            }

            if (score >= GetLevelConfig ().maxFoodItems) {
                // Animation
                EndGameTearDown ();
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
        FoodList foodList = GameManager.Instance.GetComponent<FoodList> ();
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
        newFood.name = foods[randomIndex].GetComponent<Food> ().foodName;
		newFood.AddComponent<BrainbowFoodItem> ();
		newFood.GetComponent<Food>().Spawn(spawnPos, spawnPos, foodScale);

        // Remove created food item from food pool
		foods.RemoveAt(randomIndex);
    }

	void EndGameTearDown () {
        if (gameStarted) {
            subtitlePanel.Hide ();
            gameStarted = false;
            inputAllowed = false;

            timer.StopTimer ();
            foodPanel.Deactivate ();

            StartCoroutine (RunEndGameAnimation ());
        }
	}

	IEnumerator RunEndGameAnimation() {
        OnGameEnd ();
        monsterObject.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        yield return new WaitForSeconds (2f + 0.1f*activeFoods.Count);
        SoundManager.Instance.AddToVOQueue(voData.FindVO("yay"));
        monsterObject.ChangeEmotions (DataType.MonsterEmotions.Thoughtful);
        StartCoroutine (TurnOffRainbows ());
        yield return new WaitForSeconds (3f);
        
        // Player reached goal
        if (score >= scoreGoal) {

            // If level one, give player a sticker and complete the level, 
            // otherwise complete the level normally.
            if (difficultyLevel == 1) {
                GameOver (DataType.GameEnd.EarnedSticker);
            } else {
                GameOver (DataType.GameEnd.CompletedLevel);
            }
        } 
        
        // Player did not reach goal, show game over
        else {
            GameOver (DataType.GameEnd.FailedLevel);
        }
    }

    void UpdateScoreGauge () {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / scoreGoal);
    }

    public bool GetGameStarted() { return gameStarted; }

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

    public void ShowRainbowStripe (int selection, bool show) {
        if (show) {
            stripes[selection].gameObject.SetActive (true);
        }
        else {
            stripes[selection].ClearStripe ();
            stripes[selection].GetComponent<Animator> ().Play ("StripeFadeOut");
            StartCoroutine (TurnOffStripe (stripes[selection].gameObject));
        }
    }

    IEnumerator TurnOffStripe (GameObject stripe) {
        yield return new WaitForSeconds (0.5f);
        stripe.SetActive (false);
    }

    IEnumerator TurnOnRainbows () {
        yield return new WaitForSeconds (0.5f);
        ShowRainbowStripe (0, true);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (1, true);
        yield return new WaitForSeconds (0.25f);
        if (difficultyLevel > 1) {
            ShowRainbowStripe (2, true);
            yield return new WaitForSeconds (0.25f);
            if (difficultyLevel > 2) {
                ShowRainbowStripe (3, true);
            }
        }
    }

    IEnumerator TurnOffRainbows () {
        yield return new WaitForSeconds (1.5f);
        if (difficultyLevel > 2) {
            ShowRainbowStripe (3, false);
            yield return new WaitForSeconds (0.25f);
        }
        if (difficultyLevel > 1) {
            ShowRainbowStripe (2, false);
            yield return new WaitForSeconds (0.25f);
        }

        ShowRainbowStripe (1, false);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (0, false);
        yield return new WaitForSeconds (0.25f);
    }
}