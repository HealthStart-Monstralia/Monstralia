using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgePiece : MonoBehaviour {
    public AudioClip bonePickupSfx;
    [SerializeField] private SnapToJoint leftJoint, rightJoint;

    public void OnMouseDown () {
        leftJoint.Detach ();
        rightJoint.Detach ();
        SoundManager.GetInstance ().PlaySFXClip (bonePickupSfx);
    }

    public void OnMouseUp () {
        leftJoint.TrySnapping ();
        rightJoint.TrySnapping ();
    }
}
