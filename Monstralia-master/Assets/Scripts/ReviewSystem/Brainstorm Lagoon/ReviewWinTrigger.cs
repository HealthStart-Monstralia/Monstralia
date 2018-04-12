using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewWinTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.GetComponent<ReviewBrainMazeMonster> ()) {
            print(col);
            ReviewBrainMazeCanvas.Instance.EndReview();
        }
    }
}
