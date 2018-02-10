﻿using UnityEngine;
using System.Collections;

public class BMazeFinishline : MonoBehaviour {
    /* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
    public MazeCell cell;
    public MazeDirection direction;
    public GameObject finishSpot;

    private void Awake () {
        GetComponent<Collider2D> ().enabled = false;
    }

    public void UnlockFinishline () {
        GetComponent<Collider2D> ().enabled = true;
	}

	void OnTriggerEnter2D (Collider2D col) {
        if (col.tag == "Monster") {
            if (BMazeManager.Instance.OnFinish ()) {
                GetComponent<Collider2D> ().enabled = false;
            }
        }
	}

    public void Initialize (MazeCell cell, MazeDirection direction) {
        this.cell = cell;
        this.direction = direction;
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation ();
    }
}
