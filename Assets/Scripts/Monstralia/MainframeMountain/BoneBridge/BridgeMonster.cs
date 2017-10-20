using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMonster : MonoBehaviour {

    public GameObject goalObject;

    private Rigidbody2D rigBody;
    private BoxCollider2D col;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;
    public bool tapToMove;

    private void Awake () {
        rigBody = gameObject.AddComponent<Rigidbody2D> ();
        rigBody.freezeRotation = true;
        rigBody.mass = 10;
        rigBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        transform.SetParent (transform.root.parent);

    }

    public void OnMouseDown () {
        if (BoneBridgeManager.GetInstance ().inputAllowed) {
            if (tapToMove) {
                Stop ();
                BoneBridgeManager.GetInstance ().CameraSwitch (gameObject);
                StartCoroutine (Move ());
            }
        }
    }

    IEnumerator Move () {
        Vector2 dir;
        while (true) {
            dir = goalObject.transform.position - transform.position;
            Debug.DrawLine (transform.position, goalObject.transform.position);
            MoveTowards (dir);
            yield return null;
        }
    }

    public void Stop() {
        print ("Stopping");
        StopAllCoroutines ();
    }

    public void MoveTowards (Vector2 pos) {
        rigBody.AddForce (pos * 2f);
    }
}
