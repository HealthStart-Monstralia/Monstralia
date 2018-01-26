using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* CREATED BY: Colby Tang
 * GAME: Brain Maze
 */

public class BMazeManager : AbstractGameManager<BMazeManager> {
    [Header ("Brain Maze Fields")]
    public VoiceOversData voData;
    [HideInInspector] public bool inputAllowed = false;

	public ScoreGauge scoreGauge;
    public int score;
    public int scoreGoal = 1;

    [HideInInspector] public TimerClock timerClock;
	public float timeLimit;
	public BMazeLevel[] assetList;
    public GameObject subtitlePanel;

	public GameObject backButton;
	public GameObject tutorialHand;
    [HideInInspector] public bool isTutorialRunning = false;

    [SerializeField] private List<BMazePickup> pickupList = new List<BMazePickup> ();
    private int level = 0;
	private static bool gameStarted = false;
	private Coroutine tutorialCoroutine;
    private BMazeLevel selectedAsset;
    private Animator monsterAnimator;

    [Header ("References")]
    [SerializeField] private BMazeFactory factory;
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
        ResetScore ();
        if (playerMonster)
            RemoveMonster ();

        if (GameManager.Instance.GetPendingTutorial (DataType.Minigame.BrainMaze)) {
            selectedAsset = assetList[0];
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {
            level = GameManager.Instance.GetLevel (DataType.Minigame.BrainMaze);
            selectedAsset = assetList[level];
            selectedAsset.gameObject.SetActive (true);

            CreateMonster ();
            scoreGauge.gameObject.SetActive (true);
            timerClock.gameObject.SetActive (true);
            StartCoroutine (WaitBeforeStarting (2f));
        }

    }

    IEnumerator WaitBeforeStarting (float waitTime) {
        yield return new WaitForSeconds (waitTime);
        scoreGoal = pickupList.Count;
        StartCountdown (GameStart);
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
        Transform monsterParent = playerMonster.transform.parent;
		yield return new WaitForSeconds(1.5f);
		playerMonster.transform.SetParent (tutorialHand.transform);
		yield return new WaitForSeconds(1.5f);
        playerMonster.transform.SetParent (monsterParent);
        yield return new WaitForSeconds (1.5f);

        ShowSubtitle ("Now you try!");
        SoundManager.Instance.PlayVoiceOverClip (
            voData.FindVO ("nowyoutry")
            );

        ResetScore ();
        Transform startingLocation = selectedAsset.startingLocation;
        playerMonster.transform.position = startingLocation.transform.position;
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
        pickupList.Clear ();
        PregameSetup ();
	}

    public void OnScore(BMazePickup obj) {
        if (pickupList.Contains (obj)) {
            pickupList.Remove (obj);
        }

        score++;
        UpdateScoreGauge ();
        if (score >= scoreGoal)
            selectedAsset.UnlockDoor ();
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
        Transform startingLocation = selectedAsset.startingLocation;
        CreatePlayerMonster (startingLocation);
        SetScaleMonster (playerMonster.transform);
        playerMonster.transform.SetParent (startingLocation.transform.root);
        playerMonster.transform.gameObject.AddComponent<BMazeMonsterMovement> ();
        monsterAnimator = playerMonster.GetComponentInChildren<Animator> ();
        PlaySpawn ();
    }

	void SetScaleMonster(Transform monsterTransform) {
        monsterTransform.localScale = Vector3.one * selectedAsset.monsterScale;
        Collider2D monsterCollider = monsterTransform.GetComponent<Collider2D> ();
        if (monsterCollider) {
            monsterCollider.enabled = false;
        }

        monsterCollider = monsterTransform.GetComponentInChildren<Collider2D> ();
        if (monsterCollider) {
            monsterCollider.enabled = false;
        }

        monsterTransform.gameObject.AddComponent<CircleCollider2D> ().radius = (selectedAsset.monsterScale * 5);
        //monsterTransform.GetComponent<CircleCollider2D> ().radius = (monsterScale[level] * 4);
	}

	public GameObject GetMonsterObject() {
		return playerMonster.gameObject;
	}

	public void RemoveMonster() {
        if (playerMonster)
		    Destroy(playerMonster);
	}

	public void ShowSubtitle(string text) {
		subtitlePanel.GetComponent<SubtitlePanel> ().Display (text, null);
	}

    public List<GameObject> GetFactoryList () {
        return factory.pickupPrefabList;
    }

    public GameObject SpawnPickup (GameObject prefab, Transform parent) {
        GameObject pickup = factory.Manufacture (prefab, parent);
        pickupList.Add (pickup.GetComponent<BMazePickup> ());

        return pickup;
    }

    public void SetFactoryScale (float scale) {
        factory.scale = Vector3.one * scale;
    }

    public void PlaySpawn () {
        monsterAnimator.StopPlayback ();
        monsterAnimator.Play ("BMaze_Spawn", -1, 0f);
    }

    public void PlayDance () {
        monsterAnimator.StopPlayback ();
        monsterAnimator.Play ("BMaze_Dance", -1, 0f);
    }
}