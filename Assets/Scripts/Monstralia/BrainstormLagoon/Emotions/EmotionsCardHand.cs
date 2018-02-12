using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsCardHand : MonoBehaviour {
    public GameObject[] cardLocations;
    public GameObject cardObject;

    [SerializeField] private Transform cardSpawn;
    private List<GameObject> cards = new List<GameObject> ();
    private int slots;
    private Animator animComp;


    void Awake () {
        animComp = GetComponent<Animator> ();

        for (int i = 0; i < cardLocations.Length; i++) {
            cardLocations[i].SetActive (false);
        }

        if (!cardSpawn) {
            cardSpawn = transform;
        }
    }

    public EmotionCard SpawnCard (int iteration, DataType.MonsterEmotions changeToEmotion, Sprite img, Color emoColor, AudioClip audio = null) {
        EmotionCard card = Instantiate (
            cardObject,
            cardSpawn.position, 
            Quaternion.identity, 
        transform.parent).GetComponent<EmotionCard> ();

        cards.Add (card.gameObject);
        card.colorOfCard = emoColor;
        card.ChangeEmotion (changeToEmotion, img, audio);
        card.StartCoroutine (card.MoveToAndFlip(cardLocations[iteration]));
        return card;
    }

    public void RemoveCards () {
        if (cards.Count > 0) {
            for (int i = 0; i < slots; i++) {
                EmotionCard card = cards[0].GetComponent<EmotionCard> ();
                cards.Remove (card.gameObject);
                card.StartCoroutine (card.MoveToAndRemove (EmotionsGameManager.Instance.monsterLocation.gameObject));
            }
        }
    }

    public void SetSlots (int numOfSlots) {
        slots = numOfSlots;
        for (int i = 0; i < numOfSlots; i++) {
            cardLocations[i].SetActive (true);
        }
    }

    public void SpawnIn() {
        animComp.Play ("CardHand_Start", -1, 0f);
    }

    public void ExitAnimation () {
        animComp.Play ("CardHand_End", -1, 0f);
    }
}
