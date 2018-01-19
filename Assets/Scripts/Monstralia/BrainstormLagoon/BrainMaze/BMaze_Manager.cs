using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CREATED BY: Colby Tang
 * GAME: Brain Maze
 */

public class BMaze_Manager : AbstractGameManager<BMaze_Manager> {
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

    [SerializeField] private List<BMaze_Pickup> pickupList = new List<BMaze_Pickup> ();
    private GameObject monsterObject;
    private int level = 0;
	private static bool gameStarted = false;
	private Coroutine tutorialCoroutine;
    private BMaze_SceneAssets selectedAsset;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private GameObject tutorialPickup;

    public override void PregameSetup () {
        print ("PregameSetup");
        if (SoundManager.Instance) {
            SoundManager.Instance.ChangeAmbientSound (ambientSound);
            SoundManager.Instance.StopPlayingVoiceOver ();
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

        timerClock = TimerClock.Instance;
        timerClock.SetTimeLimit (timeLimit);
        pickupList.Clear ();
        ResetScore ();
        if (monsterObject)
            RemoveMonster ();

        if (GameManager.Instance.GetPendingTutorial (DataType.Minigame.BrainMaze)) {
            selectedAsset = assetList[0];
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {

            level = GameManager.Instance.GetLevel (DataType.Minigame.BrainMaze);
            selectedAsset = assetList[level];
            selectedAsset.gameObject.SetActive (true);
            pickupList.AddRange (assetList[level].pickups);
            scoreGoal = pickupList.Count;

            CreateMonster ();
            scoreGauge.gameObject.SetActive (true);
            timerClock.gameObject.SetActive (true);
            StartCountdown (GameStart);
        }

    }

	IEnumerator RunTutorial () {
		isTutorialRunning = true;
        pickupList.AddRange (assetList[0].pickups);
        scoreGoal = pickupList.Count;
        selectedAsset.gameObject.SetActive (true);
		print ("RunTutorial");
		scoreGauge.gameObject.SetActive (false);
		timerClock.gameObject.SetActive (false);

        yield return new WaitForSeconds(0.25f);
        subtitlePanel.SetActive (true);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Brain Maze!", null);

		SoundManager.Instance.StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("1_tutorial_start");
        AudioClip tutorial2 = voData.FindVO ("2_tutorial_drag");
        AudioClip tutorial3 = voData.FindVO ("3_tutorial_letmeshow");

        SoundManager.Instance.PlayVoiceOverClip(tutorial1);
		yield return new WaitForSeconds(tutorial1.length);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		CreateMonster ();
        inputAllowed = false;

        SoundManager.Instance.PlayVoiceOverClip (tutorial2);
        yield return new WaitForSeconds (tutorial2.length);

        SoundManager.Instance.PlayVoiceOverClip (tutorial3);
        yield return new WaitForSeconds (tutorial3.length);

        tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("BMaze_HandMoveMonster");
		yield return new WaitForSeconds(1.5f);
		playerMonster.transform.SetParent (tutorialHand.transform);
		yield return new WaitForSeconds(1.5f);
        playerMonster.transform.SetParent (null);
        yield return new WaitForSeconds (1.5f);

        ShowSubtitle ("Now you try!");
        SoundManager.Instance.PlayVoiceOverClip (
            voData.FindVO ("nowyoutry")
            );

        ResetScore ();
        GameObject startingLocation = selectedAsset.GetStartLocation();
        playerMonster.transform.position = GetStartingLocationVector (startingLocation);
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
		GameManager.Instance.CompleteTutorial(DataType.Minigame.BrainMaze);
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
            SoundManager.Instance.AddToVOQueue (youdidit);
            GameEnd ();
        } else {
            TutorialFinished ();
        }
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
			if (GameManager.Instance)
				GameManager.Instance.LevelUp (DataType.Minigame.BrainMaze);
			Instance.ChangeScene ();
		}
		button.SetActive (false);
	}

	Vector3 GetStartingLocationVector(GameObject location) {
		return new Vector3 (
			location.transform.position.x,
			location.transform.position.y,
			0f);
	}

	void CreateMonster() {
        GameObject startingLocation = selectedAsset.GetStartLocation();
        CreatePlayerMonster (startingLocation.transform);
        SetScaleMonster (playerMonster.transform);
        playerMonster.transform.SetParent (startingLocation.transform.root);
        playerMonster.transform.gameObject.AddComponent<BMaze_Monster> ();
        playerMonster.transform.gameObject.AddComponent<BMaze_MonsterMovement> ();
    }

	void SetScaleMonster(Transform monsterTransform) {
        monsterTransform.localScale = new Vector3(monsterScale[level], monsterScale[level], monsterScale[level]);
        if (monsterTransform.GetComponent<Collider2D>()) {
            monsterTransform.GetComponent<Collider2D> ().enabled = false;
        }
        monsterTransform.gameObject.AddComponent<CircleCollider2D> ().radius = (monsterScale[level] * 4);
        //monsterTransform.GetComponent<CircleCollider2D> ().radius = (monsterScale[level] * 4);
	}

	public GameObject GetMonster() {
		return monsterObject;
	}

	public void RemoveMonster() {
		Destroy(monsterObject);
	}

	public void UnlockDoor () {
        AudioClip unlockeddoor = voData.FindVO ("unlockeddoor");
        SoundManager.Instance.AddToVOQueue (unlockeddoor);

        selectedAsset.GetDoor ().OpenDoor ();
		selectedAsset.GetFinishline ().UnlockFinishline ();
	}

	public void ShowSubtitle(string text) {
		subtitlePanel.GetComponent<SubtitlePanel> ().Display (text, null);
	}

}