using UnityEngine;
using System.Collections;

public class BMaze_Finishline : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public GameObject door;

	private AudioSource audioSrc;
	private bool finished;

	void Start () {
		if (door)
			finished = false;
		audioSrc = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (!door && !finished) {
			col.GetComponentInChildren<Animator> ().Play ("BMaze_Dance");
			finished = true;
			audioSrc.Play ();
			BMaze_Manager.GameEnd ();
			Invoke ("ChangeScene", 3f);
		}
	}

	void ChangeScene () {
		GetComponent<SwitchScene> ().loadScene ();
	}
}
