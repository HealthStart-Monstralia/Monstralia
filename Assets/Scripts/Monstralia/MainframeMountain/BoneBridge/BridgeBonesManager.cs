using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* CREATED BY: Colby Tang
 * GAME: Bridge Bones
 */

public class BridgeBonesManager : AbstractGameManager {

    public VoiceOversData voData;
    public bool inputAllowed = false;
    public bool isTutorialRunning = false;
    public ScoreGauge scoreGauge;

    public Text timerText;
    public float timeLimit;
    public float timeLeft;
    public GameObject subtitlePanel;
    public GameObject goalObject;
    public Monster monster;

    private int level = 0;
    private Coroutine tutorialCoroutine;
    private bool gameStarted = false;
    private static BridgeBonesManager instance = null;
    private CreateMonster createMonster;


    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        createMonster = GetComponent<CreateMonster> ();
    }

    public override void PregameSetup () {
        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BrainMaze)) {

        }
        else {

        }

        monster = createMonster.SpawnMonster ().GetComponentInChildren<Monster> ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        BridgeMonster bridgeMonster = monster.transform.parent.gameObject.AddComponent<BridgeMonster> ();
        bridgeMonster.goalObject = goalObject;
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

    public void GameStart () {
        gameStarted = true;
        inputAllowed = true;
    }

    public override void GameOver () {
        gameStarted = false;
        inputAllowed = false;
    }

    public static BridgeBonesManager GetInstance () {
        return instance;
    }
}
