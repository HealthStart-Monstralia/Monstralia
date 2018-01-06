using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowStripe : MonoBehaviour {
    public Colorable.Color stripeColor;
    public Transform[] foodSlots;

    private int slotIndex = 0;
    private BrainbowFoodItem detectedFood;

    private void Awake () {
        transform.parent.gameObject.SetActive (false);
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        detectedFood = collision.GetComponent<BrainbowFoodItem> ();
        if (detectedFood) {
            Colorable.Color foodColor = collision.GetComponent<Food> ().color;
            if (foodColor == stripeColor) {
                detectedFood.stripeToAttach = this;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (detectedFood && detectedFood.stripeToAttach == this)
            detectedFood.stripeToAttach = null;
    }

    public void MoveItemToSlot (GameObject item) {
        item.transform.SetParent (foodSlots[slotIndex]);
        item.transform.localPosition = Vector3.zero;
        if (slotIndex < 5)
            slotIndex++;
    }

    public void ClearStripe() {
        foreach (Transform slot in foodSlots) {
            GameObject foodItem = slot.GetChild (0).gameObject;
            if (foodItem) Destroy (foodItem);
        }
    }
}
