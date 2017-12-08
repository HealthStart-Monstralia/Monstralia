using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgePiece : PhysicsDrag {
    public AudioClip bonePickupSfx;
    public float jointBreakForce = 250f;
    public BoneBridgeSnapToSocket leftJoint, rightJoint;
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
        switch (phase) {
            case BoneBridgeManager.BridgePhase.Start:
                rigBody.bodyType = RigidbodyType2D.Kinematic;
                break;
            case BoneBridgeManager.BridgePhase.Building:
                break;
            case BoneBridgeManager.BridgePhase.Falling:
                if (isAttached) {
                    rigBody.bodyType = RigidbodyType2D.Dynamic;
                    if (leftJoint.jointAttached)
                        leftJoint.jointAttached.DestroyJoint ();
                    if (rightJoint.jointAttached)
                        rightJoint.jointAttached.DestroyJoint ();
                    GetComponent<BoxCollider2D> ().enabled = false;
                    isAttached = false;
                    Invoke ("ResetBone", 3f);
                }
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
        leftJoint.GetComponent<BoneBridgeJoint> ().jointBreakForce = jointBreakForce;
        rightJoint.GetComponent<BoneBridgeJoint> ().jointBreakForce = jointBreakForce;
        //OnPhaseChange(BoneBridgeManager.BridgePhase.Building);
    }

    public new void OnMouseDown () {
        print ("MouseDown");
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
            if (leftJoint.TrySnapping ()) {
                isAttached = true;
                print (gameObject + " isAttached: " + isAttached);
            }
            if (rightJoint.TrySnapping ()) {
                isAttached = true;
                print (gameObject + "isAttached: " + isAttached);
            }
            if (!isAttached) {
                ResetBone ();
            }
        }
    }

    public new void OnMouseDrag () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            base.OnMouseDrag ();
        }
    }

    public void ResetBone() {
        rigBody.bodyType = RigidbodyType2D.Kinematic;
        rigBody.velocity = Vector2.zero;
        rigBody.angularVelocity = 0f;
        transform.position = startPos;
        transform.rotation = startRot;
        leftJoint.ActivateJoint ();
        rightJoint.ActivateJoint ();
        GetComponent<BoxCollider2D> ().enabled = true;
    }
}
