using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CatchToxinsManager : AbstractGameManager<CatchToxinsManager> {
    [System.Serializable]
    public struct LevelConfig {
        public float minimumSpawnDelay, maximumSpawnDelay;
        public int scoreGoal;
        public float timeLimit;
        public GameObject[] spawnArray;
    }

    [Header ("CatchToxins Fields")]
    public LevelConfig levelOne;
    public LevelConfig levelTwo;
    public LevelConfig levelThree;
    public AudioClip congratulationsSFX;
    public bool isInputAllowed = false;

    public delegate void GameAction ();
    public static event GameAction OnGameStart, OnGameEnd;

    private int score;
    private int scoreGoal;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private Spawner spawner;
    [SerializeField] private WhiteBloodCell whiteCell;
    private bool hasGameStarted = false;

    public override void PregameSetup () {
        spawner.SetSpawnSettings (GetConfig ());
        scoreGoal = GetConfig ().scoreGoal;
        UICanvas.SetActive (true);
        UpdateScoreGauge ();
        TimerClock.Instance.SetTimeLimit (GetConfig ().timeLimit);
        UICanvas.SetActive (false);
        whiteCell.Score = OnScore;
        StartGame ();
    }

    IEnumerator DelayBeforeStart (float delay) {
        yield return new WaitForSeconds (delay);
        UICanvas.SetActive (true);
        StartCountdown (StartGame);
    }

    public void StartGame () {
        UICanvas.SetActive (true);
        hasGameStarted = true;
        isInputAllowed = true;
        TimerClock.Instance.StartTimer ();
        OnGameStart ();
    }

    public void EndGame () {
		if (hasGameStarted) {
            TimerClock.Instance.StopTimer ();
            hasGameStarted = false;
            isInputAllowed = false;
            OnGameEnd ();
            Invoke ("CompleteLevel", 3f);
        }
	}

    public void OnOutOfTime () {
        EndGame ();
    }

	public void CompleteLevel () {
        if (HasPlayerWon()) {
            GameOver (DataType.GameEnd.CompletedLevel);
            SoundManager.Instance.PlaySFXClip (congratulationsSFX);
        }
        else {
            GameOver (DataType.GameEnd.FailedLevel);
        }
    }

    private LevelConfig GetConfig () {
        switch (GameManager.Instance.GetLevel (typeOfGame)) {
            case 1: return levelOne;
            case 2: return levelTwo;
            case 3: return levelThree;
            default: return levelOne;
        }
    }

    public void OnScore () {
        score++;
        UpdateScoreGauge ();
        SoundManager.Instance.PlayCorrectSFX ();
        if (HasPlayerWon()) {
            EndGame ();
        }
    }

    private bool HasPlayerWon () {
        return score >= scoreGoal;
    }

    void UpdateScoreGauge () {
        if (ScoreGauge.Instance.gameObject.activeSelf)
            ScoreGauge.Instance.SetProgressTransition ((float)score / scoreGoal);
    }
}
