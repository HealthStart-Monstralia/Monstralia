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
        Lose,
        Finish
    };

    [Header ("BoneBridgeManager Fields")]
    public BridgePhase bridgePhase;

    public VoiceOversData voData;
    public bool doCountdown, playIntro;
    public bool inputAllowed = false;
    public bool isTutorialRunning = false;
    public ScoreGauge scoreGauge;

    public Timer timerObject;
    public GameObject subtitlePanel;
    public GameObject start, goal;
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

    private Rigidbody2D rigBody;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        scoreGauge.gameObject.SetActive (false);
        timerObject.gameObject.SetActive (false);
    }

    public static BoneBridgeManager GetInstance () {
        return instance;
    }

    private void Update () {
        if (bridgeMonster) {
            ChangeProgressBar (
                (bridgeMonster.transform.position.x - start.transform.position.x) / (goal.transform.position.x - start.transform.position.x)
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
        difficultyLevel = GameManager.GetInstance ().GetLevel (typeOfGame);
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
                print ("Lost");
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

    void OnOutOfTime () {
        ChangePhase (BridgePhase.Lose);
    }

    IEnumerator GameOverSequence() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
        SoundManager.GetInstance ().PlayVoiceOverClip (voData.FindVO ("youdidit"));
        timerObject.StopTimer ();
        SoundManager.GetInstance ().PlayCorrectSFX ();
        GameManager.GetInstance ().LevelUp (DataType.Minigame.BoneBridge);
        yield return new WaitForSeconds (3.0f);
        GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.CompletedLevel);
    }

    void CreateMonster() {
        monster = GetComponent<CreateMonster> ().SpawnMonster ();
        bridgeMonster = monster.transform.parent.gameObject.AddComponent<BoneBridgeMonster> ();
        bridgeMonster.rigBody = monster.transform.parent.gameObject.GetComponent<Rigidbody2D>();
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
