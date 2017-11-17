using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeJoint : MonoBehaviour {
    public float jointBreakForce = 250f;
    public bool rejectLeftEnd, rejectRightEnd;
    public AudioClip boneDetachSfx;
    public HingeJoint2D boneJoint;

    public HingeJoint2D AddJoint(GameObject obj, BoneBridgeSnapToSocket.EndType typeOfEnd) {
        switch (typeOfEnd) {
            case BoneBridgeSnapToSocket.EndType.Left:
                return !boneJoint ? CreateJoint (obj, typeOfEnd, -90f, 90f) : null;
            case BoneBridgeSnapToSocket.EndType.Right:
                return !boneJoint ? CreateJoint (obj, typeOfEnd, 90f, -90f) : null;
            default:
                return null;
        }
    }

    HingeJoint2D CreateJoint (GameObject obj, BoneBridgeSnapToSocket.EndType typeOfEnd, float lowerAngle, float upperAngle) {
        if (GetComponent<Rigidbody2D> ()) {
            boneJoint = gameObject.AddComponent<HingeJoint2D> ();
            if (typeOfEnd == BoneBridgeSnapToSocket.EndType.Left) {
                obj.GetComponent<BoneBridgePiece> ().leftJoint.GetComponent<BoneBridgeJoint> ().boneJoint = boneJoint;
            } else {
                obj.GetComponent<BoneBridgePiece> ().rightJoint.GetComponent<BoneBridgeJoint> ().boneJoint = boneJoint;
            }
        } else {
            boneJoint = transform.parent.gameObject.AddComponent<HingeJoint2D> ();
            if (typeOfEnd == BoneBridgeSnapToSocket.EndType.Left) {
                boneJoint.anchor = new Vector2 (2f, boneJoint.anchor.y);
                obj.GetComponent<BoneBridgePiece> ().leftJoint.GetComponent<BoneBridgeJoint> ().boneJoint = boneJoint;
            } else {
                boneJoint.anchor = new Vector2 (-2f, boneJoint.anchor.y);
                obj.GetComponent<BoneBridgePiece> ().rightJoint.GetComponent<BoneBridgeJoint> ().boneJoint = boneJoint;
            }
        }
        Rigidbody2D rigBody = obj.GetComponent<Rigidbody2D> ();
        rigBody.velocity = Vector2.zero;
        boneJoint.breakForce = jointBreakForce;
        boneJoint.connectedBody = rigBody;


        JointAngleLimits2D limits = boneJoint.limits;
        limits.min = lowerAngle;
        limits.max = upperAngle;
        boneJoint.limits = limits;
        boneJoint.useLimits = true;
        print ("Created " + boneJoint + " with: " + obj);
        return boneJoint;
    }

    public bool IsJointAvailible(BoneBridgeSnapToSocket.EndType end) {
        switch (end) {
            case BoneBridgeSnapToSocket.EndType.Left:
                return (!rejectLeftEnd && !boneJoint) ? true : false;
            case BoneBridgeSnapToSocket.EndType.Right:
                return (!rejectRightEnd && !boneJoint) ? true : false;
            default:
                return false;
        }
    }

    public void DestroyJoint() {
        if (boneJoint) {
            print ("Firing Destroy Joint");
            Destroy (boneJoint);
            boneJoint = null;
        }
    }

    private void OnJointBreak2D (Joint2D joint) {
        print ("Joint broke: " + joint);
        DestroyJoint ();
        SoundManager.GetInstance ().PlaySFXClip (boneDetachSfx);
    }
}
