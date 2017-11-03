using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToJoint : MonoBehaviour {
    public enum EndType {
        Left = 0,
        Right = 1
    }

    public Color normalColor, touchColor, snapColor;
    public EndType typeOfEnd;
    public AudioClip boneAttachSfx, boneDetachSfx;

    private CircleCollider2D col;
    [SerializeField] private BoneJoint jointTouching;
    public HingeJoint2D jointAttached;
    private SpriteRenderer spr;

    private void Awake () {
        spr = GetComponent<SpriteRenderer> ();
        col = GetComponent<CircleCollider2D> ();
        spr.color = normalColor;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        BoneJoint joint = collision.gameObject.GetComponent<BoneJoint> ();
        if (joint) {
            if (joint.IsJointAvailible(typeOfEnd)) {
                spr.color = touchColor;
                jointTouching = joint;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<BoneJoint> ()) {
            spr.color = jointAttached ? snapColor : normalColor;
            jointTouching = null;
        }
    }

    public void TrySnapping() {
        if (jointTouching) SnapTo (jointTouching);
    }

    public void Detach() {
        if (jointAttached) {
            jointAttached.GetComponent<BoneJoint>().DestroyJoint();
            //Destroy (jointAttached);
            jointAttached = null;
            spr.color = normalColor;
            SoundManager.GetInstance ().PlaySFXClip (boneDetachSfx);
        }
    }

    private void SnapTo (BoneJoint joint) {
        transform.parent.position = joint.transform.position + (transform.parent.position - transform.position);
        jointAttached = joint.AddJoint (transform.parent.gameObject, typeOfEnd);
        SoundManager.GetInstance ().PlaySFXClip (boneAttachSfx);
    }
}
