using UnityEngine;
using System.Collections;

public class MemoryMatchGameManager : MonoBehaviour {

	private static MemoryMatchGameManager instance;
	private bool gameStart;

	public Transform foodToMatchSpawnPos;
	public GameObject[] foods;
	public Timer timer;
	public float timeLimit;

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		if(timer != null) {
			timer = Instantiate(timer);
			timer.SetTimeLimit(timeLimit);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static MemoryMatchGameManager GetInstance() {
		return instance;
	}

	public void StartGame() {
		gameStart = true;
		timer.StartTimer();
	}
}
