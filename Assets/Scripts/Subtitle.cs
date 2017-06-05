using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle {

	public void Display(GameObject subtitlePanel, Text subtitleTextComp, string subtitle, AudioClip clip) {
		subtitlePanel.GetComponent<Animator> ().Play ("Subtitle_In"); // Animation has SetActive(true)
		subtitleTextComp.text = subtitle;
		SoundManager.GetInstance().PlayVoiceOverClip(clip);
	}

	public void Hide(GameObject subtitlePanel) {
		subtitlePanel.GetComponent<Animator> ().Play ("Subtitle_Out"); // Animation has SetActive(false)
	}

}
