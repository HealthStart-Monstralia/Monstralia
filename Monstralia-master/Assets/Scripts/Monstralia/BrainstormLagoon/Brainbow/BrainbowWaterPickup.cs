using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BrainbowWaterPickup : MonoBehaviour {
	public AudioClip waterClip;
    [HideInInspector] public float waterTimeBoost = 5f;
    [HideInInspector] public BrainbowWaterManager manager;

	void OnMouseDown() {
        if (BrainbowGameManager.Instance.inputAllowed) {
            TimerClock.Instance.AddTime (waterTimeBoost);
            SoundManager.Instance.PlaySFXClip (waterClip);
            manager.waterBottleList.Remove (gameObject);
            Destroy (gameObject);
        }
	}
}
