using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BMaze_Manager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public AudioClip backgroundMusic;
	public GameObject scoreSlider;
	public Text timerText;
	public float timeLimit;
	public static float timeLeft;

	protected static bool gameStarted = false;
	private Slider scoreSliderComponent;

	// Use this for initialization
	void Start () {
		scoreSliderComponent = scoreSlider.GetComponent<Slider>();
		ChangeSlider (0f);
		GameStart ();
		timeLeft = timeLimit;
		if (timerText == null)
			Debug.LogError ("No Timer Found!");
		timerText.text = Mathf.Round(timeLeft).ToString();
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
	}
}
