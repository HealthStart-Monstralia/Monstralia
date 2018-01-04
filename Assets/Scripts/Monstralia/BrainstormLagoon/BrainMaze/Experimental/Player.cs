using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigBody;

    private void Awake () {
        rigBody = gameObject.GetComponent<Rigidbody2D> ();
    }

    private void FixedUpdate () {
        if (Input.GetKey (KeyCode.D)) { rigBody.AddForce (Vector2.right * speed);}
        if (Input.GetKey (KeyCode.A)) { rigBody.AddForce (Vector2.left * speed); }
        if (Input.GetKey (KeyCode.W)) { rigBody.AddForce (Vector2.up * speed); }
        if (Input.GetKey (KeyCode.S)) { rigBody.AddForce (Vector2.down * speed); }
    }

}
