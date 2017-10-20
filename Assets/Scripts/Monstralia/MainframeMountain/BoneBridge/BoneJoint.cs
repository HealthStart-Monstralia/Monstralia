using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneJoint : MonoBehaviour {
    public float jointBreakForce = 250f;

    public void AddJoint(GameObject obj, SnapToJoint.EndType typeOfEnd) {
        float anchorDistanceX = 0f;

        switch (typeOfEnd) {
            case SnapToJoint.EndType.Left:
                anchorDistanceX = -1.5f;
                break;
            case SnapToJoint.EndType.Right:
                anchorDistanceX = 1.5f;
                break;
        }

        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D> ();
        print ("Created joint with: " + obj);
        joint.anchor = new Vector2 (anchorDistanceX, joint.anchor.y);
        joint.connectedBody = obj.GetComponent<Rigidbody2D> ();
        joint.breakForce = jointBreakForce;
    }

    private void OnJointBreak2D (Joint2D joint) {
        print ("Joint broke: " + joint);
        joint.connectedBody.GetComponent<SnapToJoint> ().Detach ();
    }
}
