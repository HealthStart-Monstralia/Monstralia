using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchToxinMovement : MonoBehaviour {
    public float speed;
    public bool CanMove {
        get {
            return canMove;
        }

        set {
            canMove = value;
            if (!canMove) {
                rigbody.bodyType = RigidbodyType2D.Kinematic;
                rigbody.velocity = Vector2.zero;
            } else {
                rigbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private bool canMove;
    private Rigidbody2D rigbody;
    private Animator animator;

    private void Awake () {
        rigbody = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        CanMove = true;
    }

    public void OnDeath () {
        GetComponent<Collider2D> ().enabled = false;
        CanMove = false;
        animator.SetBool ("isDying", true);
    }

    public void DestroyFromAnim () {
        Destroy (gameObject);
    }

    void FixedUpdate () {
        if (CanMove && rigbody.velocity.y > -speed)
            rigbody.AddForce (Vector2.down * speed);
    }
}
