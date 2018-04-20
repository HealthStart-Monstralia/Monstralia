using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinEnemy : MonoBehaviour {
    public AudioClip deathSound;
    
    private void OnEnable () {
        CatchToxinsManager.OnGameEnd += DestroyEnemy;
    }

    private void OnDisable () {
        CatchToxinsManager.OnGameEnd -= DestroyEnemy;
    }

    public void DestroyEnemy () {
        SoundManager.Instance.PlaySFXClip (deathSound);
        GetComponent<CatchToxinMovement> ().OnDeath ();
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag ("Player")) {
            DestroyEnemy ();
        }
    }

    private void OnBecameInvisible () {
        if (transform.position.y < Camera.main.ViewportToWorldPoint (new Vector2 (0f, 0f)).y) {
            CatchToxinsManager.Instance.OnWrongScore ();
        }

        Destroy (gameObject);
    }
}
