using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowFoodItem : MonoBehaviour {
    [HideInInspector] public BrainbowStripe stripeToAttach;
    private Vector3 offset;
    private Rigidbody2D rigBody;
    private bool moving = false;
    private bool isPlaced = false;
    private bool isBeingEaten = false;

    private void Awake () {
        rigBody = gameObject.GetComponent<Rigidbody2D> ();
    }

    private void OnDestroy () {
        if (isBeingEaten)
            SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().munchSound);
        if (BrainbowGameManager.GetInstance ().activeFoods.Contains (this))
            BrainbowGameManager.GetInstance ().activeFoods.Remove (this);
    }

    private void OnMouseDown () {
        if (BrainbowGameManager.GetInstance().inputAllowed) {
            moving = true;
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
            BrainbowGameManager.GetInstance ().ShowSubtitles (gameObject.name);
            SoundManager.GetInstance ().AddToVOQueue (gameObject.GetComponent<Food> ().clipOfName);
        }
    }

    private void OnMouseUp () {
        if (moving) {
            if (stripeToAttach) {
                BrainbowGameManager.GetInstance ().ScoreAndReplace (transform.parent);
                SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().correctSound);
                InsertItemIntoStripe (stripeToAttach);

                // Random praise VO line
                if (Random.value < 0.2f) {
                    int randomClipIndex = Random.Range (0, BrainbowGameManager.GetInstance ().correctMatchClips.Length);
                    SoundManager.GetInstance ().AddToVOQueue (BrainbowGameManager.GetInstance ().correctMatchClips[randomClipIndex]);
                }

            } else {
                MoveBack ();
            }
        }
        moving = false;
    }

    void FixedUpdate () {
        if (moving) {
            Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
            rigBody.MovePosition (curPosition);
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

    public void InsertItemIntoStripe (BrainbowStripe stripe) {
        stripe.MoveItemToSlot (gameObject);
        gameObject.GetComponent<Collider2D> ().enabled = false;
        isPlaced = true;
        BrainbowGameManager.GetInstance ().activeFoods.Add (this);
    }

    public IEnumerator GetEaten () {
        isBeingEaten = true;
        gameObject.GetComponent<Collider2D> ().enabled = true;
        yield return new WaitForSeconds (0.5f);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 1f) {
            transform.position = Vector2.MoveTowards (transform.position, new Vector3 (0f, transform.position.y, 0f), t * 0.3f);
            Debug.DrawLine (transform.position, new Vector3 (0f, transform.position.y, 0f), Color.yellow);
            yield return new WaitForFixedUpdate ();
        }
        rigBody.bodyType = RigidbodyType2D.Dynamic;
        rigBody.gravityScale = 2.0f;
        yield return new WaitForSeconds (2f);
        Destroy (gameObject);
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Monster" && isPlaced) {
            Destroy (gameObject);
        }
    }
}
