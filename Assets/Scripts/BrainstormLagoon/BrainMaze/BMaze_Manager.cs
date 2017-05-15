using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BMaze_Manager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public enum MonsterType {
		Blue = 0, 
		Green = 1, 
		Red = 2, 
		Yellow = 3
	};

	public GameManager.MonsterType typeOfMonster;
	public GameObject[] monsterList = new GameObject[4];
	[Range(0.1f,1.0f)]
	public float[] monsterScale;
	public static GameObject monsterObject;
	public GameObject monsterShadow;

	public AudioClip backgroundMusic;
	public Slider scoreSlider;
	public Text timerText;
	public float timeLimit;
	public static float timeLeft;
	public static int level = 0;
	public GameObject beachBackgroundObj, mazeGraphicObj;
	public GameObject[] mazeColliders;
	public BMaze_SceneAssets[] assetList;
	public bool inputAllowed = false;

	private static bool gameStarted = false;
	private static BMaze_Manager instance = null;

	void SetupMaze(int level) {
		print (level);
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
	}

	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		if (GameManager.GetInstance())
			level = GameManager.GetInstance ().GetLevel ("BrainMaze") - 1;
		SetupMaze (level);
		typeOfMonster = GameManager.GetMonster();
		CreateMonster ();
	}

	void Start () {
		if (SoundManager.GetInstance ()) {
			SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
			SoundManager.GetInstance ().StopPlayingVoiceOver ();
		}

		ChangeSlider (0f);
		GameStart ();
		timeLeft = timeLimit;
		if (timerText == null)
			Debug.LogError ("No Timer Found!");
		timerText.text = Mathf.Round(timeLeft).ToString();

	}

	void Update () {
		if (timeLeft > 0f && gameStarted) {
			timeLeft -= Time.deltaTime;
			timerText.text = Mathf.Round (timeLeft).ToString ();
			ChangeSlider(timeLeft / timeLimit);
		}
	}

	public void ChangeSlider(float value) {
		scoreSlider.value = value;
	}

	public static void GameStart () {
		gameStarted = true;
		instance.inputAllowed = true;
	}

	public static void GameEnd (bool playerWin) {
		gameStarted = false;

		if (playerWin) {
			if (GameManager.GetInstance ())
				GameManager.GetInstance ().LevelUpNoStars ("BrainMaze");
			instance.Invoke ("ChangeScene", 3f);
		}
	}

	public void ChangeScene () {
		GetComponent<SwitchScene> ().loadScene ();
	}

	public void SkipLevel () {
		gameStarted = false;
		if (GameManager.GetInstance())
			GameManager.GetInstance ().LevelUpNoStars ("BrainMaze");
		instance.ChangeScene ();
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

	void CreateMonster() {
		GameObject startingLocation = assetList [level].GetStartLocation();

		Vector3 monsterPosition = new Vector3 (
			startingLocation.transform.position.x,
          	startingLocation.transform.position.y,
          	0f);
		
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
		monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale[level]) + 0.5f;
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

	public static bool GetGameStarted() {
		return gameStarted;
	}

	public static void UnlockDoor () {
		instance.assetList [level].GetDoor().OpenDoor();
		UnlockFinish ();
	}

	public static void UnlockFinish () {
		instance.assetList [level].GetFinishline ().UnlockFinishline ();
	}
}
