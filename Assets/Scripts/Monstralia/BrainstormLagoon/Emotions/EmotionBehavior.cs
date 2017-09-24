using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionBehavior : MonoBehaviour {
	public EmotionsGameManager.MonsterEmotions emotions;
	public AudioClip clipOfName;

	void OnMouseDown() {
		if (EmotionsGameManager.GetInstance ().inputAllowed && (EmotionsGameManager.GetInstance().isTutorialRunning || EmotionsGameManager.GetInstance().gameStarted)) {
			EmotionsGameManager.GetInstance ().CheckEmotion (emotions);
			EmotionsGameManager.GetInstance ().subtitlePanel.GetComponent<SubtitlePanel> ().Display (emotions.ToString(), this.clipOfName);
		}
	}
}
