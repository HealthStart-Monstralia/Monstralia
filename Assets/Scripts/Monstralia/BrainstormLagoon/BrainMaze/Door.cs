using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public void OpenDoor() {
        Invoke ("Hide", 0.5f);
	}

	public void Hide() {
		gameObject.SetActive (false);
	}

	public void Show() {
		gameObject.SetActive (true);
	}
}
