using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeSnapToSocket : MonoBehaviour {
    public enum EndType {
        Left = 0,
        Right = 1
    }

    public Color normalColor, touchColor, snapColor;
    public EndType typeOfEnd;
    public AudioClip boneAttachSfx;
    public BoneBridgeJoint jointTouching, jointAttached;
    public HingeJoint2D hingeAttached;

    private SpriteRenderer spr;

    private void Awake () {
        spr = GetComponent<SpriteRenderer> ();
        spr.color = snapColor;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        BoneBridgeJoint joint = collision.gameObject.GetComponent<BoneBridgeJoint> ();
        if (joint) {
            if (joint.IsJointAvailible(typeOfEnd)) {
                spr.color = touchColor;
                jointTouching = joint;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<BoneBridgeJoint> ()) {
            spr.color = jointAttached ? snapColor : normalColor;
            jointTouching = null;
        }
    }

    public bool TrySnapping() {
        print ("TrySnapping: " + typeOfEnd);
        if (jointTouching) {
            SnapTo (jointTouching);
            return true;
        }
        if (GetComponentInParent<BoneBridgePiece>().isAttached) spr.color = normalColor;
        else spr.color = snapColor;
        return false;
    }

    public void ActivateJoint() {
        spr.color = normalColor;
    }

    private void SnapTo (BoneBridgeJoint joint) {
        transform.parent.position = joint.transform.position + (transform.parent.position - transform.position);
        jointAttached = joint;
        hingeAttached = joint.AddJoint (this);
        BoneBridgeSnapToSocket snap = joint.GetComponent<BoneBridgeSnapToSocket> ();

        if (snap) {
            snap.jointAttached = jointAttached;
            snap.hingeAttached = hingeAttached;
        }

        SoundManager.GetInstance ().PlaySFXClip (boneAttachSfx);
    }
}
