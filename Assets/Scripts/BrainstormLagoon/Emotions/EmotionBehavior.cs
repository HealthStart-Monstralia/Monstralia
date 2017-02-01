using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		EmotionsGameManager.GetInstance().CheckEmotion(this.gameObject);
	}
}
