﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BMaze_MonsterMovement : MonoBehaviour {
    /* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
    public Vector2 gotoPos;
    public bool finished = false;

    private Vector3 pointerOffset;
	private Vector3 cursorPos;
	private Rigidbody2D rigBody;

	void Start () {
        rigBody = GetComponent<Rigidbody2D> ();
        if (!rigBody)
            rigBody = gameObject.AddComponent<Rigidbody2D> ();
        rigBody.gravityScale = 0.0f;
        rigBody.freezeRotation = true;
        rigBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

	void FixedUpdate () {
		if (finished)
			FinishMove (gotoPos);
	}

    /*
    public void OnMouseDown () {
        cursorPos = Input.mousePosition;
        cursorPos.z -= (Camera.main.transform.position.z + 10f);
        pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
    }

    public void OnMouseDrag () {
        cursorPos = Input.mousePosition;
        cursorPos.z -= (Camera.main.transform.position.z + 10f);
        MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
    }
    */

	public void OnMouseDown() {
		if (BMaze_Manager.GetInstance().inputAllowed) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
		}
	}

	public void OnMouseDrag() {
		if (BMaze_Manager.GetInstance ().inputAllowed) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
		}
	}

    public void MoveTowards (Vector2 pos) {
		rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.8f));
	}

	public void FinishMove (Vector2 pos) {
		transform.position = Vector2.MoveTowards (transform.position, pos, 0.3f);
		if (transform.position.x == pos.x && transform.position.y == pos.y)
			finished = false;
	}
}