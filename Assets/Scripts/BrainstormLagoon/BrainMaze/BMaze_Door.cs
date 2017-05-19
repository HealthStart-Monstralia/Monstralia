using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Door : MonoBehaviour {

	public void OpenDoor() {
		AudioSource audio = GetComponent<AudioSource> ();
		audio.Play ();
		Invoke ("Hide", 0.5f);
		//Destroy(gameObject, 0.5f);
	}

	public void Hide() {
		gameObject.SetActive (false);
	}

	public void Show() {
		gameObject.SetActive (true);
	}
}
