﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesGameManager : AbstractGameManager<SensesGameManager> {
    [Header ("Senses Game Manager Fields")]

    public bool skipTutorial = false;
    public VoiceOversData voData;
    public SensesLevelManager levelOne, levelTwo, levelThree;
    public SensesFireworksSystem fireworksSystem;
    [HideInInspector] public bool hasGameStarted;
    [HideInInspector] public int score;
    public delegate void GameStart ();
    public delegate void GameEnd ();
    public static event GameStart OnGameStartEvent;
    public static event GameStart OnGameEndEvent;

    [Header ("Audio Clips")]
    [Tooltip ("Drag and drop the appropriate audio files to the appropriate function.")]
    [SerializeField] private AudioClip introChime;
    [SerializeField] private AudioClip correctSfx;
    [SerializeField] private AudioClip finishedSfx;

    [Header ("Voiceovers")]
    public AudioClip[] rightChoiceVO;
    public AudioClip[] wrongChoiceVO;

    private DataType.Level difficultyLevel;

    private SensesLevelManager currentLevelManager;
    private bool isInputAllowed;

    [Header ("References")]
    [SerializeField] private GameObject introCanvas;
    [SerializeField] private ScoreGauge scoreGauge;
    [SerializeField] private TimerClock timerClock;
    [SerializeField] private GameObject sensePanel;

    public bool IsInputAllowed {
        get {
            return isInputAllowed;
        }

        set {
            isInputAllowed = value;
            ActivateSenseButtons (value);
        }
    }

    new void Awake () {
        base.Awake ();
        levelOne.gameObject.SetActive (false);
        levelTwo.gameObject.SetActive (false);
        levelThree.gameObject.SetActive (false);
    }

    public override void PregameSetup () {
        ActivateHUD (false);
        difficultyLevel = (DataType.Level)GameManager.Instance.GetLevel (DataType.Minigame.MonsterSenses);
        currentLevelManager = GetLevelConfig ();
        currentLevelManager.gameObject.SetActive (true);
    }

    public void ActivateHUD(bool activate) {
        scoreGauge.gameObject.SetActive (activate);
        timerClock.gameObject.SetActive (activate);
        sensePanel.SetActive (activate);
        if (activate) {
            UpdateScoreGauge ();
            SetTimeLimit (GetLevelConfig ().timeLimit);
        }
    }

    public void StartLevel() {
        introCanvas.SetActive (false);
        SoundManager.Instance.PlaySFXClip (introChime);
        GetLevelConfig ().SetupGame ();
    }

    public void OnGameStart() {
        hasGameStarted = true;
        IsInputAllowed = true;
        timerClock.StartTimer ();
        if (OnGameStartEvent != null)
            OnGameStartEvent ();
    }

    public void OnGameEnd () {
        hasGameStarted = false;
        IsInputAllowed = false;
        timerClock.StopTimer ();
        if (OnGameEndEvent != null)
            OnGameEndEvent ();
        StartCoroutine (GameOverSequence ());
    }

    public void OnScore() {
        StartCoroutine (OnGuess (true));
        SoundManager.Instance.PlaySFXClip (correctSfx);
        score++;
        AddTime (5f);
        UpdateScoreGauge ();
        if (score >= currentLevelManager.scoreGoal) {
            currentLevelManager.EndGame ();
        }
    }

    public void OnOutOfTime() {
        OnGameEnd ();
    }

    public void OnWrongScore () {
        StartCoroutine (OnGuess (false));
        SoundManager.Instance.PlayIncorrectSFX ();
    }

    public void OnSense (DataType.Senses sense) {
        if (isInputAllowed) {
            if (currentLevelManager.IsSenseCorrect (sense)) OnScore ();
            else OnWrongScore ();
        }
    }

    IEnumerator OnGuess(bool isCorrect) {
        IsInputAllowed = false;
        if (isCorrect)
            timerClock.StopTimer ();
        yield return new WaitForSeconds (2f);
        if (hasGameStarted) {
            IsInputAllowed = true;
            if (isCorrect) {
                currentLevelManager.NextQuestion ();
                timerClock.StartTimer ();
            }
            currentLevelManager.monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        }
    }

    IEnumerator GameOverSequence () {
        yield return new WaitForSeconds (1f);
        if (score >= currentLevelManager.scoreGoal) {
            SoundManager.Instance.PlaySFXClip (finishedSfx);
            yield return new WaitForSeconds (2f);
            if (!GameManager.Instance.GetIsStickerUnlocked(typeOfGame)) {
                GameOver (DataType.GameEnd.EarnedSticker);
            } else {
                GameOver (DataType.GameEnd.CompletedLevel);
            }
        } else {
            yield return new WaitForSeconds (3f);
            GameOver (DataType.GameEnd.FailedLevel);
        }
    }

    void UpdateScoreGauge () {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / GetLevelConfig().scoreGoal);
    }

    void ActivateSenseButtons(bool activate) {
        if (sensePanel.activeSelf) {
            Button[] childrenButtons = sensePanel.GetComponentsInChildren<Button> ();
            foreach (Button button in childrenButtons) {
                button.interactable = activate;
            }
        }
    }

    SensesLevelManager GetLevelConfig () {
        switch (difficultyLevel) {
            case DataType.Level.LevelOne:
                return levelOne;
            case DataType.Level.LevelTwo:
                return levelTwo;
            case DataType.Level.LevelThree:
                return levelThree;
            default:
                return levelOne;
        }
    }


}
