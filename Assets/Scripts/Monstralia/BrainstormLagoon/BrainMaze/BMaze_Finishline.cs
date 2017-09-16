using UnityEngine;
using System.Collections;

public class BMaze_Finishline : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public GameObject finishSpot;

	private AudioSource audioSrc;
	private bool finished = false;

	void Start () {
		audioSrc = GetComponent<AudioSource> ();
	}

	public void UnlockFinishline () {
		finished = true;
	}

	void OnTriggerEnter2D (Collider2D col) {
		print ("Finish");
		if (finished) {
			col.GetComponentInChildren<BMaze_Monster> ().PlayDance ();
			col.GetComponent<BMaze_MonsterMovement> ().allowMovement = false;
			col.GetComponent<BMaze_MonsterMovement> ().finished = true;
			col.GetComponent<BMaze_MonsterMovement> ().gotoPos = finishSpot.transform.position;
			//col.transform.position = finishSpot.transform.position;

			audioSrc.Play ();
			if (!BMaze_Manager.isTutorialRunning)
				StartCoroutine(BMaze_Manager.GetInstance().EndGameWait (3f));
			else {
				BMaze_Manager.GetInstance ().TutorialFinished ();
			}
		}
	}
}
