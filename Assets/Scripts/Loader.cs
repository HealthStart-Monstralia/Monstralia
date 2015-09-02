using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	
	public GameObject gameManager;
	public AudioClip backgroundMusic;

	void Awake() { 

		if(GameManager.instance == null) {
			Instantiate(gameManager);
		}
	}
}
