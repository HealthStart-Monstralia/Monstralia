using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragMovement : MonoBehaviour {
    private Vector3 pointerOffset;
    private Vector3 cursorPos;
    private Rigidbody2D rigBody;
    private float startYAxis;

    void Start () {
        rigBody = GetComponent<Rigidbody2D> ();
        if (!rigBody) {
            rigBody = gameObject.AddComponent<Rigidbody2D> ();
            rigBody.bodyType = RigidbodyType2D.Kinematic;
            rigBody.freezeRotation = true;
            rigBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        startYAxis = transform.position.y;
    }

    public void OnMouseDown () {
        if (CatchToxinsManager.Instance.isInputAllowed) {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.nearClipPlane + 10f);
            cursorPos = new Vector3 (cursorPos.x, startYAxis, cursorPos.z);
            pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
        }
    }

    public void OnMouseDrag () {
        if (CatchToxinsManager.Instance.isInputAllowed) {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            cursorPos = new Vector3 (cursorPos.x, startYAxis, cursorPos.z);
            MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
        }
    }

    public void MoveTowards (Vector2 pos) {
        rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.4f));
    }
}
