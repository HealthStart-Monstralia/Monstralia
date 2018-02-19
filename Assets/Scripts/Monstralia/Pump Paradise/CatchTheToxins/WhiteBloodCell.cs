using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBloodCell : MonoBehaviour {
    public delegate void OnScore ();
    public OnScore Score;
    
    private void OnCollisionEnter2D (Collision2D collision) {
        print ("Collision");
        if (collision.gameObject.CompareTag ("Target")) {
            Destroy (collision.gameObject);
            Score ();
        }
    }
}
