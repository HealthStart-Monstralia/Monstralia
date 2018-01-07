using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowStripe : MonoBehaviour {
    public Colorable.Color stripeColor;
    public Transform[] foodSlots;
    [HideInInspector] public BrainbowFoodItem detectedFood;

    private int slotIndex = 0;

    private void Awake () {
        transform.gameObject.SetActive (false);
    }

    public void MoveItemToSlot (GameObject item) {
        item.transform.SetParent (foodSlots[slotIndex]);
        item.transform.localPosition = Vector3.zero;
        if (slotIndex < 5)
            slotIndex++;
    }

    public void ClearStripe() {
        foreach (Transform slot in foodSlots) {
            if (slot.childCount > 0) {
                GameObject foodItem = slot.GetChild (0).gameObject;
                if (foodItem) Destroy (foodItem);
            }
        }
        slotIndex = 0;
    }
}
