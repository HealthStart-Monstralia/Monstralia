using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeChest : MonoBehaviour {
    public Sprite chestClosed, chestOpened;
    public AudioClip popSfx, rewardSfx;

    private void Awake () {
        GetComponent<SpriteRenderer> ().sprite = chestClosed;
    }

    public void OpenChest() {
        StartCoroutine (Opening ());
    }

    IEnumerator Opening() {
        yield return new WaitForSeconds (1.5f);
        GetComponent<SpriteRenderer> ().sprite = chestOpened;
        SoundManager.GetInstance ().PlaySFXClip (popSfx);
        yield return new WaitForSeconds (0.25f);
        GameObject randomFood = GameManager.GetInstance ().GetComponent<FoodList> ().GetRandomGoodFood ();
        GameObject foodObject = Instantiate (randomFood, transform);
        SoundManager.GetInstance ().PlaySFXClip (rewardSfx);
        foodObject.GetComponent<SpriteRenderer> ().sortingOrder = 3;
        foodObject.transform.localScale = Vector3.one;
    }
}
