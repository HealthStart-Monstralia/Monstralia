using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BMaze_Manager : AbstractGameManager {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameObject[] monsterList = new GameObject[4];
	[Range(0.1f,1.0f)]
	public float[] monsterScale;
    public VoiceOversData voData;
    public bool inputAllowed = false;
    public static GameObject monsterObject;

	public AudioClip backgroundMusic;
	public Slider scoreSlider;
	public Text timerText;
	public GameObject timer;
	public float timeLimit;
	public static float timeLeft;
	public static int level = 0;
	public GameObject beachBackgroundObj, mazeGraphicObj;
	public GameObject[] mazeColliders;
	public BMaze_SceneAssets[] assetList;

	public GameObject subtitlePanel;

	public GameObject gameOverCanvas;
	public GameObject backButton;
	public BMaze_SceneAssets tutorialAssets;
	public GameObject tutorialHand;
	public GameObject tutorialPickup;
	public static bool isTutorialRunning = false;

	private GameObject skipButton;
	private static bool gameStarted = false;
	private static BMaze_Manager instance = null;
	private Coroutine tutorialCoroutine;

	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

        CheckForGameManager ();

		level = GameManager.GetInstance ().GetLevel (DataType.Minigame.BrainMaze) - 1;
		if (level > 2) {
			level = 2;

		}

		if (SoundManager.GetInstance ()) {
			SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
			SoundManager.GetInstance ().StopPlayingVoiceOver ();
		}

		backButton.SetActive (true);
		gameOverCanvas.SetActive (false);
		subtitlePanel.SetActive (false);
		tutorialHand.SetActive (false);
		ChangeSlider (0f);
		timeLeft = timeLimit;
		if (timerText == null)
			Debug.LogError ("No Timer Found!");
		timerText.text = Mathf.Round (timeLeft).ToString ();

		typeOfMonster = GameManager.GetInstance().GetMonsterType ();
	}

    public override void PregameSetup () {
        if (GameManager.GetInstance ().GetPendingTutorial (DataType.Minigame.BrainMaze)) {
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        } else {
            SetupMaze (level);
        }

    }

    void SetupMaze(int level) {
		beachBackgroundObj.GetComponent<BMaze_BeachBackgrounds> ().ChangeBackground (level);
		mazeGraphicObj.GetComponent<BMaze_MazeGraphics> ().ChangeMaze (level);

		for (int i = 0; i < mazeColliders.Length; i++) {
			if (i != level) {
				mazeColliders [i].SetActive (false);
				assetList [i].gameObject.SetActive (false);
			} else {
				mazeColliders [i].SetActive (true);
				assetList [i].gameObject.SetActive (true);
			}
		}

		if (monsterObject)
			RemoveMonster ();
		monsterObject = CreateMonster ();
		StartCoroutine (Countdown ());
	}
		
	void Update () {
		if (timeLeft > 0f && gameStarted) {
			timeLeft -= Time.deltaTime;
			timerText.text = Mathf.Round (timeLeft).ToString ();
			ChangeSlider(timeLeft / timeLimit);
		}
	}

	IEnumerator RunTutorial () {
		isTutorialRunning = true;
		tutorialAssets.gameObject.SetActive (true);
		print ("RunTutorial");
		mazeColliders [0].SetActive (true);
		scoreSlider.gameObject.SetActive (false);
		timer.SetActive (false);

		yield return new WaitForSeconds(0.25f);
		subtitlePanel.SetActive (true);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Brain Maze!", null);

		SoundManager.GetInstance().StopPlayingVoiceOver();
        AudioClip tutorial1 = voData.FindVO ("tutorial1");
        AudioClip tutorial2 = voData.FindVO ("tutorial2");
        AudioClip tutorial3 = voData.FindVO ("tutorial3");

        SoundManager.GetInstance().PlayVoiceOverClip(tutorial1);
		yield return new WaitForSeconds(tutorial1.length);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		monsterObject = CreateMonster ();
        monsterObject.GetComponent<BMaze_MonsterMovement> ().allowMovement = false;

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

        subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Now you try!", voData.FindVO("nowyoutry"));
		GameObject startingLocation = instance.tutorialAssets.GetStartLocation();
		monsterObject.transform.position = GetStartingLocationVector (startingLocation);
		tutorialPickup.GetComponent<BMaze_Pickup>().ReActivate ();
        monsterObject.GetComponent<BMaze_MonsterMovement> ().allowMovement = true;

        subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		yield return new WaitForSeconds(2f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Get all the pickups!", null);
		yield return new WaitForSeconds(5f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
	}

	public void TutorialFinished() {
		instance.inputAllowed = false;
		tutorialHand.SetActive (false);
		GameManager.GetInstance ().CompleteTutorial(DataType.Minigame.BrainMaze);
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

	public IEnumerator TutorialDoorUnlockedVO () {
		SoundManager.GetInstance().StopPlayingVoiceOver();

        AudioClip unlockeddoor = voData.FindVO ("unlockeddoor");
        SoundManager.GetInstance ().PlayVoiceOverClip (unlockeddoor);
        yield return new WaitForSeconds (unlockeddoor.length);
    }

	IEnumerator TutorialTearDown() {
		print ("TutorialTearDown");
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Let's play!", null);
		yield return new WaitForSeconds(2.0f);
		isTutorialRunning = false;
		yield return new WaitForSeconds(1.0f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		instance.tutorialAssets.gameObject.SetActive (false);
		SetupMaze (level);
	}

	public void ChangeSlider(float value) {
		scoreSlider.value = value;
	}

	IEnumerator Countdown() {
        yield return new WaitForSeconds (1.0f);
        GameManager.GetInstance ().Countdown ();
		scoreSlider.gameObject.SetActive (true);
		timer.SetActive (true);
		yield return new WaitForSeconds (4.0f);
		GameStart ();
	}

	public void GameStart () {
		gameStarted = true;
        monsterObject.GetComponent<BMaze_MonsterMovement> ().allowMovement = true;
        if (instance.skipButton)
			instance.skipButton.SetActive (true);
	}

	public override void GameOver () {
		if (!isTutorialRunning && gameStarted ) {
			gameStarted = false;

			if (level == 0)
				UnlockSticker();
			else {
                GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.CompletedLevel);
            }

            if (GameManager.GetInstance ())
				GameManager.GetInstance ().LevelUp (DataType.Minigame.BrainMaze);
		}
	}

	public IEnumerator EndGameWait (float duration) {
		yield return new WaitForSeconds (duration);
		GameOver ();
	}

	public void ShowGameOver() {
		backButton.SetActive (false);
		gameOverCanvas.SetActive (true);
		Text gameoverScore = gameOverCanvas.GetComponentInChildren<Text> ();
		gameoverScore.text = "Good job! You led your monster through the maze and collected all the pickups!";
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
		GameObject startingLocation = assetList [level].GetStartLocation();

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
		monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale[level]) + 1.0f;
	}

	public static BMaze_Manager GetInstance() {
		return instance;
	}

	public static GameObject GetMonster() {
		return monsterObject;
	}

	public static void RemoveMonster() {
		Destroy(monsterObject);
	}

	public static bool GetGameStarted() {
		return gameStarted;
	}

	public void UnlockDoor () {
        StartCoroutine ("TutorialDoorUnlockedVO");
		if (isTutorialRunning) {
			instance.tutorialAssets.GetDoor ().OpenDoor ();
			instance.tutorialAssets.GetFinishline ().UnlockFinishline ();
		}
		else {
			instance.assetList [level].GetDoor().OpenDoor();
			instance.assetList [level].GetFinishline ().UnlockFinishline ();
		}

	}

	public void ShowSubtitle(string text) {
		subtitlePanel.GetComponent<SubtitlePanel> ().Display (text, null);
	}
}
