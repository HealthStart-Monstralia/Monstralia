using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MemoryMatchGameManager : AbstractGameManager<MemoryMatchGameManager>
{
    [System.Serializable]
    public struct MemoryMatchLevel
    {
        public float timeLimit;
        public int numDishes;
    }
    [Header("Memory Match Game Manager fields")]
    public MilestoneManager milestoneManager;
    public VoiceOversData voData;
    public MemoryMatchLevel levelOne, levelTwo, levelThree;
    [HideInInspector] public MemoryMatchLevel currentLevel;

    public Transform monsterSpawnPos;
    public Transform foodToMatchSpawnPos;
    public float foodScale;
    public float foodToMatchScale;
    public GameObject dishPrefab;
    public GameObject dishAnchor;

    public ScoreGauge scoreGauge;
    [HideInInspector] public bool animIsPlaying = false;
    [HideInInspector] public bool inputAllowed = false;
    [HideInInspector] public GameObject selectedFood;
    public float rotationalSpeed;

    public AudioClip[] wrongMatchClips;
    public AudioClip munchClip;
    [HideInInspector] public bool isGuessing = false;


    private bool gameStarted;
    private int score;
    [SerializeField] private MemoryMatchTutorialManager tutorialManager;
    [SerializeField] private AudioClip[] goodjobClips;
    private int numberOfDishes;
    private GameObject currentFoodToMatch;
    private int difficultyLevel;
    private List<GameObject> dishes = new List<GameObject>();
    private List<GameObject> foodList = new List<GameObject>();
    private List<GameObject> activeFoodList = new List<GameObject>();
    private bool isRunningTutorial = false;
    private float stopRotateTime;
    private Animator monsterAnimator;

    public override void PregameSetup()
    {
        difficultyLevel = GameManager.Instance.GetLevel(DataType.Minigame.MemoryMatch);
        currentLevel = GetLevelConfig(difficultyLevel);
        inputAllowed = false;

        CreatePlayerMonster(monsterSpawnPos);
        monsterAnimator = playerMonster.monsterAnimator.animator;
        monsterAnimator.Play ("MM_Spawn", -1, 0f);
        playerMonster.gameObject.transform.localScale = Vector3.one * 0.75f;
        playerMonster.spriteRenderer.sortingOrder = -2;

        SubtitlePanel.Instance.Hide();

        // Retrieve foods from Game Manager Food List
        // Use AddRange to copy lists. Assigning lists does not copy over the list, only the reference.
        foodList.AddRange (FoodList.GetGoodFoodsList ());

        if (GameManager.Instance.GetPendingTutorial(DataType.Minigame.MemoryMatch))
        {
            isRunningTutorial = true;
            tutorialManager.StartTutorial(ref selectedFood);
        }
        else
        {
            StartCoroutine(PrepareGame());
        }
    }

    public void OnTutorialFinish()
    {
        StartCoroutine(TutorialTearDown());
    }

    IEnumerator TutorialTearDown()
    {
        isRunningTutorial = false;
        score = 0;

        yield return new WaitForSeconds(2.0f);

        AudioClip letsPlay = voData.FindVO("letsplay");
        SubtitlePanel.Instance.Display("Perfect!", letsPlay);
        yield return new WaitForSeconds(2.0f);

        SubtitlePanel.Instance.Hide();
        tutorialManager.gameObject.SetActive(false);
        GameManager.Instance.CompleteTutorial(DataType.Minigame.MemoryMatch);
        StartCoroutine(PrepareGame());
    }

    IEnumerator PrepareGame()
    {
        yield return new WaitForSeconds(0.1f);
        playerMonster.transform.position = monsterSpawnPos.position;

        score = 0;
        SetTimeLimit(currentLevel.timeLimit);
        numberOfDishes = currentLevel.numDishes;

        ActivateHUD (true);
        SpawnDishes ();
        CreateFoodInDishes ();
        SubtitlePanel.Instance.Display ("Remember the foods!", voData.FindVO("memory_remember"));
        yield return new WaitForSeconds (4f);

        StartCoroutine(SpawnDishLids());
    }

    public void ActivateHUD(bool activate)
    {
        scoreGauge.gameObject.SetActive(activate);
        TimerClock.Instance.gameObject.SetActive(activate);
        if (activate)
            UpdateScoreGauge();
    }

    void SpawnDishes()
    {
        //Mathf.Cos/Sin use radians, so the dishes are positioned 2pi/numDishes apart
        float dishPositionAngleDelta = (2 * Mathf.PI) / numberOfDishes;
        bool reduceSize = false;
        if (numberOfDishes > 4)
            reduceSize = true;

        for (int i = 0; i < numberOfDishes; ++i)
        {
            GameObject newDish = Instantiate(dishPrefab, dishAnchor.transform);
            float offset = 2f;
            float dishX = offset * Mathf.Cos(dishPositionAngleDelta * i + Mathf.PI / 2);
            float dishY = offset * Mathf.Sin(dishPositionAngleDelta * i + Mathf.PI / 2);
            newDish.transform.localPosition = new Vector3(
                dishX,
                dishY,
                0);

            dishes.Add(newDish);

            // Reduce size of dish depending on number of dishes
            if (reduceSize)
            {
                newDish.transform.localScale = newDish.transform.localScale * 0.9f;
            }
        }
    }

    void CreateFoodInDishes()
    {
        for (int i = 0; i < numberOfDishes; ++i)
        {
            GameObject newChosenFood = foodList.RemoveRandom();
            DishObject dishComponent = dishes[i].GetComponent<DishObject>();
            GameObject newFoodObject = SpawnFood(newChosenFood, dishComponent.lid.transform, dishComponent.dish.transform, foodScale, true);
            dishComponent.SetFood(newFoodObject);
            activeFoodList.Add(newFoodObject);
        }
    }

    IEnumerator SpawnDishLids()
    {
        for (int i = 0; i < numberOfDishes; ++i)
        {
            dishes[i].GetComponent<DishObject>().SpawnLids(true);
            yield return new WaitForSeconds(0.25f);
        }

        if (difficultyLevel >= 1)
        {
            StartCoroutine(RotateDishes());
        }
        else
        {
            StartCountdown(GameStart);
        }
    }

    IEnumerator RotateDishes()
    {
        Vector3 zAxis = Vector3.forward; //<0, 0, 1>;
        float waitDuration = 0f;

        while (waitDuration < 5f)
        {
            print(waitDuration);
            for (int i = 0; i < numberOfDishes; ++i)
            {
                GameObject d = dishes[i];
                Quaternion startRotation = d.transform.rotation;

                d.transform.RotateAround(dishAnchor.transform.position, zAxis, rotationalSpeed * 0.75f);
                d.transform.rotation = startRotation;
            }

            waitDuration += 0.02f;
            yield return new WaitForFixedUpdate();
        }

        StartCountdown(GameStart);
    }

    void GameStart()
    {
        ChooseFoodToMatch();
        gameStarted = true;
        inputAllowed = true;
        StartTimer();
    }

    void ResetDishes()
    {
        for (int i = 0; i < difficultyLevel * 3; ++i)
        {
            GameObject dish = dishes[i];
            dish.GetComponent<DishObject>().Reset();
        }
    }

    public bool OnGuess(DishObject dish, GameObject food)
    {
        isGuessing = true;
        if (food == selectedFood) {
            if (Random.Range(0, 1f) < 0.3f) {
                SoundManager.Instance.PlayVoiceOverClip (goodjobClips.GetRandomItem ());
            }

            dish.Correct();
            SoundManager.Instance.PlayCorrectSFX();
            OnScore(food);
            return true;
        }

        return false;
    }

    public void ChooseFoodToMatch()
    {
        if (currentFoodToMatch)
            Destroy(currentFoodToMatch);

        selectedFood = activeFoodList.RemoveRandom();

        if (activeFoodList.Count >= 0)
        {
            currentFoodToMatch = SpawnFood(selectedFood, foodToMatchSpawnPos, foodToMatchSpawnPos, foodToMatchScale, false);
            currentFoodToMatch.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
            currentFoodToMatch.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
    }

    GameObject SpawnFood(GameObject foodObject, Transform spawnPos, Transform parent, float scale, bool needAnchor)
    {
        GameObject newFood = Instantiate(foodObject);
        newFood.name = foodObject.GetComponent<Food>().foodName;
        newFood.GetComponent<Food>().Spawn(spawnPos, parent, scale);

        if (needAnchor)
        {
            newFood.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            newFood.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
        }

        newFood.GetComponent<Collider2D>().enabled = false;

        newFood.transform.localPosition = new Vector3(0f, 1.1f, 0f);
        return newFood;
    }

    public void OnScore(GameObject food)
    {
        if (!isRunningTutorial)
        {
            ++score;
            UpdateScoreGauge();
            if (score >= numberOfDishes)
            {
                StartCoroutine(RunEndGameAnimation());
            }
            else
            {
                Invoke("ChooseFoodToMatch", 2f);
            }
        }
        else
        {
            StartCoroutine(TutorialTearDown());
        }
    }

    IEnumerator RunEndGameAnimation()
    {
        animIsPlaying = true;
        StopTimer();
        gameStarted = false;
        inputAllowed = false;
        yield return new WaitForSeconds (1.0f);

        for (int i = 0; i < numberOfDishes; ++i) {
			if(dishes[i].GetComponent<DishObject>().IsMatched ()) {
                GameObject food = dishes[i].GetComponent<DishObject> ().foodObject.gameObject;
                dishes[i].GetComponent<DishObject>().Shake(true);
                monsterAnimator.Play ("MM_Eat", -1, 0f);

                for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 2) {
                    food.transform.position = Vector2.Lerp (food.transform.position, playerMonster.transform.position + new Vector3 (0, 1f, 0), t);
                    yield return null;
                }

                food.GetComponent<Food> ().EatFood ();
                yield return new WaitForSeconds (0.5f);
            }
		}
        monsterAnimator.Play ("MM_Dance", -1, 0f);
        yield return new WaitForSeconds (1f);
        GameEnd ();
	}

    public void OnOutOfTime() {
        StartCoroutine (RunEndGameAnimation ());
    }

    public void GameEnd()
    {
        ActivateHUD(false);
        if (currentFoodToMatch.gameObject)
            currentFoodToMatch.gameObject.SetActive(false);
        EndScreen screen;
        if (score >= numberOfDishes)
        {
            if (difficultyLevel == 1)
            {
                milestoneManager.UnlockMilestone(DataType.Milestone.MemoryMatch1);
                screen = GameOver(DataType.GameEnd.EarnedSticker);
                screen.EditHeader("Great job! You matched " + score + " healthy foods and earned a new sticker!");
            }
            if (difficultyLevel == 3)
            {
                milestoneManager.UnlockMilestone(DataType.Milestone.MemoryMatch3);
                screen = GameOver(DataType.GameEnd.CompletedLevel);
                screen.EditHeader("Nice job! You matched " + score + " healthy foods!");
            }
            else
            {
                screen = GameOver(DataType.GameEnd.CompletedLevel);
                screen.EditHeader("Nice job! You matched " + score + " healthy foods!");
            }
        }
        else
        {
            screen = GameOver(DataType.GameEnd.FailedLevel);
            screen.EditHeader("Time ran out! You matched " + score + " healthy foods! Let's try again!");
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public bool IsRunningTutorial()
    {
        return isRunningTutorial;
    }

    void UpdateScoreGauge()
    {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition((float)score / numberOfDishes);
    }

    MemoryMatchLevel GetLevelConfig(int level)
    {
        switch (level)
        {
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
