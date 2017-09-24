using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsCardHand : MonoBehaviour {
    public GameObject[] cardLocations;
    public GameObject cardObject;

    private List<GameObject> cards = new List<GameObject> ();
    private int slots;
    private Animator animComp;


    void Awake () {
        animComp = GetComponent<Animator> ();

        for (int i = 0; i < cardLocations.Length; i++) {
            cardLocations[i].SetActive (false);
        }

        slots = GameManager.GetInstance ().GetLevel (DataType.Minigame.MonsterEmotions) + 1;

        for (int i = 0; i < slots; i++) {
            cardLocations[i].SetActive (true);
        }
    }

    public void SpawnCard (int iteration, EmotionsGameManager.MonsterEmotions changeToEmotion, Sprite img, AudioClip audio) {
        EmotionCard card = Instantiate (cardObject, Vector2.zero, Quaternion.identity, transform.parent).GetComponent<EmotionCard> ();
        cards.Add (card.gameObject);
        card.ChangeEmotion (changeToEmotion, img, audio);
        card.StartCoroutine (card.MoveToAndFlip(cardLocations[iteration]));
    }

    public void RemoveCards () {
        if (cards.Count > 0) {
            for (int i = 0; i < slots; i++) {
                EmotionCard card = cards[0].GetComponent<EmotionCard> ();
                cards.Remove (card.gameObject);
                card.StartCoroutine (card.MoveToAndRemove (EmotionsGameManager.GetInstance ().monsterLocation.gameObject));
            }
        }
    }

    public void SpawnIn() {
        animComp.Play ("CardHand_Start", -1, 0f);
    }

    public void ExitAnimation () {
        animComp.Play ("CardHand_End", -1, 0f);
    }
}
