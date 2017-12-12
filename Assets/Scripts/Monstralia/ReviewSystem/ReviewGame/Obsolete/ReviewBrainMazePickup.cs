using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewBrainMazePickup : MonoBehaviour {
	public AudioClip pickupSfx;

	void OnTriggerEnter2D(Collider2D col) {
		SoundManager.GetInstance().PlaySFXClip(pickupSfx);
		gameObject.SetActive (false);
		ReviewBrainMazeCanvas.GetInstance ().EndReview ();
	}
}
