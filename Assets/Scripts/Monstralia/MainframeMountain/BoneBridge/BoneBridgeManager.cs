using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/* CREATED BY: Colby Tang
 * GAME: Bone Bridge
 */

public class BoneBridgeManager : AbstractGameManager {
    public enum BridgePhase {
        Start,
        Countdown,
        Building,
        Falling,
        Crossing,
        Lose,
        Finish
    };

    [Header ("BoneBridgeManager Fields")]
    public BridgePhase bridgePhase;
    public DataType.Level difficultyLevel;
    public BoneBridgeLevel levelOne, levelTwo, levelThree;
    [HideInInspector] public BoneBridgeLevel currentLevel;

    public VoiceOversData voData;
    public BoneBridgeData config;
    public bool doCountdown, playIntro;
    public bool inputAllowed = false;
    [HideInInspector] public bool isTutorialRunning = false;
    public bool getLevelFromGameManager = true;
    public ScoreGauge scoreGauge;
    public SubtitlePanel subtitlePanel;

    public Timer timerObject;
    public BoneBridgeCamera boneCamera;
    public GameObject chest;

    [HideInInspector] public Monster monster;
    private Monster trappedMonster;

    [HideInInspector] public BoneBridgeMonster bridgeMonster;

    // Events
    public delegate void PhaseChangeAction (BridgePhase phase);
    public static event PhaseChangeAction PhaseChange;

    [SerializeField] private List<GameObject> monsterPool = new List<GameObject> ();
    [SerializeField] private List<DataType.MonsterType> savedMonsters = new List<DataType.MonsterType> ();
    private Transform start, goal;
    private Coroutine tutorialCoroutine;
    private bool gameStarted = false;
    private static BoneBridgeManager instance = null;
    private Rigidbody2D rigBody;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        scoreGauge.gameObject.SetActive (false);
        timerObject.gameObject.SetActive (false);
        inputAllowed = false;
    }

    public static BoneBridgeManager GetInstance () {
        return instance;
    }

    private void Update () {
        if (bridgeMonster) {
            ChangeProgressBar (
                (bridgeMonster.transform.position.x - start.position.x) / (goal.position.x - start.transform.position.x)
            );
        }
    }

    private void OnEnable () {
        PhaseChange += OnPhaseChange;
        Timer.OutOfTime += OnOutOfTime;
    }

    private void OnDisable () {
        PhaseChange -= OnPhaseChange;
        Timer.OutOfTime -= OnOutOfTime;
    }

    public override void PregameSetup () {
        if (getLevelFromGameManager)
            difficultyLevel = (DataType.Level)GameManager.GetInstance ().GetLevel (typeOfGame);
        ChangeLevel (GetLevelObject(difficultyLevel));

        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BoneBridge)) {
            tutorialCoroutine = StartCoroutine (Tutorial ());
        }
        else {
            CameraSwitch (currentLevel.monsterSpawn.gameObject);
            OnPhaseChange (BridgePhase.Start);
            StartCoroutine (Intro ());
        }
    }

    IEnumerator Intro () {
        yield return new WaitForSeconds (1.0f);
        CreateMonsters ();

        if (playIntro) {
            monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
            yield return new WaitForSeconds (1.0f);
            float initialDampTime = boneCamera.dampTime;
            boneCamera.dampTime = 1.5f;
            CameraSwitch (currentLevel.prizeSpawn.gameObject);
            CreatePrize ();
            yield return new WaitForSeconds (3.0f);
            boneCamera.dampTime = 1.0f;
            yield return new WaitForSeconds (0.5f);
            if (trappedMonster) monster.ChangeEmotions (DataType.MonsterEmotions.Afraid);
            boneCamera.dampTime = 0.7f;
            yield return new WaitForSeconds (1.0f);
            boneCamera.dampTime = initialDampTime;
            CameraSwitch (bridgeMonster.gameObject);
            yield return new WaitForSeconds (1.0f);
            monster.ChangeEmotions (DataType.MonsterEmotions.Thoughtful);
            yield return new WaitForSeconds (1.0f);
        }

        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        bridgeMonster.StartCoroutine (bridgeMonster.Move ());
        CameraSwitch (bridgeMonster.gameObject);
    }

    IEnumerator Tutorial () {
        isTutorialRunning = true;
        yield return new WaitForSeconds (1.0f);
        subtitlePanel.Display ("Welcome to Bone Bridge!");
        yield return new WaitForSeconds (3.0f);

        GameManager.GetInstance ().CompleteTutorial (typeOfGame);
        isTutorialRunning = false;
        PregameSetup ();
    }

    IEnumerator Countdown () {
        ChangePhase (BridgePhase.Countdown);
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().Countdown ();
        yield return new WaitForSeconds (4.0f);
        doCountdown = false;
        GameStart ();
    }

    public void GameStart () {
        timerObject.gameObject.SetActive (true);
        scoreGauge.gameObject.SetActive (true);
        if (doCountdown)
            StartCoroutine (Countdown ());
        else {
            gameStarted = true;
            ChangePhase (BridgePhase.Building);
        }
    }

    public bool GetGameStarted () { return gameStarted; }

    public override void GameOver () { ChangePhase(BridgePhase.Finish); }

    void OnOutOfTime () { ChangePhase (BridgePhase.Lose); }

    public void ChangePhase (BridgePhase phase) {
        if (bridgePhase != phase) {
            bridgePhase = phase;
            print ("Manager ChangePhase firing: " + phase);
            PhaseChange (phase);
        }
    }

    void OnPhaseChange (BridgePhase phase) {
        switch (phase) {
            case BridgePhase.Start:
                inputAllowed = false;
                break;
            case BridgePhase.Countdown:
                inputAllowed = false;
                break;
            case BridgePhase.Building:
                inputAllowed = true;
                timerObject.StartTimer ();
                break;
            case BridgePhase.Falling:
                inputAllowed = false;
                timerObject.StartTimer ();
                break;
            case BridgePhase.Crossing:
                inputAllowed = false;
                timerObject.StopTimer ();
                break;
            case BridgePhase.Lose:
                inputAllowed = false;
                gameStarted = false;
                GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.FailedLevel);
                break;
            case BridgePhase.Finish:
                inputAllowed = false;
                gameStarted = false;
                StartCoroutine (GameOverSequence ());
                break;
        }
    }


    IEnumerator GameOverSequence() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
        if (trappedMonster) trappedMonster.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        SoundManager.GetInstance ().PlayVoiceOverClip (voData.FindVO ("youdidit"));
        timerObject.StopTimer ();
        SoundManager.GetInstance ().PlayCorrectSFX ();
        GameManager.GetInstance ().LevelUp (DataType.Minigame.BoneBridge);
        yield return new WaitForSeconds (3.0f);
        GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.CompletedLevel);
    }

    void CreateMonsters() {
        // Populate monster pool
        foreach (DataType.MonsterType typeOfMonster in System.Enum.GetValues (typeof (DataType.MonsterType))) {
            int count = 0;
            monsterPool.Add (GameManager.GetInstance ().GetMonsterObject (typeOfMonster));
            count++;
        }

        // Create player monster
        monster = currentLevel
            .monsterSpawn.GetComponent<CreateMonster> ()
            .SpawnMonster (GameManager.GetInstance().GetPlayerMonsterType());
        bridgeMonster = monster.transform.parent.gameObject.AddComponent<BoneBridgeMonster> ();
        bridgeMonster.rigBody = monster.transform.parent.gameObject.GetComponent<Rigidbody2D>();
        monsterPool.Remove (GameManager.GetInstance ().GetPlayerMonsterType ());

        Transform[] friendSpawns = currentLevel.friendSpawns;
        // Choose monsters
        for (int i = 0; i < friendSpawns.Length; i++) {
            int randomNumber = Random.Range (0, monsterPool.Count);
            GameObject selectedMonster = monsterPool[randomNumber];
            print (string.Format ("selectedMonster: {0} [{1}] randomNumber: {2} monsterPool.Count: {3}", selectedMonster, i, randomNumber, monsterPool.Count));
            savedMonsters.Add (selectedMonster.GetComponentInChildren<Monster> ().typeOfMonster);
            monsterPool.Remove (selectedMonster);

            CreateMonster monsterCreator = friendSpawns[i].GetComponent<CreateMonster> ();
            monsterCreator.SpawnMonster (selectedMonster);
        }

    }

    void ChangeLevel(BoneBridgeLevel level) {
        levelOne.gameObject.SetActive (false);
        levelTwo.gameObject.SetActive (false);
        levelThree.gameObject.SetActive (false);

        level.gameObject.SetActive (true);
        currentLevel = level;

        start = currentLevel.start;
        goal = currentLevel.goal;
        boneCamera.xMinClamp = currentLevel.cameraXMin;
        boneCamera.xMaxClamp = currentLevel.cameraXMax;
    }

    BoneBridgeLevel GetLevelObject(DataType.Level level) {
        switch (level) {
            case DataType.Level.LevelOne:
                return levelOne;
            case DataType.Level.LevelTwo:
                return levelTwo;
            case DataType.Level.LevelThree:
                return levelThree;
        }
        return levelOne;
    }

    public void CameraSwitch(GameObject obj) {
        boneCamera.target = obj;
    }

    public void ResetMonster (Vector2 pos, GameObject focus) {
        StartCoroutine (ResettingMonster (pos, focus));
    }

    IEnumerator ResettingMonster (Vector2 pos, GameObject focus) {
        yield return new WaitForSeconds (3.0f);
        bridgeMonster.transform.position = pos;
        CameraSwitch (focus);
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        ChangePhase (BridgePhase.Building);
    }

    public void ChangeProgressBar (float value) {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition (value);
    }

    public void CreatePrize () {
        Transform prize = currentLevel.prizeSpawn;

        switch (difficultyLevel) {
            case DataType.Level.LevelOne:
                trappedMonster = prize.GetComponent<CreateMonster> ().SpawnMonster (monsterPool[Random.Range(0,monsterPool.Count)]);
                trappedMonster.ChangeEmotions (DataType.MonsterEmotions.Afraid);
                subtitlePanel.Display ("Rescue the trapped monster!");
                break;
            case DataType.Level.LevelTwo:
                trappedMonster = prize.GetComponent<CreateMonster> ().SpawnMonster (monsterPool[Random.Range (0, monsterPool.Count)]);
                trappedMonster.ChangeEmotions (DataType.MonsterEmotions.Afraid);
                subtitlePanel.Display ("Rescue the trapped monster!");
                break;
            case DataType.Level.LevelThree:
                Instantiate (chest, prize.transform.position, Quaternion.identity, prize.transform.root);
                break;
            default:
                break;
        }
    }
}
