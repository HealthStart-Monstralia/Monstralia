using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsCardHand : MonoBehaviour {
    public GameObject[] cardLocations;
    private Animator animComp;


    void Awake () {
        animComp = GetComponent<Animator> ();

        for (int i = 0; i < cardLocations.Length; i++) {
            cardLocations[i].SetActive (false);
        }
    }

    public void PutCardInSlot (EmotionCard card, int slotNum) {
        card.StartCoroutine (card.MoveToAndFlip (cardLocations[slotNum].transform));
    }

    public EmotionCard GetCardInSlot (int slotNum) {
        EmotionCard card = GetComponentInChildren<EmotionCard> ();
        return card;
    }

    public void SetSlots (int numOfSlots) {
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
