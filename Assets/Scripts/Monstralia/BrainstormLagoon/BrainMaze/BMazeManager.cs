using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CREATED BY: Colby Tang
 * GAME: Brain Maze
 */

public class BMazeManager : AbstractGameManager<BMazeManager>
{
    [Header("Brain Maze Fields")]
    public MilestoneManager milestoneManager;
    public VoiceOversData voData;
    public bool InputAllowed {
        get {
            return inputAllowed;
        }

        set {
            inputAllowed = value;
            BMazeMonsterMovement.isMonsterMovementAllowed = value;
        }
    }
    private bool inputAllowed = false;

    [System.Serializable]
    public struct BrainMazeLevelConfig
    {
        public float timeLimit;
        public int scoreGoal;
        public IntVector2 mazeSize;
    }

    public BrainMazeLevelConfig levelOne, levelTwo, levelThree;
    public GameObject backButton;
    public GameObject tutorialHand;
    public float generationStepDelay;

    private BrainMazeLevelConfig levelConfig;
    private bool gameStarted = false;
    private Coroutine tutorialCoroutine;
    private Animator monsterAnimator;
    private int score;
    private int scoreGoal;
    private int numOfPickupsToSpawn;
    private int numOfPickups = 0;
    private bool isTutorialRunning = false;
    private Maze mazeInstance;
    private List<GameObject> pickupPrefabList = new List<GameObject>();
    private Transform monsterStart;

    [Header("References")]
    [SerializeField]
    private Maze mazePrefab;
    [SerializeField] private BMazeFactory factory;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private AudioClip unlockedDoorSfx;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject tutorialPickup;
    [SerializeField] private Transform tutorialStartingSpot;
    [SerializeField] private Door tutorialDoor;
    [SerializeField] private BMazeFinishline tutorialFinishline;
    [HideInInspector] public Door doorInstance;
    [HideInInspector] public BMazeFinishline finishLine;

    private new void Awake()
    {
        base.Awake();

        IntVector2 size = GetMazeSize();
        int numOfSpawnableTiles = size.x * size.y - 2;
        if (numOfSpawnableTiles < 7)
        {
            size.x = 3;
            size.x = 3;
        }

        if (numOfPickupsToSpawn > numOfSpawnableTiles)
        {
            numOfPickupsToSpawn = numOfSpawnableTiles - 1;
        }

        tutorial.SetActive(false);
        pickupPrefabList.AddRange(GetFactoryList());
    }

    private void OnDisable () {
        finishLine.OnFinish -= OnFinish;
    }

    public override void PregameSetup () {
        if (SoundManager.Instance) {
            SoundManager.Instance.ChangeAndPlayAmbientSound (ambientSound);
            SoundManager.Instance.StopPlayingVoiceOver ();
        }

        backButton.SetActive(true);
        tutorialHand.SetActive(false);

        UpdateScoreGauge();
        ResetScore();

        if (GameManager.Instance.GetPendingTutorial(DataType.Minigame.BrainMaze))
        {
            tutorialCoroutine = StartCoroutine(RunTutorial());
        }
        else
        {
            SelectLevel();
            scoreGoal = levelConfig.scoreGoal;
            ScoreGauge.Instance.gameObject.SetActive(true);
            TimerClock.Instance.gameObject.SetActive(true);
            TimerClock.Instance.SetTimeLimit(levelConfig.timeLimit);
            StartCoroutine(GenerateMaze());
        }
    }

    IEnumerator GenerateMaze()
    {
        yield return null;
        score = 0;
        UpdateScoreGauge();
        numOfPickups = 0;
        numOfPickupsToSpawn = scoreGoal;
        mazeInstance = Instantiate(mazePrefab, transform.position, Quaternion.identity) as Maze;
        StartCoroutine(mazeInstance.Generate(PostGeneration, generationStepDelay));
    }
    
    void PostGeneration () {
        TimerClock.Instance.SetTimeLimit (levelConfig.timeLimit);

        finishLine.OnFinish += OnFinish;
        finishLine.isActivated = true;

        monsterStart = mazeInstance.GetFirstCell ().transform;
        mazeInstance.ScaleMaze ();
        StartCountdown (GameStart, 2f);
        Invoke ("CreateMonster", 1f);
    }

    IEnumerator RunTutorial()
    {
        isTutorialRunning = true;
        scoreGoal = 2;
        doorInstance = tutorialDoor;
        finishLine = tutorialFinishline;
        tutorial.SetActive(true);
        print("RunTutorial");
        ScoreGauge.Instance.gameObject.SetActive(false);
        TimerClock.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);

        SubtitlePanel.Instance.Display("Welcome to Brain Maze!", null);
        SoundManager.Instance.StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO("1_tutorial_start");
        AudioClip tutorial2 = voData.FindVO("2_tutorial_drag");
        AudioClip tutorial3 = voData.FindVO("3_tutorial_letmeshow");
        SoundManager.Instance.PlayVoiceOverClip(tutorial1);
        yield return new WaitForSeconds(tutorial1.length);

        SubtitlePanel.Instance.Hide();
        monsterStart = tutorialStartingSpot;
        CreateMonster ();
        playerMonster.transform.localScale = Vector3.one * 0.2f;
        InputAllowed = false;

        SoundManager.Instance.PlayVoiceOverClip(tutorial2);
        yield return new WaitForSeconds(tutorial2.length);

        SoundManager.Instance.PlayVoiceOverClip(tutorial3);
        yield return new WaitForSeconds(tutorial3.length);

        tutorialHand.SetActive(true);
        tutorialHand.GetComponent<Animator>().Play("BMaze_HandMoveMonster");
        Transform monsterParent = playerMonster.transform.parent;
        yield return new WaitForSeconds(1.5f);

        playerMonster.transform.SetParent(tutorialHand.transform);
        yield return new WaitForSeconds(1.5f);

        playerMonster.transform.SetParent(monsterParent);
        yield return new WaitForSeconds(1.5f);

        SubtitlePanel.Instance.Display("Now you try!");
        SoundManager.Instance.PlayVoiceOverClip(
            voData.FindVO("nowyoutry")
            );

        ResetScore();
        playerMonster.transform.position = monsterStart.position;
        tutorialPickup.SetActive (true);
        InputAllowed = true;
        SubtitlePanel.Instance.Hide ();
		yield return new WaitForSeconds(2f);

        SubtitlePanel.Instance.Display("Get all the pickups!", null);
        yield return new WaitForSeconds(5f);

        SubtitlePanel.Instance.Hide();
    }

	public void TutorialFinished() {
		InputAllowed = false;
        tutorialHand.SetActive (false);
		GameManager.Instance.CompleteTutorial(DataType.Minigame.BrainMaze);
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

    IEnumerator TutorialTearDown()
    {
        print("TutorialTearDown");
        SubtitlePanel.Instance.Display("Let's play!", null);
        yield return new WaitForSeconds(2.0f);

        isTutorialRunning = false;
        yield return new WaitForSeconds(1.0f);

        SubtitlePanel.Instance.Hide();
        tutorial.SetActive(false);
        PregameSetup();
    }

    public void OnScore(BMazePickup obj)
    {
        score++;
        UpdateScoreGauge();
        if (score >= scoreGoal)
        {
            doorInstance.OpenDoor();
            finishLine.UnlockFinishline();
            SoundManager.Instance.AddToVOQueue(voData.FindVO("unlockeddoor"));
            SoundManager.Instance.PlaySFXClip(unlockedDoorSfx);
        }
    }

    public void UpdateScoreGauge()
    {
        if (ScoreGauge.Instance.gameObject.activeSelf)
            ScoreGauge.Instance.SetProgressTransition((float)score / scoreGoal);
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreGauge();
    }

    public void OnOutOfTime()
    {
        GameEnd();
    }

    public void OnFinish() {
        if (isTutorialRunning) {
            MonsterVictoryDance ();
            TutorialFinished ();
        } else if (gameStarted) {
            MonsterVictoryDance ();
            GameEnd ();
        }
    }

	public void GameStart () {
		gameStarted = true;
        InputAllowed = true;
        TimerClock.Instance.StartTimer ();
	}

    public void GameEnd()
    {
        gameStarted = false;
        InputAllowed = false;

        MonsterVictoryDance();
        AudioClip youdidit = voData.FindVO("youdidit");
        SoundManager.Instance.AddToVOQueue(youdidit);
        TimerClock.Instance.StopTimer();

        StartCoroutine(EndGameWait(3f));
    }

    void MonsterVictoryDance()
    {
        PlayDance();
        playerMonster.GetComponent<BMazeMonsterMovement>().MoveToFinishLine(finishLine.transform.position);
    }

    public IEnumerator EndGameWait (float duration) {
        yield return new WaitForSeconds (duration);
        EndGame ();
    }

    void EndGame() {
        if (!isTutorialRunning && !gameStarted) {
            if (score >= scoreGoal) {
                if (GameManager.Instance.GetLevel (typeOfGame) == 1) {
                    MilestoneManager.Instance.UnlockMilestone (DataType.Milestone.BrainMaze1);
                } else if (GameManager.Instance.GetLevel (typeOfGame) == 3) {
                    MilestoneManager.Instance.UnlockMilestone (DataType.Milestone.BrainMaze3);
                }
                if (!GameManager.Instance.GetIsStickerUnlocked (typeOfGame)) {
                    GameOver (DataType.GameEnd.EarnedSticker);
                } else {
                    GameOver (DataType.GameEnd.CompletedLevel);
                }
            }
            else
            {
                GameOver(DataType.GameEnd.FailedLevel);
            }
        }
    }

    public void ChangeScene()
    {
        GetComponent<SwitchScene>().LoadScene();
    }

    public void SkipLevel(GameObject button)
    {
        if (isTutorialRunning)
        {
            TutorialFinished();
        }
        else
        {
            gameStarted = false;
            if (GameManager.Instance)
                GameManager.Instance.LevelUp(DataType.Minigame.BrainMaze);
            Instance.ChangeScene();
        }
        button.SetActive(false);
    }

	public void ChangeScene () {
		GetComponent<SwitchScene> ().LoadScene ();
	}

	public void SkipLevel (GameObject button) {
		if (isTutorialRunning) {
			TutorialFinished ();
		} else {
			gameStarted = false;
			if (GameManager.Instance)
				GameManager.Instance.LevelUp (DataType.Minigame.BrainMaze);
			Instance.ChangeScene ();
		}
		button.SetActive (false);
	}

	void CreateMonster() {
        CreatePlayerMonster (monsterStart);
        playerMonster.transform.localScale = Vector3.one * 0.1f;
        playerMonster.transform.SetParent (monsterStart.transform.root);
        playerMonster.transform.gameObject.AddComponent<BMazeMonsterMovement> ();
        monsterAnimator = playerMonster.GetComponentInChildren<Animator> ();
        PlaySpawn ();
    }

    public void RemoveMonster()
    {
        if (playerMonster)
            Destroy(playerMonster);
    }

    public List<GameObject> GetFactoryList()
    {
        return factory.pickupPrefabList;
    }

    public GameObject CreatePickup(MazeCell cell)
    {
        if (numOfPickupsToSpawn > 0)
        {
            GameObject prefabSelected;

            // Create one of each pickup at least once, afterwards randomly select a pickup to spawn
            if (numOfPickups < pickupPrefabList.Count)
            {
                prefabSelected = pickupPrefabList[numOfPickups].gameObject;
            }
            else
            {
                prefabSelected = pickupPrefabList.GetRandomItem().gameObject;
            }

            numOfPickupsToSpawn--;
            numOfPickups++;
            GameObject product = OrderPickupFromFactory(prefabSelected, cell.transform);
            return product;
            //return Instantiate (prefabSelected, cell.transform.position, Quaternion.identity, cell.transform);
        }
        else
        {
            return null;
        }
    }

    public GameObject OrderPickupFromFactory(GameObject prefab, Transform parent)
    {
        GameObject pickup = factory.Manufacture(prefab, parent);
        return pickup;
    }

    public void SetFactoryScale(float scale)
    {
        factory.scale = Vector3.one * scale;
    }

    public void PlaySpawn()
    {
        monsterAnimator.StopPlayback();
        monsterAnimator.Play("BMaze_Spawn", -1, 0f);
    }

    public void PlayDance()
    {
        monsterAnimator.StopPlayback();
        monsterAnimator.Play("BMaze_Dance", -1, 0f);
    }

    void SelectLevel()
    {
        switch (GameManager.Instance.GetLevel(DataType.Minigame.BrainMaze))
        {
            default:
                levelConfig = levelOne;
                break;
            case 2:
                levelConfig = levelTwo;
                break;
            case 3:
                levelConfig = levelThree;
                break;
        }
    }

    public bool GetIsTutorialRunning()
    {
        return isTutorialRunning;
    }

    public IntVector2 GetMazeSize()
    {
        return levelConfig.mazeSize;
    }
    
    public bool GetInputAllowed () {
        return InputAllowed;
    }
}