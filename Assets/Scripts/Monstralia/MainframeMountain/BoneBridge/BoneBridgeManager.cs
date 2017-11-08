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
        Crossing,
        Finish
    };

    [Header ("BoneBridgeManager Fields")]
    public BridgePhase bridgePhase;
    public int bridgeSection;

    [Range (0.1f, 5f)]
    public float monsterMass = 1.5f;

    public VoiceOversData voData;
    public bool doCountdown;
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

    // Events
    public delegate void PhaseChangeAction (BridgePhase phase);
    public static event PhaseChangeAction PhaseChange;

    private int difficultyLevel = 0;
    private Coroutine tutorialCoroutine;
    private bool gameStarted = false;
    private static BoneBridgeManager instance = null;
    private BoneBridgeMonster bridgeMonster;
    private Vector2 startPos;

    private Rigidbody2D rigBody;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        startPos = GetComponent<CreateMonster> ().spawnPosition.transform.position;
        ChangePhase(BridgePhase.Start);
        //difficultyLevel = GameManager.GetInstance ().GetLevel (typeOfGame);
    }

    public static BoneBridgeManager GetInstance () {
        return instance;
    }

    private void Update () {
        ChangeProgressBar (
            (bridgeMonster.transform.position.x - startPos.x) / (goal.transform.position.x - startPos.x)
        );
        if (rigBody)
            if (rigBody.mass != monsterMass) rigBody.mass = monsterMass;
    }

    public override void PregameSetup () {

        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BoneBridge)) {

        }
        else {

        }

        monster = GetComponent<CreateMonster> ().SpawnMonster ().GetComponentInChildren<Monster> ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        bridgeMonster = monster.transform.parent.gameObject.AddComponent<BoneBridgeMonster> ();
        bridgeMonster.tapToMove = true;
        rigBody = bridgeMonster.GetComponent<Rigidbody2D> ();
        rigBody.mass = monsterMass;
        monster.GetComponent<BoxCollider2D> ().enabled = false;
        monster.gameObject.AddComponent<CapsuleCollider2D> ();
        if (doCountdown)
            StartCoroutine (Countdown ());
        else
            GameStart ();
    }

    IEnumerator Countdown () {
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().Countdown ();
        scoreGauge.gameObject.SetActive (true);
        timerText.transform.parent.gameObject.SetActive (true);
        yield return new WaitForSeconds (4.0f);
        GameStart ();
    }

    public void ChangeProgressBar (float value) {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition (value);
    }

    public void GameStart () {
        gameStarted = true;
        ChangePhase(BridgePhase.Building);
    }

    public override void GameOver () {
        gameStarted = false;
        ChangePhase(BridgePhase.Finish);
    }

    public void ChangePhase (BridgePhase phase) {
        bridgePhase = phase;
        print ("Manager ChangePhase firing");
        PhaseChange (phase);
    }

    void OnPhaseChange (BridgePhase phase) {
        switch (phase) {
            case BridgePhase.Start:
                inputAllowed = false;
                break;
            case BridgePhase.Building:
                inputAllowed = true;
                break;
            case BridgePhase.Crossing:
                inputAllowed = true;
                break;
            case BridgePhase.Finish:
                inputAllowed = false;
                break;
        }
    }

    public void CameraSwitch(GameObject obj) {
        boneCamera.target = obj;
    }

    public void ChangeWaypoint(GameObject waypoint) {
        bridgeMonster.StopAllCoroutines ();
        bridgeMonster.goalObject = waypoint;
    }

    public void ResetMonster (Vector2 pos) {
        bridgeMonster.gameObject.transform.position = pos;
    }
}
