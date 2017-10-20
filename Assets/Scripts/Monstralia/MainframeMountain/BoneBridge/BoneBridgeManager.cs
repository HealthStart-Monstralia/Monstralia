using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* CREATED BY: Colby Tang
 * GAME: Bridge Bones
 */

public class BoneBridgeManager : AbstractGameManager {

    public VoiceOversData voData;
    public bool inputAllowed = false;
    public bool isTutorialRunning = false;
    public ScoreGauge scoreGauge;

    public Text timerText;
    public float timeLimit;
    public float timeLeft;
    public GameObject subtitlePanel;
    public GameObject goal;
    public GameObject[] waypoints;
    public GameObject[] focusPoints;
    public Monster monster;
    public BoxCollider2D leftTransition, rightTransition;

    public BoneBridgeCamera boneCamera;

    private int level = 0;
    private Coroutine tutorialCoroutine;
    private bool gameStarted = false;
    private static BoneBridgeManager instance = null;
    private BridgeMonster bridgeMonster;
    private Vector2 startPos;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        startPos = GetComponent<CreateMonster> ().spawnPosition.transform.position;
    }

    private void Update () {
        ChangeProgressBar (
            (bridgeMonster.transform.position.x - startPos.x) / (goal.transform.position.x - startPos.x)
        );
    }

    public override void PregameSetup () {
        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BoneBridge)) {

        }
        else {

        }

        monster = GetComponent<CreateMonster> ().SpawnMonster ().GetComponentInChildren<Monster> ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        bridgeMonster = monster.transform.parent.gameObject.AddComponent<BridgeMonster> ();
        bridgeMonster.goalObject = waypoints[0];
        bridgeMonster.tapToMove = true;
        monster.GetComponent<BoxCollider2D> ().enabled = false;
        monster.gameObject.AddComponent<CapsuleCollider2D> ();
        GameStart ();
        //StartCoroutine (Countdown ());
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
        inputAllowed = true;
    }

    public override void GameOver () {
        gameStarted = false;
        inputAllowed = false;
    }

    public static BoneBridgeManager GetInstance () {
        return instance;
    }

    public void CameraSwitch(GameObject obj) {
        boneCamera.target = obj;
    }

    public void SwitchGoal (int num) {
        bridgeMonster.Stop ();
        bridgeMonster.goalObject = waypoints[1];
    }

    public void ResetMonster (int num) {
        bridgeMonster.Stop ();
        switch (num) {
            case 0:
                bridgeMonster.gameObject.transform.position = startPos;
                CameraSwitch (focusPoints[0]);
                break;
            case 1:
                bridgeMonster.gameObject.transform.position = waypoints[0].transform.position;
                CameraSwitch (focusPoints[1]);
                break;
        }
    }
}
