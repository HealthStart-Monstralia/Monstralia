using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainMazeReviewFood : MonoBehaviour {
    public AudioClip pickupSfx;
    public Sprite[] goodFoods, badFoods;
    public bool isGood;

    private void Start() {
        Image myImage = GetComponent<Image>();

        if (isGood) {
            int num = Random.Range (0, goodFoods.Length);
            myImage.sprite = goodFoods[num];
            gameObject.name = goodFoods[num].name;
        } else {
            int num = Random.Range (0, badFoods.Length);
            myImage.sprite = badFoods[num];
            gameObject.name = badFoods[num].name;
        }

        if (!isGood) {
            GetComponent<CircleCollider2D>().isTrigger = false;
            //gameObject.AddComponent<Rigidbody2D>();
            //Rigidbody2D myRigidbody = GetComponent<Rigidbody2D>();
            //myRigidbody.gravityScale = 0;
        }
    }

    void OnTriggerEnter2D (Collider2D col) {
        if (col.GetComponent<ReviewBrainMazeMonster> ()) {
            SoundManager.Instance.PlaySFXClip (pickupSfx);
            gameObject.SetActive (false);
        }
    }
}
