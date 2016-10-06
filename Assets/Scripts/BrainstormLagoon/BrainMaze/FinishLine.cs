using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public GameObject door;

	private AudioSource audioSrc;

	void Start () {
		audioSrc = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (!door)
			audioSrc.Play ();
	}
}
