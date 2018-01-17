using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CREATED BY: Colby Tang
 * GAME: Brain Maze
 */

public class BMaze_Manager : AbstractGameManager {

	public GameObject[] monsterList = new GameObject[4];
	[Range(0.1f,2.0f)]
	public float[] monsterScale;
    public VoiceOversData voData;
    public bool inputAllowed = false;

	public ScoreGauge scoreGauge;
    public int score;
    public int scoreGoal = 1;

    [HideInInspector] public TimerClock timerClock;
	public float timeLimit;
	public BMaze_SceneAssets[] assetList;
    public GameObject subtitlePanel;

	public GameObject backButton;
	public GameObject tutorialHand;
    [HideInInspector] public bool isTutorialRunning = false;

    private List<GameObject> pickupList = new List<GameObject> ();
    private GameObject monsterObject;
    private int level = 0;
    private GameObject skipButton;
	private static bool gameStarted = false;
	private static BMaze_Manager instance = null;
	private Coroutine tutorialCoroutine;
    private BMaze_SceneAssets selectedAsset;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private GameObject tutorialPickup;

    void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

        CheckForGameManager ();

		if (SoundManager.GetInstance ()) {
			SoundManager.GetInstance ().ChangeAmbientSound (ambientSound);
			SoundManager.GetInstance ().StopPlayingVoiceOver ();
		}

		backButton.SetActive (true);
		subtitlePanel.SetActive (false);
		tutorialHand.SetActive (false);

        for (int i = 0; i < 3; i++) {
            assetList[i].gameObject.SetActive (false);
        }

        // To avoid division by zero
        if (scoreGoal <= 0) scoreGoal = 1;

		UpdateScoreGauge ();
		typeOfMonster = GameManager.GetInstance().GetMonsterType ();
	}

    public static BMaze_Manager GetInstance () {
        return instance;
    }

    public override void PregameSetup () {
        timerClock = TimerClock.GetInstance ();
        timerClock.SetTimeLimit (timeLimit);
        pickupList.Clear ();
        ResetScore ();
        if (monsterObject)
            RemoveMonster ();

        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BrainMaze)) {
            selectedAsset = assetList[0];
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {

            level = GameManager.GetInstance ().GetLevel (DataType.Minigame.BrainMaze);
            selectedAsset = assetList[level];
            selectedAsset.gameObject.SetActive (true);

            monsterObject = CreateMonster ();
            StartCoroutine (Countdown ());
        }

    }

	IEnumerator RunTutorial () {
		isTutorialRunning = true;
        selectedAsset.gameObject.SetActive (true);
		print ("RunTutorial");
		scoreGauge.gameObject.SetActive (false);
		timerClock.gameObject.SetActive (false);

        yield return new WaitForSeconds(0.25f);
        pickupList[0].GetComponent<BMaze_Pickup> ();
        pickupList[1].GetComponent<BMaze_Pickup> ();

        subtitlePanel.SetActive (true);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Brain Maze!", null);

		SoundManager.GetInstance().StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("1_tutorial_start");
        AudioClip tutorial2 = voData.FindVO ("2_tutorial_drag");
        AudioClip tutorial3 = voData.FindVO ("3_tutorial_letmeshow");

        SoundManager.GetInstance().PlayVoiceOverClip(tutorial1);
		yield return new WaitForSeconds(tutorial1.length);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		monsterObject = CreateMonster ();
        inputAllowed = false;

        SoundManager.GetInstance ().PlayVoiceOverClip (tutorial2);
        yield return new WaitForSeconds (tutorial2.length);

        SoundManager.GetInstance ().PlayVoiceOverClip (tutorial3);
        yield return new WaitForSeconds (tutorial3.length);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("BMaze_HandMoveMonster");
		yield return new WaitForSeconds(1.5f);
		monsterObject.transform.SetParent (tutorialHand.transform);
		yield return new WaitForSeconds(1.5f);
		monsterObject.transform.SetParent (null);
        yield return new WaitForSeconds (1.5f);

        ShowSubtitle ("Now you try!");
        SoundManager.GetInstance ().PlayVoiceOverClip (
            voData.FindVO ("nowyoutry")
            );

        ResetScore ();
        GameObject startingLocation = selectedAsset.GetStartLocation();
		monsterObject.transform.position = GetStartingLocationVector (startingLocation);
        tutorialPickup.SetActive (true);
        inputAllowed = true;

        subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		yield return new WaitForSeconds(2f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Get all the pickups!", null);
		yield return new WaitForSeconds(5f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
	}

	public void TutorialFinished() {
		inputAllowed = false;
		tutorialHand.SetActive (false);
		GameManager.GetInstance ().CompleteTutorial(DataType.Minigame.BrainMaze);
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

	IEnumerator TutorialTearDown() {
		print ("TutorialTearDown");
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Let's play!", null);
		yield return new WaitForSeconds(2.0f);
		isTutorialRunning = false;
		yield return new WaitForSeconds(1.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		selectedAsset.gameObject.SetActive (false);
        PregameSetup ();
	}

    public void OnScore(GameObject obj) {
        score++;
        UpdateScoreGauge ();
        if (score >= scoreGoal)
            UnlockDoor ();
    }

    public void UpdateScoreGauge () {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)score / scoreGoal);
    }

    public void ResetScore () {
        score = 0;
        UpdateScoreGauge ();
    }

    public void OnOutOfTime() {
        GameEnd ();
    }

    public void OnFinish() {
        if (!isTutorialRunning) {
            AudioClip youdidit = voData.FindVO ("youdidit");
            SoundManager.GetInstance ().AddToVOQueue (youdidit);
            GameEnd ();
        } else {
            TutorialFinished ();
        }
    }

    IEnumerator Countdown() {
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().StartCountdown ();
        scoreGauge.gameObject.SetActive (true);
		timerClock.gameObject.SetActive (true);
		yield return new WaitForSeconds (4.0f);
		GameStart ();
	}

	public void GameStart () {
		gameStarted = true;
        inputAllowed = true;
        timerClock.StartTimer ();
	}

    public void GameEnd() {
        gameStarted = false;
        inputAllowed = false;
        timerClock.StopTimer ();
        StartCoroutine (EndGameWait (3f));
    }

    public IEnumerator EndGameWait (float duration) {
        yield return new WaitForSeconds (duration);
        if (!isTutorialRunning && !gameStarted) {
            if (score >= scoreGoal) {
                if (level == 1) {
                    GameOver (DataType.GameEnd.EarnedSticker);
                } else {
                    GameOver (DataType.GameEnd.CompletedLevel);
                }
            } else {
                GameOver (DataType.GameEnd.FailedLevel);
            }
        }
    }

	public void ChangeScene () {
		GetComponent<SwitchScene> ().LoadScene ();
	}

	public void SkipLevel (GameObject button) {
		if (isTutorialRunning) {
			TutorialFinished ();
		} else {
			gameStarted = false;
			if (GameManager.GetInstance())
				GameManager.GetInstance ().LevelUp (DataType.Minigame.BrainMaze);
			instance.ChangeScene ();
		}
		skipButton = button;
		button.SetActive (false);
	}

	Vector3 GetStartingLocationVector(GameObject location) {
		return new Vector3 (
			location.transform.position.x,
			location.transform.position.y,
			0f);
	}

	GameObject CreateMonster() {
        GameObject startingLocation = selectedAsset.GetStartLocation();

		Vector3 monsterPosition = GetStartingLocationVector (startingLocation);
		
		Quaternion monsterRotation = startingLocation.transform.rotation;

		switch (typeOfMonster) {
		    case DataType.MonsterType.Blue:
			    monsterObject = Instantiate (
				    monsterList [0], 
				    monsterPosition,
				    monsterRotation) as GameObject;
			    ScaleMonster ();
                monsterObject.GetComponent<BMaze_Monster> ().PlaySpawn ();
                return monsterObject;
		    case DataType.MonsterType.Green:
			    monsterObject = Instantiate (
				    monsterList [1], 
				    monsterPosition,
				    monsterRotation) as GameObject;
			    ScaleMonster ();
                monsterObject.GetComponent<BMaze_Monster> ().PlaySpawn ();
                return monsterObject;
                case DataType.MonsterType.Red:
		    monsterObject = Instantiate (
				    monsterList [2], 
				    monsterPosition,
				    monsterRotation) as GameObject;
			    ScaleMonster ();
                monsterObject.GetComponent<BMaze_Monster> ().PlaySpawn ();
                return monsterObject;
		    case DataType.MonsterType.Yellow:
			    monsterObject = Instantiate (
				    monsterList [3], 
				    monsterPosition,
				    monsterRotation) as GameObject;
			    ScaleMonster ();
                monsterObject.GetComponent<BMaze_Monster> ().PlaySpawn ();
                return monsterObject;
            default:
                return null;
		}
	}

	void ScaleMonster() {
		monsterObject.transform.localScale = new Vector3(monsterScale[level], monsterScale[level], monsterScale[level]);
		monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale[level] * 4);
	}

	public GameObject GetMonster() {
		return monsterObject;
	}

	public void RemoveMonster() {
		Destroy(monsterObject);
	}

	public void UnlockDoor () {
        AudioClip unlockeddoor = voData.FindVO ("unlockeddoor");
        SoundManager.GetInstance ().AddToVOQueue (unlockeddoor);

        selectedAsset.GetDoor ().OpenDoor ();
		selectedAsset.GetFinishline ().UnlockFinishline ();
	}

	public void ShowSubtitle(string text) {
		subtitlePanel.GetComponent<SubtitlePanel> ().Display (text, null);
	}

    public void AddPickupToList (GameObject obj) {
        pickupList.Add (obj);
        scoreGoal = pickupList.Count;
    }

}