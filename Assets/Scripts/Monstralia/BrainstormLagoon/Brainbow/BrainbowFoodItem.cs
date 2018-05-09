using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowFoodItem : MonoBehaviour {
    [HideInInspector] public BrainbowStripe stripeToAttach;

    private Vector3 offset;
    private Rigidbody2D rigBody;
    private bool isMoved = false;
    private bool isBeingEaten = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] float t;

    private int moveBackID;

    private void Awake () {
        rigBody = gameObject.GetComponent<Rigidbody2D> ();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
    }

    private void OnEnable () {
        BrainbowGameManager.OnGameEnd += GetEaten;
    }

    private void OnDisable () {
        if (isBeingEaten) {
            //SoundManager.Instance.PlaySFXClip (BrainbowGameManager.Instance.munchSound);
            if (BrainbowGameManager.Instance) {
                if (BrainbowGameManager.Instance.activeFoods.Contains (this))
                    BrainbowGameManager.Instance.activeFoods.Remove (this);
            }
        }
        BrainbowGameManager.OnGameEnd -= GetEaten;
    }

    private void OnMouseDown () {
        if (BrainbowGameManager.Instance.inputAllowed) {
            LeanTween.cancel (moveBackID);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
            AudioClip clip = gameObject.GetComponent<Food> ().clipOfName;
            SubtitlePanel.Instance.Display (gameObject.name, null, true);
            if (!SoundManager.Instance.DoesQueueContainClip (clip)) {
                SoundManager.Instance.AddToVOQueue (clip);
            }
            spriteRenderer.sortingOrder = 6;
        }
    }

    private void OnMouseUp () {
        if (isMoved) {
            if (stripeToAttach) {
                BrainbowGameManager.Instance.ScoreAndReplace (transform.parent);
                SoundManager.Instance.PlaySFXClip (BrainbowGameManager.Instance.correctSound);
                InsertItemIntoStripe (stripeToAttach);

                // Random praise VO line
                if (Random.value < 0.2f && !BrainbowGameManager.Instance.tutorialManager.isRunningTutorial) {
                    int randomClipIndex = Random.Range (0, BrainbowGameManager.Instance.correctMatchClips.Length);
                    SoundManager.Instance.AddToVOQueue (BrainbowGameManager.Instance.correctMatchClips[randomClipIndex]);
                }

            } else {
                MoveBack ();
            }
            isMoved = false;
        }
        spriteRenderer.sortingOrder = 3;
    }

    void OnMouseDrag () {
        if (BrainbowGameManager.Instance.inputAllowed) {
            if (!isMoved)
                isMoved = true;
            if (isMoved) {
                Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
                rigBody.MovePosition (curPosition);
            }
        }
    }

    void MoveBack () {
        // gameObject.transform.localPosition = Vector3.zero;
        moveBackID = LeanTween.move (gameObject, gameObject.transform.parent.position, 0.5f).setEaseOutExpo ().id;
        SoundManager.Instance.PlaySFXClip (BrainbowGameManager.Instance.incorrectSound);
    }

    public void InsertItemIntoStripe (BrainbowStripe stripe) {
        stripe.MoveItemToSlot (gameObject);
        gameObject.GetComponent<Collider2D> ().enabled = false;
        BrainbowGameManager.Instance.activeFoods.Add (this);
    }

    public void GetEaten() {
        if (BrainbowGameManager.Instance.DidPlayerWin ())
            StartCoroutine (MoveToMonster ());
    }

    IEnumerator MoveToMonster () {
        isBeingEaten = true;
        gameObject.GetComponent<Collider2D> ().enabled = true;
        transform.SetParent (transform.root);
        yield return new WaitForSeconds (0.5f * (Random.Range (0.8f, 1.5f)) );

        for (t = 0.0f; t < 0.1f; t += Time.deltaTime * 0.1f) {
            transform.position = Vector2.Lerp (transform.position, BrainbowGameManager.Instance.monsterObject.transform.position, t);
            yield return null;
        }

        GetComponent<Food> ().EatFood ();
    }

}
