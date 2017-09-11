﻿using UnityEngine;
using System.Collections;

public class BMaze_Pickup : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public enum TypeOfPickup { 
		Water = 0, 
		Running = 1,
		Swimming = 2,
		Hiking = 3,
		Biking = 4
	};
	public TypeOfPickup pickup;
	public AudioClip pickupSfx;

	private BMaze_PickupManager pickupMan;

	void Start () {
		pickupMan = transform.parent.GetComponent<BMaze_PickupManager> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<BMaze_MonsterMovement> ()) {
            //col.GetComponent<BMaze_MonsterMovement> ().Pickup (pickup);
            SoundManager.GetInstance ().PlayCorrectSFX ();
            SoundManager.GetInstance().PlaySFXClip(pickupSfx);
			if (BMaze_Manager.GetInstance ()) {
				if (pickup == TypeOfPickup.Water)
					GetComponent<BMaze_WaterPickup> ().IncreaseTime ();
				BMaze_Manager.GetInstance ().ShowSubtitle (pickup.ToString ());
				pickupMan.pickupList.Remove (gameObject);
			}
			gameObject.SetActive (false);
		}
	}

	public void ReActivate() {
		pickupMan.pickupList.Add (gameObject);
		gameObject.SetActive (true);
	}
}
