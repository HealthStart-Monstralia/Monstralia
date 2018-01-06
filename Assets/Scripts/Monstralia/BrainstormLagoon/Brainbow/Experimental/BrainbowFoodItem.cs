using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowFoodItem : MonoBehaviour {
    [HideInInspector] public BrainbowStripe stripeToAttach;
    private Vector3 offset;
    private bool moving = false;
    private Animator anim;

    private void Start () {
        anim = gameObject.GetComponent<Animator> ();
    }
    private void OnEnable () {
        //transform.localPosition = Vector3.zero;
    }

    private void OnMouseDown () {
        moving = true;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
        BrainbowGameManager.GetInstance ().ShowSubtitles (gameObject.name);
        SoundManager.GetInstance ().AddToVOQueue (gameObject.GetComponent<Food> ().clipOfName);
    }

    private void OnMouseUp () {
        moving = false;
        if (stripeToAttach) {
            BrainbowGameManager.GetInstance ().Replace (transform.parent);
            stripeToAttach.MoveItemToSlot (gameObject);
            SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().correctSound);
            gameObject.GetComponent<Collider2D> ().enabled = false;

            if (Random.value < 0.2f) {
                int randomClipIndex = Random.Range (0, BrainbowGameManager.GetInstance ().correctMatchClips.Length);
                SoundManager.GetInstance ().AddToVOQueue (BrainbowGameManager.GetInstance ().correctMatchClips[randomClipIndex]);
            }

        } else {
            MoveBack ();
        }
    }

    void FixedUpdate () {
        if (moving) {
            Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
            gameObject.GetComponent<Rigidbody2D> ().MovePosition (curPosition);
        }
    }

    void MoveBack () {
        gameObject.transform.localPosition = Vector3.zero;
        SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().incorrectSound);
    }

    IEnumerator HideSubtitle () {
        yield return new WaitForSeconds (0.5f);
        BrainbowGameManager.GetInstance ().HideSubtitles ();
    }
}
