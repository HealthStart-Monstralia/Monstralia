using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour {
    public delegate void OnScore ();
    public static OnScore Score;

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Target")) {
            Destroy (gameObject);
        }
    }

    private void OnBecameInvisible () {
        if (transform.position.y < Camera.main.ViewportToWorldPoint (new Vector2 (0f, 0f)).y) {
            Score ();
            Destroy (gameObject);
        }
    }
}
