using UnityEngine;
using System.Collections;

public class BMazeFinishline : MonoBehaviour {
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
        BMazeMonsterMovement movement = col.GetComponent<BMazeMonsterMovement> ();
        if (movement) {
            BMazeManager.Instance.PlayDance ();
            col.GetComponent<BMazeMonsterMovement> ().finished = true;
            col.GetComponent<BMazeMonsterMovement> ().gotoPos = finishSpot.transform.position;
            BMazeManager.Instance.OnFinish ();
        }

	}
}
