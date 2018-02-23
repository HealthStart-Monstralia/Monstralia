using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour {
    public delegate void OnScore ();
    public static OnScore Score;

    public AudioClip collideSound;
    public AudioClip deathSound;

    private bool onScreen = false;

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.CompareTag ("Target")) {
            GetComponentInParent<CatchToxinMovement> ().OnDeath ();
            if (onScreen)
                SoundManager.Instance.PlaySFXClip (deathSound);
        }
        else if (other.gameObject.CompareTag ("Player")) {
            SoundManager.Instance.PlaySFXClip (collideSound);
        }
    }

    private void OnBecameVisible () {
        onScreen = true;
    }

    private void OnBecameInvisible () {
        if (transform.position.y < Camera.main.ViewportToWorldPoint (new Vector2 (0f, 0f)).y) {
            Score ();
            Destroy (gameObject);
        }
    }
}
