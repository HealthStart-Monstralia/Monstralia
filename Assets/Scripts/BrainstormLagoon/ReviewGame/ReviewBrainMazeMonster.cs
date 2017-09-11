using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewBrainMazeMonster : MonoBehaviour {
	public bool allowMovement;
	public Sprite[] spriteList;

	private Vector3 pointerOffset;
	private Vector3 cursorPos;
	private Rigidbody2D rigBody;

	void Start () {
		rigBody = GetComponent<Rigidbody2D> ();
	}

	public void OnMouseDown() {
        if (allowMovement) {
            cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
		}
	}

	public void OnMouseDrag() {
        if (allowMovement) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
		}
	}

	public void MoveTowards (Vector2 pos) {
        rigBody.AddForce ((pos - rigBody.position) * 6);
        //rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.8f));
	}

    public void FadeOut () {
        GetComponentInChildren<Animator> ().Play ("ReviewBrainMazeMonsterFadeOut");
    }
}
