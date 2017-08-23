using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewLoseTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col) {
        ReviewGameWinLose.GetInstance().LoseCondition();
    }
}
