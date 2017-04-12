using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BMaze_Manager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public static GameObject monsterObject;
	public static BMaze_Manager manager;
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
		if (sceneSelect != currentScene) {
			manager.GetComponent<SwitchScene> ().sceneToLoad = (sceneList [sceneSelect - 1]);
			ChangeScene ();
		} else {
			monsterObject = GameObject.Find ("Monster");
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
}
