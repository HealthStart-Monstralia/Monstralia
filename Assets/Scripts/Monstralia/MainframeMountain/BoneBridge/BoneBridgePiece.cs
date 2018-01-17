using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgePiece : MonoBehaviour {
    public AudioClip bonePickupSfx;
    public float jointBreakForce = 250f;
    public BoneBridgeSnapToSocket leftJoint, rightJoint;
    public bool isAttached = false;
    public static BoneBridgePiece boneHeld = null;

    private Rigidbody2D rigBody;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;
    private Vector3 startPos;
    private Quaternion startRot;
    private BoxCollider2D col;

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

    private void Awake () {
        rigBody = GetComponent<Rigidbody2D> ();
        col = GetComponent<BoxCollider2D> ();
    }

    void Start() {
        startPos = transform.position;
        startRot = transform.rotation;
        leftJoint.GetComponent<BoneBridgeJoint> ().jointBreakForce = jointBreakForce;
        rightJoint.GetComponent<BoneBridgeJoint> ().jointBreakForce = jointBreakForce;
    }

    public void OnMouseDown () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            boneHeld = this;
            rigBody.gravityScale = 0f;
            rigBody.freezeRotation = true;
            transform.rotation = Quaternion.identity;
            col.enabled = false;

            transform.SetParent (transform.root);
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;

            leftJoint.ActivateJoint ();
            rightJoint.ActivateJoint ();
            SoundManager.GetInstance ().PlaySFXClip (bonePickupSfx);
        }
    }

    public void OnMouseUp () {
        boneHeld = null;
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached) {
            rigBody.gravityScale = 1f;
            rigBody.freezeRotation = false;
            rigBody.velocity = Vector2.zero;
            col.enabled = true;

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

    public void OnMouseDrag () {
        if (BoneBridgeManager.GetInstance ().inputAllowed && !isAttached && boneHeld) {
            cursorPos = Input.mousePosition;
            cursorPos.z -= (Camera.main.transform.position.z + 10f);
            transform.position = Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset;
        }
    }

    public void ResetBone() {
        rigBody.bodyType = RigidbodyType2D.Kinematic;
        rigBody.velocity = Vector2.zero;
        rigBody.angularVelocity = 0f;

        transform.position = startPos;
        transform.rotation = startRot;
        col.enabled = true;
    }
}
