using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging_CreateManagers : MonoBehaviour {
	/**
	 * \class Debugging_CreateManagers
	 * \brief Creates the needed managers for debugging purposes.
	 * CREATED BY: Colby Tang
	 */

	public bool startDebugging = false;
	public SoundManager soundMan;
	public GameManager gameMan;

	// Use this for initialization
	void Awake () {
		if (startDebugging) {
			if (!GameObject.FindObjectOfType<SoundManager>())
				Instantiate (soundMan);
			if (!GameObject.FindObjectOfType<GameManager> ()) {
				GameManager gameManagerObject = Instantiate (gameMan);
				gameManagerObject.setMonster("Green");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
