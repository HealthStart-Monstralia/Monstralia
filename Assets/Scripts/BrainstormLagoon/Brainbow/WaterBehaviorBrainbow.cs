using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaterBehaviorBrainbow : MonoBehaviour {

	public AudioClip waterClip;
    public GameObject plus5;

	void Start () {
		BrainbowGameManager.GetInstance ().waterBottleList.Add (gameObject);
    }

	void OnMouseDown() {
        BrainbowGameManager.GetInstance().timer.AddTime(5.0f);
		SoundManager.GetInstance ().PlaySFXClip (waterClip);
		CreatePlusFive ();
		BrainbowGameManager.GetInstance ().waterBottleList.Remove (gameObject);
		Destroy (gameObject);
	}

	void CreatePlusFive() {
		GameObject plusFive = Instantiate (plus5, transform.position, Quaternion.identity, BrainbowGameManager.GetInstance ().mainCanvas.transform);
		Destroy (plusFive, 2.5f);
	}
}
