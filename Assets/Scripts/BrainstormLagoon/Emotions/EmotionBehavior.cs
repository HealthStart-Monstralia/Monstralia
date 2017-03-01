using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionBehavior : MonoBehaviour {

	public AudioClip clipOfName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		EmotionsGameManager.GetInstance().CheckEmotion(this.gameObject);
		EmotionsGameManager.GetInstance().subtitlePanel.GetComponent<SubtitlePanel>().Display(this.gameObject.name, this.clipOfName);
	}
}
