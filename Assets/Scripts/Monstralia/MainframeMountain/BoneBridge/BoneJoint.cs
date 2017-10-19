using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneJoint : MonoBehaviour {
    [SerializeField] private HingeJoint2D joint;

    public void AddJoint(GameObject obj) {
        if (!joint) {
            print ("Created joint");
            joint = gameObject.AddComponent<HingeJoint2D> ();
            joint.connectedBody = obj.GetComponent<Rigidbody2D> ();
            joint.breakForce = 1000f;
        }
    }

    private void OnJointBreak2D (Joint2D joint) {
        print ("Joint broke: " + joint);
    }
}
