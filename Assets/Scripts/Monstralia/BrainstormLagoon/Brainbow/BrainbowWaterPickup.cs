using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowWaterPickup : MonoBehaviour {
	public AudioClip waterClip;
    [HideInInspector] public float waterTimeBoost = 5f;
    [HideInInspector] public BrainbowWaterManager manager;

    private bool canDrink = true;

	void OnMouseDown() {
        if (canDrink) {
            canDrink = false;

            if (BrainbowGameManager.Instance.inputAllowed) {
                TimerClock.Instance.AddTime (waterTimeBoost);
                SoundManager.Instance.PlaySFXClip (waterClip);
                manager.waterBottleList.Remove (gameObject);
                DestroyWater ();
            } else if (BrainbowGameManager.Instance.tutorialManager.isRunningTutorial) {
                TimerClock.Instance.AddTime (waterTimeBoost);
                SoundManager.Instance.PlaySFXClip (waterClip);
                DestroyWater ();
            }
        }
	}

    void DestroyWater () {
        LeanTween.scale (gameObject, Vector3.zero, 0.25f).setEaseInBack ();
        Destroy (gameObject, 0.25f);
    }
}
