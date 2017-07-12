using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BMaze_Manager : AbstractGameManager {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameManager.MonsterType typeOfMonster;
	public GameObject[] monsterList = new GameObject[4];
	[Range(0.1f,1.0f)]
	public float[] monsterScale;
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
	public bool inputAllowed = false;
	public GameObject subtitlePanel;
	public AudioClip[] instructionVOList;

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

		if (!GameManager.GetInstance ()) {
			SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
			switchScene.loadScene ("Start");
		} else {
			level = GameManager.GetInstance ().GetLevel ("BrainMaze") - 1;
			if (level > 2) {
				level = 2;

			}

			if (SoundManager.GetInstance ()) {
				SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
				SoundManager.GetInstance ().StopPlayingVoiceOver ();
			}

			backButton.SetActive (true);
			stickerPopupCanvas.gameObject.SetActive (false);
			gameOverCanvas.SetActive (false);
			subtitlePanel.SetActive (false);
			tutorialHand.SetActive (false);
			ChangeSlider (0f);
			timeLeft = timeLimit;
			if (timerText == null)
				Debug.LogError ("No Timer Found!");
			timerText.text = Mathf.Round (timeLeft).ToString ();

			if (GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.BRAINMAZE]) {
				tutorialCoroutine = StartCoroutine (RunTutorial ());
			} else {
				SetupMaze (level);
			}

			typeOfMonster = GameManager.GetMonsterType ();
		}
	}

	void SetupMaze(int level) {
		beachBackgroundObj.GetComponent<BMaze_BeachBackgrounds> ().ChangeBackground (level);
		mazeGraphicObj.GetComponent<BMaze_MazeGraphics> ().ChangeMaze (level);
		for (int i = 0; i < mazeColliders.Length; i++) {
			if (i != level) {
				mazeColliders [i].SetActive (false);
				assetList [i].gameObject.SetActive (false);
				/*
				assetList [i].GetPickupManager ().gameObject.SetActive (false);
				assetList [i].GetDoor().gameObject.SetActive (false);
				assetList [i].GetFinishline().gameObject.SetActive (false);
				*/
			} else {
				mazeColliders [i].SetActive (true);
				assetList [i].gameObject.SetActive (true);
				/*
				assetList [i].GetPickupManager ().gameObject.SetActive (true);
				assetList [i].GetDoor().gameObject.SetActive (true);
				assetList [i].GetFinishline().gameObject.SetActive (true);
				*/
			}
		}
		if (monsterObject)
			RemoveMonster ();
		CreateMonster ();
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
		instance.tutorialAssets.gameObject.SetActive (true);
		print ("RunTutorial");
		mazeColliders [0].SetActive (true);
		instance.inputAllowed = false;
		scoreSlider.gameObject.SetActive (false);
		timer.SetActive (false);

		yield return new WaitForSeconds(0.5f);
		subtitlePanel.SetActive (true);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Welcome to Brain Maze!", null);

		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(instructionVOList[0]);
		yield return new WaitForSeconds(instructionVOList[0].length);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();

		CreateMonster ();
		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(instructionVOList[1]);
		yield return new WaitForSeconds(instructionVOList[1].length - 1f);

		tutorialHand.SetActive (true);
		tutorialHand.GetComponent<Animator> ().Play ("BMaze_HandMoveMonster");
		yield return new WaitForSeconds(1.5f);
		monsterObject.transform.SetParent (tutorialHand.transform);
		yield return new WaitForSeconds(1.5f);
		monsterObject.transform.SetParent (null);


		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(instructionVOList[2]);
		yield return new WaitForSeconds(instructionVOList[2].length);

		SoundManager.GetInstance().StopPlayingVoiceOver();
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Now you try!", instructionVOList[3]);
		GameObject startingLocation = instance.tutorialAssets.GetStartLocation();
		monsterObject.transform.position = GetStartingLocationVector (startingLocation);
		tutorialPickup.GetComponent<BMaze_Pickup>().ReActivate ();
		instance.inputAllowed = true;
		yield return new WaitForSeconds(instructionVOList[3].length);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
		yield return new WaitForSeconds(2f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Display ("Get all the pickups!", null);
		yield return new WaitForSeconds(5f);
		subtitlePanel.GetComponent<SubtitlePanel> ().Hide ();
	}

	public void TutorialFinished() {
		instance.inputAllowed = false;
		tutorialHand.SetActive (false);
		GameManager.GetInstance ().LagoonTutorial [(int)Constants.BrainstormLagoonLevels.BRAINMAZE] = false;
		StopCoroutine (tutorialCoroutine);
		StartCoroutine(TutorialTearDown ());
	}

	public IEnumerator TutorialDoorUnlockedVO () {
		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(instructionVOList[4]);
		yield return new WaitForSeconds(instructionVOList[4].length);
		SoundManager.GetInstance().PlayVoiceOverClip(instructionVOList[5]);
		yield return new WaitForSeconds(instructionVOList[5].length);
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
		GameManager.GetInstance ().Countdown ();
		scoreSlider.gameObject.SetActive (true);
		timer.SetActive (true);
		yield return new WaitForSeconds (4.0f);
		GameStart ();
	}

	public void GameStart () {
		gameStarted = true;
		instance.inputAllowed = true;
		if (instance.skipButton)
			instance.skipButton.SetActive (true);
	}

	public override void GameOver () {
		if (!isTutorialRunning && gameStarted ) {
			gameStarted = false;

			if (level == 0)
				UnlockSticker(StickerManager.StickerType.Frontal);
			else {
				ShowGameOver ();
			}

			if (GameManager.GetInstance ())
				GameManager.GetInstance ().LevelUp ("BrainMaze");
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
		GetComponent<SwitchScene> ().loadScene ();
	}

	public void SkipLevel (GameObject button) {
		if (isTutorialRunning) {
			TutorialFinished ();
		} else {
			gameStarted = false;
			if (GameManager.GetInstance())
				GameManager.GetInstance ().LevelUp ("BrainMaze");
			instance.ChangeScene ();
		}
		skipButton = button;
		button.SetActive (false);
	}

	/*
	public void NextSceneSelect () {
		if (sceneSelect < 5)
			sceneSelect += 1;
		instance.Invoke ("ChangeScene", 3f);
	}

	public void NextSceneSelectFast () {
		if (sceneSelect < 5)
			sceneSelect += 1;
		instance.Invoke ("ChangeScene", 0.25f);
	}
	*/

	/*
	void DetermineMonster() {
		// Determines what kind of monster is chosen
		if (GameManager.GetInstance ()) {
			if (GameManager.GetInstance ().getMonster ().Contains ("Blue")) {
				typeOfMonster = MonsterType.Blue;
			} else {
				if (GameManager.GetInstance ().getMonster ().Contains ("Green")) {
					typeOfMonster = MonsterType.Green;
				} else {
					if (GameManager.GetInstance ().getMonster ().Contains ("Red")) {
						typeOfMonster = MonsterType.Red;
					} else {
						typeOfMonster = MonsterType.Yellow;
					}
				}
			}
		} else {
			typeOfMonster = MonsterType.Green;
		}
	}
	*/

	Vector3 GetStartingLocationVector(GameObject location) {
		return new Vector3 (
			location.transform.position.x,
			location.transform.position.y,
			0f);
	}

	void CreateMonster() {
		GameObject startingLocation = assetList [level].GetStartLocation();

		Vector3 monsterPosition = GetStartingLocationVector (startingLocation);
		
		Quaternion monsterRotation = startingLocation.transform.rotation;

		switch (typeOfMonster) {
		case GameManager.MonsterType.Blue:
			monsterObject = Instantiate (
				monsterList [0], 
				monsterPosition,
				monsterRotation) as GameObject;
			ScaleMonster ();
			break;
		case GameManager.MonsterType.Green:
			monsterObject = Instantiate (
				monsterList [1], 
				monsterPosition,
				monsterRotation) as GameObject;
			ScaleMonster ();
			break;
		case GameManager.MonsterType.Red:
			monsterObject = Instantiate (
				monsterList [2], 
				monsterPosition,
				monsterRotation) as GameObject;
			ScaleMonster ();
			break;
		case GameManager.MonsterType.Yellow:
			monsterObject = Instantiate (
				monsterList [3], 
				monsterPosition,
				monsterRotation) as GameObject;
			ScaleMonster ();
			break;
		}

		monsterObject.GetComponent<BMaze_Monster>().PlaySpawn ();
	}

	void ScaleMonster() {
		monsterObject.transform.localScale = new Vector3(monsterScale[level], monsterScale[level], monsterScale[level]);
		monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale[level]) + 1.0f;
	}

	/*
	public void CreateShadowObject () {
		GameObject shadow = Instantiate (monsterShadow, startingLocation.transform.position, startingLocation.transform.rotation) as GameObject;
		shadow.GetComponent<BMaze_Shadow> ().objectToFollow = monsterObject;
	}
	*/

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

	public static void UnlockDoor () {
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
