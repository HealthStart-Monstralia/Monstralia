using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowFoodPanel : MonoBehaviour {
    public Transform[] slots;

    private void Awake () {
        TurnOnNumOfSlots (0);
    }

    private void OnEnable () {
        Activate ();
    }

    public void TurnOnNumOfSlots(int slotsToTurnOn) {
        foreach (Transform slot in slots) {
            slot.gameObject.SetActive (false);
        }

        if (slotsToTurnOn > slots.Length)
            slotsToTurnOn = slots.Length;

        for (int i = 0; i < slotsToTurnOn; i++) {
            slots[i].gameObject.SetActive (true);
        }
    }

    public GameObject CreateItemAtSlot(GameObject item, Transform slot) {
        GameObject newFood = Instantiate (item, slot);
        return newFood;
    }

    public void Activate() {
        GetComponent<Animator> ().Play ("FoodPanelFadeIn", -1, 0f);
    }

    public void Deactivate () {
        GetComponent<Animator> ().Play ("FoodPanelFadeOut", -1, 0f);
    }

    // For animator
    public void DeactivateGameObject() {
        gameObject.SetActive (false);
    }
}
