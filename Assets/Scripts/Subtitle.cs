using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle {

	public void Display(GameObject subtitlePanel, GameObject food, AudioClip clip) {
		subtitlePanel.SetActive(true);
		subtitlePanel.GetComponentsInChildren<Text>()[0].text = food.name;
		SoundManager.GetInstance().PlayVoiceOverClip(clip);
	}

	public void Hide(GameObject subtitlePanel) {
		subtitlePanel.SetActive(false);
	}
}
