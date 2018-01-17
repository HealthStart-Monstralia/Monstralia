using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewBrainbowStripe : MonoBehaviour {
    public Colorable.Color stripeColor;
    public Transform foodSlot;
    [HideInInspector] public ReviewBrainbowFood detectedFood;

    private void Awake () {
        transform.gameObject.SetActive (false);
    }

    public void MoveItemToSlot (GameObject item) {
        item.transform.SetParent (foodSlot);
        item.transform.localPosition = Vector3.zero;
        ReviewBrainbow.GetInstance ().IncreaseNumOfFilledSlots ();
    }

    public void ClearStripe () {
        if (foodSlot.childCount > 0) {
            GameObject foodItem = foodSlot.GetChild (0).gameObject;
            if (foodItem) Destroy (foodItem);
        }
    }
}
