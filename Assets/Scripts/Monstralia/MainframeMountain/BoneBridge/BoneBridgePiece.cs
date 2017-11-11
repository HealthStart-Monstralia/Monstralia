using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgePiece : PhysicsDrag {
    public AudioClip bonePickupSfx;
    public float jointBreakForce = 250f;
    public SnapToJoint leftJoint, rightJoint;
    public bool isAttached = false;

    private Vector3 startPos;
    private Quaternion startRot;

    private void OnEnable () {
        BoneBridgeManager.PhaseChange += OnPhaseChange;
    }

    private void OnDisable () {
        BoneBridgeManager.PhaseChange -= OnPhaseChange;
    }

    void OnPhaseChange (BoneBridgeManager.BridgePhase phase) {
        //print ("BoneBridgePiece OnPhaseChange firing: " + phase);
        switch (phase) {
            case BoneBridgeManager.BridgePhase.Start:
                rigBody.bodyType = RigidbodyType2D.Kinematic;
                break;
            case BoneBridgeManager.BridgePhase.Building:
                //rigBody.bodyType = RigidbodyType2D.Kinematic;
                break;
            case BoneBridgeManager.BridgePhase.Crossing:
                if (isAttached)
                    rigBody.bodyType = RigidbodyType2D.Dynamic;
                break;
            case BoneBridgeManager.BridgePhase.Finish:
                if (isAttached)
                    rigBody.bodyType = RigidbodyType2D.Dynamic;
                break;
        }
    }

    void Start() {
        startPos = transform.position;
        startRot = transform.rotation;
        leftJoint.GetComponent<BoneJoint> ().jointBreakForce = jointBreakForce;
        rightJoint.GetComponent<BoneJoint> ().jointBreakForce = jointBreakForce;
        //OnPhaseChange(BoneBridgeManager.BridgePhase.Building);
    }

    public new void OnMouseDown () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            base.OnMouseDown ();
            leftJoint.ActivateJoint ();
            rightJoint.ActivateJoint ();
            SoundManager.GetInstance ().PlaySFXClip (bonePickupSfx);
        }
    }

    public new void OnMouseUp () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            base.OnMouseUp ();
            if (leftJoint.TrySnapping ())
                isAttached = true;
            if (rightJoint.TrySnapping ())
                isAttached = true;
            if (!isAttached) {
                transform.position = startPos;
                transform.rotation = startRot;
            }
            print ("isAttached: " + isAttached);
        }
    }

    public new void OnMouseDrag () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            base.OnMouseDrag ();
        }
    }
}
