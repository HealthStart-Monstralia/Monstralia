using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewLoseTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.GetComponent<ReviewBrainMazeMonster> ()) {
            //ReviewGameWinLose.Instance.LoseCondition ();
        }
    }
}
