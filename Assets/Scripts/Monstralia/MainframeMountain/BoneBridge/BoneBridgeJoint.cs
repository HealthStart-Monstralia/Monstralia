using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeJoint : MonoBehaviour {
    public BoneBridgeSnapToSocket.EndType acceptedType;
    public float jointBreakForce = 250f;
    public AudioClip boneDetachSfx;
    public HingeJoint2D boneJoint;

    public HingeJoint2D AddJoint(BoneBridgeSnapToSocket socket) {
        switch (socket.typeOfEnd) {
            case BoneBridgeSnapToSocket.EndType.Left:
                return !boneJoint ? CreateJoint (socket, -90f, 90f, 2f) : null;
            case BoneBridgeSnapToSocket.EndType.Right:
                return !boneJoint ? CreateJoint (socket, 90f, -90f, -2f) : null;
            default:
                return null;
        }
    }

    HingeJoint2D CreateJoint (BoneBridgeSnapToSocket socket, float lowerAngle, float upperAngle, float anchorOffset) {
        if (GetComponent<Rigidbody2D> ()) {
            boneJoint = gameObject.AddComponent<HingeJoint2D> ();
        } else {
            boneJoint = transform.parent.gameObject.AddComponent<HingeJoint2D> ();
            boneJoint.anchor = new Vector2 (anchorOffset, boneJoint.anchor.y);
        }

        socket.GetComponent<BoneBridgeJoint> ().boneJoint = boneJoint;

        Rigidbody2D rigBody = socket.transform.parent.GetComponent<Rigidbody2D> ();
        rigBody.velocity = Vector2.zero;
        boneJoint.breakForce = jointBreakForce;
        boneJoint.connectedBody = rigBody;

        JointAngleLimits2D limits = boneJoint.limits;
        limits.min = lowerAngle;
        limits.max = upperAngle;
        boneJoint.limits = limits;
        boneJoint.useLimits = true;
        return boneJoint;
    }

    public bool IsJointAvailible(BoneBridgeSnapToSocket.EndType end) {
        return (end == acceptedType && !boneJoint) ? true : false;
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
        SoundManager.Instance.PlaySFXClip (boneDetachSfx);
    }
}