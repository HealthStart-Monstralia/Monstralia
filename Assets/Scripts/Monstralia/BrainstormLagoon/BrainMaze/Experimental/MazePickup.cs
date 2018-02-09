using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePickup : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.transform.parent.GetComponent<Monster> ()) {
            Destroy (gameObject);
            ExperimentalBrainMazeManager.Instance.OnPickup (this);
        }
    }
}
