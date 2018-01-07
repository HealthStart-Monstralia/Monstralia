using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowWaterPickup : MonoBehaviour {
	public AudioClip waterClip;
    [HideInInspector] public float waterTimeBoost = 5f;
    [HideInInspector] public BrainbowWaterManager manager;

	void OnMouseDown() {
        if (BrainbowGameManager.GetInstance ().inputAllowed) {
            TimerClock.GetInstance().AddTime (waterTimeBoost);
            SoundManager.GetInstance ().PlaySFXClip (waterClip);
            manager.waterBottleList.Remove (gameObject);
            Destroy (gameObject);
        }
	}
}
