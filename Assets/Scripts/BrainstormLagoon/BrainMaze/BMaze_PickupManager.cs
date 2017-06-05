using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BMaze_PickupManager : MonoBehaviour {
	/* CREATED BY: Colby Tang.
	 * GAME: Brain Maze
	 */

	public List<GameObject> pickupList = new List<GameObject> ();

	private AudioSource audioSrc;
	private bool achieved = false;

	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		for (int count = 0; count < transform.childCount; count++) {
			pickupList.Add (transform.GetChild (count).gameObject);
		}
	}

	void Update () {
		if (pickupList.Count <= 0 && !achieved) {
			achieved = true;
			GoalAchieved ();
		}
	}

	void GoalAchieved () {
		audioSrc.Play ();
		BMaze_Manager.UnlockDoor ();
	}
}
