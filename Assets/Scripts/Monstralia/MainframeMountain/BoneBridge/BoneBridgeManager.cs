using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* CREATED BY: Colby Tang
 * GAME: Bone Bridge
 */

public class BoneBridgeManager : AbstractGameManager {
    public enum BridgePhase {
        Start,
        Building,
        Falling,
        Crossing,
        Finish
    };

    [Header ("BoneBridgeManager Fields")]
    public BridgePhase bridgePhase;
    public int bridgeSection;



    public VoiceOversData voData;
    public bool doCountdown, playIntro;
    public bool inputAllowed = false;
    public bool isTutorialRunning = false;
    public ScoreGauge scoreGauge;

    public Text timerText;
    public float timeLimit;
    public float timeLeft;
    public GameObject subtitlePanel;
    public GameObject goal;
    public Monster monster;
    public BoneBridgeCamera boneCamera;
    [HideInInspector] public BoneBridgeMonster bridgeMonster;

    // Events
    public delegate void PhaseChangeAction (BridgePhase phase);
    public static event PhaseChangeAction PhaseChange;

    private int difficultyLevel = 0;
    private Coroutine tutorialCoroutine;
    private bool gameStarted = false;
    private static BoneBridgeManager instance = null;
    private Vector2 startPos;

    private Rigidbody2D rigBody;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        startPos = GetComponent<CreateMonster> ().spawnPosition.transform.position;
        //difficultyLevel = GameManager.GetInstance ().GetLevel (typeOfGame);
    }

    public static BoneBridgeManager GetInstance () {
        return instance;
    }

    private void Update () {
        if (bridgeMonster) {
            ChangeProgressBar (
                (bridgeMonster.transform.position.x - startPos.x) / (goal.transform.position.x - startPos.x)
            );
        }
    }

    private void OnEnable () {
        PhaseChange += OnPhaseChange;
    }

    private void OnDisable () {
        PhaseChange -= OnPhaseChange;
    }

    public override void PregameSetup () {

        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BoneBridge)) {

        }
        else {

        }

        CameraSwitch (GetComponent<CreateMonster>().spawnPosition.gameObject);
        OnPhaseChange (BridgePhase.Start);
        StartCoroutine (Intro ());
    }

    IEnumerator Intro () {
        yield return new WaitForSeconds (1.0f);
        CreateMonster ();
        if (playIntro) {
            monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
            yield return new WaitForSeconds (1.0f);
            float initialDampTime = boneCamera.dampTime;
            boneCamera.dampTime = 1.5f;
            CameraSwitch (goal);
            yield return new WaitForSeconds (6.0f);
            boneCamera.dampTime = initialDampTime;
            CameraSwitch (bridgeMonster.gameObject);
            yield return new WaitForSeconds (1.0f);
            monster.ChangeEmotions (DataType.MonsterEmotions.Thoughtful);
            yield return new WaitForSeconds (1.0f);
        }
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        bridgeMonster.StartCoroutine (bridgeMonster.Move ());
    }

    IEnumerator Countdown () {
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().Countdown ();
        scoreGauge.gameObject.SetActive (true);
        timerText.transform.parent.gameObject.SetActive (true);
        yield return new WaitForSeconds (4.0f);
        doCountdown = false;
        GameStart ();
    }

    public void GameStart () {
        if (doCountdown)
            StartCoroutine (Countdown ());
        else {
            gameStarted = true;
            ChangePhase (BridgePhase.Building);
        }
    }

    public bool GetGameStarted () { return gameStarted; }

    public override void GameOver () {
        ChangePhase(BridgePhase.Finish);
    }

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
            case BridgePhase.Building:
                inputAllowed = true;
                break;
            case BridgePhase.Falling:
                inputAllowed = false;

                break;
            case BridgePhase.Crossing:
                inputAllowed = false;
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
        SoundManager.GetInstance ().PlayCorrectSFX ();
        yield return new WaitForSeconds (3.0f);
        GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.CompletedLevel);
    }

    void CreateMonster() {
        monster = GetComponent<CreateMonster> ().SpawnMonster ();

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
}
