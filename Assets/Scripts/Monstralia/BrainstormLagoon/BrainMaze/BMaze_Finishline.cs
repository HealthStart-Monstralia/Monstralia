using UnityEngine;
using System.Collections;

public class BMaze_Finishline : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public GameObject finishSpot;

    private void Awake () {
        GetComponent<Collider2D> ().enabled = false;
    }

    public void UnlockFinishline () {
        GetComponent<Collider2D> ().enabled = true;
	}

	void OnTriggerEnter2D (Collider2D col) {
		print ("Finish");
		col.GetComponentInChildren<BMaze_Monster> ().PlayDance ();
		col.GetComponent<BMaze_MonsterMovement> ().finished = true;
		col.GetComponent<BMaze_MonsterMovement> ().gotoPos = finishSpot.transform.position;
        BMaze_Manager.GetInstance ().OnFinish ();
	}
}
