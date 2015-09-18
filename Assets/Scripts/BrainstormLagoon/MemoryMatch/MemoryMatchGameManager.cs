using UnityEngine;
using System.Collections;

public class MemoryMatchGameManager : MonoBehaviour {

	private static MemoryMatchGameManager instance;
	private bool timing;

	public Transform foodToMatchSpawnPos;
	public GameObject[] foods;

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static MemoryMatchGameManager GetInstance() {
		return instance;
	}

	public void StartGame() {
		timing = true;
	}
}
