using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour {
	
	public AudioClip clip;

	public string GetName() {
		return name;
	}

	public AudioClip GetClip() {
		return clip;
	}

	public IEnumerator Display(GameObject subtitlePanel, GameObject food) {
		subtitlePanel.SetActive(true);
		subtitlePanel.GetComponentsInChildren<Text>()[0].text = food.name;
		SoundManager.GetInstance().PlayVoiceOverClip(clip);
		yield return new WaitForSeconds(clip.length);
	}
}
