using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneJoint : MonoBehaviour {
    public float jointBreakForce = 250f;
    public bool rejectLeftEnd, rejectRightEnd;

    [SerializeField] private HingeJoint2D joint;

    public HingeJoint2D AddJoint(GameObject obj, SnapToJoint.EndType typeOfEnd) {
        switch (typeOfEnd) {
            case SnapToJoint.EndType.Left:
                return !joint ? CreateJoint (obj, typeOfEnd, -90f, 90f) : null;
            case SnapToJoint.EndType.Right:
                return !joint ? CreateJoint (obj, typeOfEnd, 90f, -90f) : null;
            default:
                return null;
        }
    }

    HingeJoint2D CreateJoint (GameObject obj, SnapToJoint.EndType typeOfEnd, float lowerAngle, float upperAngle) {
        if (GetComponent<Rigidbody2D>())
            joint = gameObject.AddComponent<HingeJoint2D> ();
        else {
            joint = transform.parent.gameObject.AddComponent<HingeJoint2D> ();
            if (typeOfEnd == SnapToJoint.EndType.Left)
                joint.anchor = new Vector2 (2f, joint.anchor.y);
            else
                joint.anchor = new Vector2 (-2f, joint.anchor.y);
        }
        Rigidbody2D rigBody = obj.GetComponent<Rigidbody2D> ();
        rigBody.velocity = Vector2.zero;
        joint.breakForce = jointBreakForce;
        joint.connectedBody = rigBody;

        JointAngleLimits2D limits = joint.limits;
        limits.min = lowerAngle;
        limits.max = upperAngle;
        joint.limits = limits;
        joint.useLimits = true;
        print ("Created " + joint + " with: " + obj);
        return joint;
    }

    public bool IsJointAvailible(SnapToJoint.EndType end) {
        switch (end) {
            case SnapToJoint.EndType.Left:
                return (!rejectLeftEnd && !joint) ? true : false;
            case SnapToJoint.EndType.Right:
                return (!rejectRightEnd && !joint) ? true : false;
            default:
                return false;
        }
    }

    public void DestroyJoint() {
        if (joint) {
            Destroy (joint);
            joint = null;
        }
    }

    private void OnJointBreak2D (Joint2D joint) {
        print ("Joint broke: " + joint);
        SnapToJoint snap = joint.connectedBody.GetComponent<SnapToJoint>();
        if (snap) snap.Detach ();
    }
}
