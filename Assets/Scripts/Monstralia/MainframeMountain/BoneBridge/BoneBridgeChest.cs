using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeChest : MonoBehaviour {
    public Sprite chestClosed, chestOpened;
    public AudioClip popSfx, rewardSfx;

    private void Awake () {
        GetComponent<SpriteRenderer> ().sprite = chestClosed;
    }

    private void Start () {
    }

    public void OpenChest() {
        StartCoroutine (Opening ());
    }

    IEnumerator Opening() {
        print ("Coroutine started");
        yield return new WaitForSeconds (1.5f);
        GetComponent<SpriteRenderer> ().sprite = chestOpened;
        SoundManager.Instance.PlaySFXClip (popSfx);
        yield return new WaitForSeconds (0.25f);
        GameObject randomFood = FoodList.GetRandomGoodFood ();
        GameObject foodObject = Instantiate (randomFood, transform.position + new Vector3 (0f, 0f, 5f), Quaternion.identity, transform);

        SoundManager.Instance.PlaySFXClip (rewardSfx);
        foodObject.GetComponent<SpriteRenderer> ().sortingOrder = 3;
        foodObject.transform.localScale = Vector3.one * 0.2f;
        Rigidbody2D rigBody = foodObject.GetComponent<Rigidbody2D> ();
        foodObject.GetComponent<Collider2D> ().isTrigger = false;
        rigBody.bodyType = RigidbodyType2D.Dynamic;
        rigBody.gravityScale = 1f;
        rigBody.AddForce (Vector2.up * 50f);
    }
}
