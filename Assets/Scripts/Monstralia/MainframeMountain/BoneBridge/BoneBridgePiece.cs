using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgePiece : PhysicsDrag {
    public AudioClip bonePickupSfx;
    [SerializeField] private SnapToJoint leftJoint, rightJoint;

    private void OnEnable () {
        BoneBridgeManager.PhaseChange += OnPhaseChange;
    }

    private void OnDisable () {
        BoneBridgeManager.PhaseChange -= OnPhaseChange;
    }

    void OnPhaseChange (BoneBridgeManager.BridgePhase phase) {
        print ("BoneBridgePiece OnPhaseChange firing: " + phase);
        switch (phase) {
            case BoneBridgeManager.BridgePhase.Start:
                rigBody.bodyType = RigidbodyType2D.Kinematic;
                break;
            case BoneBridgeManager.BridgePhase.Building:
                rigBody.bodyType = RigidbodyType2D.Kinematic;
                break;
            case BoneBridgeManager.BridgePhase.Crossing:
                rigBody.bodyType = RigidbodyType2D.Dynamic;
                break;
            case BoneBridgeManager.BridgePhase.Finish:
                rigBody.bodyType = RigidbodyType2D.Dynamic;
                break;
        }
    }

    void Start() {
        //OnPhaseChange(BoneBridgeManager.BridgePhase.Building);
    }

    public new void OnMouseDown () {
        print ("OnMouseDown");
        base.OnMouseDown ();
        leftJoint.Detach ();
        rightJoint.Detach ();
        SoundManager.GetInstance ().PlaySFXClip (bonePickupSfx);
        if (BoneBridgeManager.GetInstance ().inputAllowed) {

        }
    }

    public new void OnMouseUp () {
        print ("OnMouseUp");
        base.OnMouseUp ();
        leftJoint.TrySnapping ();
        rightJoint.TrySnapping ();
        if (BoneBridgeManager.GetInstance ().inputAllowed) {

        }
    }

    public new void OnMouseDrag () {
        if (BoneBridgeManager.GetInstance ().inputAllowed) {
            base.OnMouseDrag ();
        }
    }
}
