using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameObject door;
	public List<GameObject> pickupList = new List<GameObject> ();

	private AudioSource audioSrc;

	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		for (int count = 0; count < transform.childCount; count++) {
			pickupList.Add (transform.GetChild (count).gameObject);
		}
	}

	void Update () {
		if (pickupList.Count <= 0 && door.gameObject) {
			OpenDoor ();
		}
	}

	void OpenDoor () {
		audioSrc.Play ();
		Destroy (door, 0.5f);
	}
}
