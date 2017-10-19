using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToJoint : MonoBehaviour {
    //public CircleCollider2D col;
    public Color normalColor, snapColor;
    private bool snappedTo;
    private SpriteRenderer spr;

    private void Awake () {
        spr = GetComponent<SpriteRenderer> ();
        spr.color = normalColor;
        snappedTo = false;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<BoneJoint> ()) {
            spr.color = snapColor;
            /*
            if (!snappedTo) {
                SnapTo (collision.gameObject);
                snappedTo = true;
            }
            */
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        print ("Exited trigger area");
        spr.color = normalColor;
    }

    private void SnapTo(GameObject obj) {
        transform.position = obj.transform.position;
        BoneJoint joint = obj.GetComponent<BoneJoint> ();
        joint.AddJoint (gameObject);
    }
}
