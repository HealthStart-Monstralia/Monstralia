using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionBehavior : MonoBehaviour {
	public EmotionsGameManager.MonsterEmotions emotions;
	public AudioClip clipOfName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		if (EmotionsGameManager.inputAllowed && (EmotionsGameManager.GetInstance().isTutorialRunning || EmotionsGameManager.GetInstance().gameStarted)) {
			EmotionsGameManager.GetInstance ().CheckEmotion (this.gameObject);
			EmotionsGameManager.GetInstance ().subtitlePanel.GetComponent<SubtitlePanel> ().Display (emotions.ToString(), this.clipOfName);
		}
	}
}
