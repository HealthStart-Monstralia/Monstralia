using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Door : MonoBehaviour {
    [SerializeField] private AudioClip unlockClip;

	public void OpenDoor() {
        SoundManager.GetInstance ().PlaySFXClip (unlockClip);
        Invoke ("Hide", 0.5f);
	}

	public void Hide() {
		gameObject.SetActive (false);
	}

	public void Show() {
		gameObject.SetActive (true);
	}
}
