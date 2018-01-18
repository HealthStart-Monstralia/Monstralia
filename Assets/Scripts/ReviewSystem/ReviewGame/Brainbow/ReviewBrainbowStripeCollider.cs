﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewBrainbowStripeCollider : MonoBehaviour {
    public ReviewBrainbowStripe stripe;

    private void OnTriggerEnter2D (Collider2D collision) {
        stripe.detectedFood = collision.GetComponent<ReviewBrainbowFood> ();
        if (stripe.detectedFood) {
            Colorable.Color foodColor = collision.GetComponent<Food> ().color;
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
