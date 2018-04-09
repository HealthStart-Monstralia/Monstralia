using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowStripeCollider : MonoBehaviour {
    public BrainbowStripe stripe;

    private void OnTriggerEnter2D (Collider2D collision) {
        stripe.detectedFood = collision.GetComponent<BrainbowFoodItem> ();
        if (stripe.detectedFood) {
            DataType.Color foodColor = collision.GetComponent<Food> ().typeOfColor;
            if (foodColor == stripe.stripeColor) {
                stripe.detectedFood.stripeToAttach = stripe;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (stripe.detectedFood && stripe.detectedFood.stripeToAttach == stripe) {
            stripe.detectedFood.stripeToAttach = null;
        }
    }
}
