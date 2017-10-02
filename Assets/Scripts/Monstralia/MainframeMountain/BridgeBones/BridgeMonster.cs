using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMonster : MonoBehaviour {

    public GameObject goalObject;

    private Rigidbody2D rigBody;
    private BoxCollider2D col;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;

    private void Awake () {
        rigBody = gameObject.AddComponent<Rigidbody2D> ();
        col = gameObject.AddComponent<BoxCollider2D> ();
        //GameObject parentObj = gameObject.transform.parent.gameObject;
        transform.SetParent (transform.root.parent);
        //Destroy (parentObj);
    }

    public void OnMouseDown () {
        StartCoroutine (Move ());
        /*
        if (BridgeBonesManager.GetInstance ().inputAllowed) {
            print ("Mousedown");
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
        }
        */
    }

    /*
    public void OnMouseDrag () {
        if (BridgeBonesManager.GetInstance ().inputAllowed) {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
        }
    }
    */

    IEnumerator Move () {
        while (true) {
            MoveTowards (goalObject.transform.position);
            yield return null;
        }
    }
    public void MoveTowards (Vector2 pos) {
        rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.1f));
    }
}
