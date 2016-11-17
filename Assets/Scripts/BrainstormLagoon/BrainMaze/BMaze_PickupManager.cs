using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BMaze_PickupManager : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameObject door;
	public Slider scoreGauge;
	public List<GameObject> pickupList = new List<GameObject> ();

	private AudioSource audioSrc;
	private int initialPickupCount;
	private int pickupCount;

	void Start () {
		audioSrc = GetComponent<AudioSource> ();
		for (int count = 0; count < transform.childCount; count++) {
			pickupList.Add (transform.GetChild (count).gameObject);
		}
		initialPickupCount = pickupList.Count;
		scoreGauge.value = 0f;
	}

	void Update () {
		if (pickupList.Count <= 0 && door.gameObject) {
			OpenDoor ();
		}

		if (pickupList.Count != 0 && pickupList.Count != pickupCount) {
			pickupCount = pickupList.Count;
			scoreGauge.value = 1 - ((float)pickupCount / (float)initialPickupCount);
		} else if (pickupList.Count == 0 && pickupList.Count != pickupCount) {
			scoreGauge.value = 1;
		}
	}

	void OpenDoor () {
		audioSrc.Play ();
		Destroy (door, 0.5f);
	}
}
