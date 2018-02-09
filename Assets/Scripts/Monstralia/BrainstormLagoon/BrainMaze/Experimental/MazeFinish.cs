using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeFinish : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.transform.parent.GetComponent<Monster> ()) {
            ExperimentalBrainMazeManager.Instance.RestartGame ();
        }
    }
}
