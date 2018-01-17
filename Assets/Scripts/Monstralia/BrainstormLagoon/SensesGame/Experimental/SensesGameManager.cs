using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesGameManager : AbstractGameManager {
    [Header ("Senses Game Manager Fields")]
    public bool skipTutorial = false;
    public VoiceOversData voData;
    public SensesLevelManager levelOne, levelTwo, levelThree;
    public SensesFireworks fireworksSystem;
    [HideInInspector] public bool hasGameStarted;
    [HideInInspector] public int score;

    [Header ("Audio Clips")]
    [Tooltip ("Drag and drop the appropriate audio files to the appropriate function.")]
    [SerializeField] private AudioClip introChime;
    [SerializeField] private AudioClip correctSfx;

    [Header ("Voiceovers")]
    public AudioClip[] rightChoiceVO;
    public AudioClip[] wrongChoiceVO;

    private DataType.Level difficultyLevel;
    private static SensesGameManager instance;
    private SensesLevelManager currentLevelManager;
    private bool isInputAllowed;

    [Header ("References")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject welcomeObject;
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

    private void Awake () {
        // Apply singleton property
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public static SensesGameManager GetInstance() {
        return instance;
    }

    public override void PregameSetup () {
        ActivateHUD (false);
        difficultyLevel = (DataType.Level)GameManager.GetInstance ().GetLevel (DataType.Minigame.Brainbow);
        currentLevelManager = GetLevelConfig ();
    }

    public void ActivateHUD(bool activate) {
        scoreGauge.gameObject.SetActive (activate);
        timerClock.gameObject.SetActive (activate);
        sensePanel.SetActive (activate);
        if (activate) {
            UpdateScoreGauge ();
            timerClock.SetTimeLimit (GetLevelConfig ().timeLimit);
        }
    }

    public void StartLevel() {
        welcomeObject.SetActive (false);
        playButton.SetActive (false);
        SoundManager.GetInstance ().PlaySFXClip (introChime);
        GetLevelConfig ().SetupGame ();
    }

    public void OnGameStart() {
        hasGameStarted = true;
        IsInputAllowed = true;
    }

    public void OnGameEnd () {
        hasGameStarted = false;
        IsInputAllowed = false;
        StartCoroutine (GameOverSequence ());
    }

    public void OnScore() {
        StartCoroutine (OnGuess (true));
        SoundManager.GetInstance ().PlaySFXClip (correctSfx);
        score++;
        UpdateScoreGauge ();
        if (score >= currentLevelManager.scoreGoal) {
            currentLevelManager.EndGame ();
        }
    }

    public void OnWrongScore () {
        StartCoroutine (OnGuess (false));
        SoundManager.GetInstance ().PlayIncorrectSFX ();
    }

    public void OnSense (DataType.Senses sense) {
        if (isInputAllowed) {
            if (currentLevelManager.IsSenseCorrect (sense)) OnScore ();
            else OnWrongScore ();
        }
    }

    IEnumerator OnGuess(bool isCorrect) {
        IsInputAllowed = false;
        yield return new WaitForSeconds (2f);
        if (hasGameStarted) {
            IsInputAllowed = true;
            if (isCorrect) {
                currentLevelManager.NextQuestion ();
            }
        }
    }

    IEnumerator GameOverSequence () {
        yield return new WaitForSeconds (3f);
        if (score >= currentLevelManager.scoreGoal) {
            if (difficultyLevel == DataType.Level.LevelOne) {
                GameOver (DataType.GameEnd.EarnedSticker);
            } else {
                GameOver (DataType.GameEnd.CompletedLevel);
            }
        } else {
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
