using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePickup : MonoBehaviour {
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<Monster> ()) {
            Destroy (gameObject);
            Maze.Instance.OnPickup (this);
        }
    }
}
