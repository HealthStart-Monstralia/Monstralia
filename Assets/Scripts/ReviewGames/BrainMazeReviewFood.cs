using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainMazeReviewFood : MonoBehaviour {

    Image myImage;
    AudioSource audioSource;
    public Sprite[] goodFoods, badFoods;
    public bool isGood;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        myImage = GetComponent<Image>();
        if (isGood == true) {
            myImage.sprite = goodFoods[Random.Range(0, goodFoods.Length)];
        } else {
            myImage.sprite = badFoods[Random.Range(0, badFoods.Length)];
        }

        if (isGood == false) {
            GetComponent<CircleCollider2D>().isTrigger = false;
            gameObject.AddComponent<Rigidbody2D>();
            Rigidbody2D myRigidbody = GetComponent<Rigidbody2D>();
            myRigidbody.gravityScale = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {
 
        if (col.GetComponent<BrainMazeReviewMonster>()) {
            audioSource.Play();
            Destroy(GetComponent<Image>());
            Destroy(GetComponent<CircleCollider2D>());
        }
    }
}
