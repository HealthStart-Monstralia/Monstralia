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
    [SerializeField] private BoneJoint jointTouching, jointSnappedTo;
    private SpriteRenderer spr;

    private void Awake () {
        spr = GetComponent<SpriteRenderer> ();
        col = GetComponent<CircleCollider2D> ();
        spr.color = normalColor;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<BoneJoint> ()) {
            spr.color = touchColor;
            jointTouching = collision.GetComponent<BoneJoint> ();
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.GetComponent<BoneJoint> ()) {
            spr.color = jointSnappedTo ? snapColor : normalColor;
            jointTouching = null;
        }
    }

    public void TrySnapping() {
        if (jointTouching) SnapTo (jointTouching);
    }

    public void Detach() {
        if (jointSnappedTo) {
            Destroy (jointSnappedTo.GetComponent<HingeJoint2D>());
            jointSnappedTo = null;
            spr.color = normalColor;
            SoundManager.GetInstance ().PlaySFXClip (boneDetachSfx);
        }
    }

    private void SnapTo(BoneJoint joint) {
        transform.position = joint.transform.position;
        joint.AddJoint (gameObject, typeOfEnd);
        jointSnappedTo = jointTouching;
        SoundManager.GetInstance ().PlaySFXClip (boneAttachSfx);
    }
}
