using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BMaze_Manager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public enum MonsterType {Blue = 0, Green = 1, Red = 2, Yellow = 3};

	public MonsterType typeOfMonster;
	public GameObject[] monsterList = new GameObject[4];
	[Range(0.01f,0.5f)]
	public float monsterScale = 0.4f;
	public static GameObject monsterObject;
	public GameObject monsterShadow;

	public static BMaze_Manager manager;
	private static BMaze_Manager instance = null;
	public GameObject startingLocation;
	public AudioClip backgroundMusic;
	public GameObject scoreSlider;
	public Text timerText;
	public float timeLimit;
	public static float timeLeft;
	public static int sceneSelect = 1;
	public int currentScene;
	public string[] sceneList = new string[5];


	protected static bool gameStarted = false;
	private Slider scoreSliderComponent;

	void Awake () {
		manager = this;

		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		DetermineMonster ();
		CreateMonster ();
		if (sceneSelect != currentScene) {
			manager.GetComponent<SwitchScene> ().sceneToLoad = (sceneList [sceneSelect - 1]);
			ChangeScene ();
		}
	}

	// Use this for initialization
	void Start () {
		scoreSliderComponent = scoreSlider.GetComponent<Slider>();
		ChangeSlider (0f);
		GameStart ();
		timeLeft = timeLimit;
		if (timerText == null)
			Debug.LogError ("No Timer Found!");
		timerText.text = Mathf.Round(timeLeft).ToString();
		if (SoundManager.GetInstance())
			SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
	}
	
	// Update is called once per frame
	void Update () {
		if (timeLeft > 0f && gameStarted) {
			timeLeft -= Time.deltaTime;
			timerText.text = Mathf.Round (timeLeft).ToString ();
			ChangeSlider(timeLeft / timeLimit);
		}
	}

	public void ChangeSlider(float value) {
		scoreSliderComponent.value = value;
	}

	public static void GameStart () {
		gameStarted = true;
	}

	public static void GameEnd () {
		gameStarted = false;
		manager.NextSceneSelect ();
	}

	public void ChangeScene () {
		GetComponent<SwitchScene> ().loadScene ();
	}

	public void NextSceneSelect () {
		if (sceneSelect < 5)
			sceneSelect += 1;
		manager.Invoke ("ChangeScene", 3f);
	}

	public void NextSceneSelectFast () {
		if (sceneSelect < 5)
			sceneSelect += 1;
		manager.Invoke ("ChangeScene", 0.25f);
	}

	void DetermineMonster() {
		// Determines what kind of monster is chosen
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
	}

	void CreateMonster() {
		switch (typeOfMonster) {
		case MonsterType.Blue:
			monsterObject = Instantiate (
				monsterList [0], 
				new Vector3(
					startingLocation.transform.position.x,
					startingLocation.transform.position.y,
					0f),
				startingLocation.transform.rotation) as GameObject;
			monsterObject.transform.localScale = new Vector3 (monsterScale, monsterScale, monsterScale);
			monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale * 2) + 0.5f;
			break;
		case MonsterType.Green:
			monsterObject = Instantiate (
				monsterList [1], 
				new Vector3(
					startingLocation.transform.position.x,
					startingLocation.transform.position.y,
					0f),
				startingLocation.transform.rotation) as GameObject;
			monsterObject.transform.localScale = new Vector3(monsterScale, monsterScale, monsterScale);
			monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale * 2) + 0.5f;
			break;
		case MonsterType.Red:
			monsterObject = Instantiate (
				monsterList [2], 
				new Vector3(
					startingLocation.transform.position.x,
					startingLocation.transform.position.y,
					0f),
				startingLocation.transform.rotation) as GameObject;
			monsterObject.transform.localScale = new Vector3(monsterScale, monsterScale, monsterScale);
			monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale * 2) + 0.5f;
			break;
		case MonsterType.Yellow:
			monsterObject = Instantiate (
				monsterList [3], 
				new Vector3(
					startingLocation.transform.position.x,
					startingLocation.transform.position.y,
					0f),
				startingLocation.transform.rotation) as GameObject;
			monsterObject.transform.localScale = new Vector3(monsterScale, monsterScale, monsterScale);
			monsterObject.GetComponent<CircleCollider2D> ().radius = (monsterScale * 2) + 0.5f;
			break;
		}

	}

	public void CreateShadowObject () {
		GameObject shadow = Instantiate (monsterShadow, startingLocation.transform.position, startingLocation.transform.rotation) as GameObject;
		shadow.GetComponent<BMaze_Shadow> ().objectToFollow = monsterObject;
	}

	public static BMaze_Manager GetInstance() {
		return instance;
	}
}
