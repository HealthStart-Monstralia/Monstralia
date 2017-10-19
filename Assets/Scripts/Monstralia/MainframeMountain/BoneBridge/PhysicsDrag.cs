using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDrag : MonoBehaviour {
    public bool holdNoGrav;
    private Rigidbody2D rigBody;
    private BoxCollider2D col;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;

    private void Start () {
        rigBody = GetComponent<Rigidbody2D> ();
    }

    public void OnMouseDown () {
        print ("Mousedown");
        if (holdNoGrav)
            rigBody.gravityScale = 0f;
        transform.SetParent (transform.root);
        cursorPos = Input.mousePosition;
        cursorPos.z -= (Camera.main.transform.position.z + 10f);
        pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
    }

    public void OnMouseDrag () {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
    }

    public void OnMouseUp () {
        if (holdNoGrav)
            rigBody.gravityScale = 1f;
    }

    public void MoveTowards (Vector2 pos) {
        rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.1f));
    }
}
