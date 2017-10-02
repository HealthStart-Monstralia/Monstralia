using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBone : MonoBehaviour {

    private Rigidbody2D rigBody;
    private BoxCollider2D col;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;

    private void Awake () {

    }

    public void OnMouseDown () {
        if (BridgeBonesManager.GetInstance ().inputAllowed) {
            print ("Mousedown");
            transform.SetParent (transform.root);
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
        }
    }

    public void OnMouseDrag () {
        if (BridgeBonesManager.GetInstance ().inputAllowed) {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
        }
    }

    public void MoveTowards (Vector2 pos) {
        rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.1f));
    }
}
