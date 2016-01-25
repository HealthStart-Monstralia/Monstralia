using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubtitlePanel : MonoBehaviour {

	private Subtitle sub;

	public void Display(string subtitle, AudioClip clip = null) {
		if(sub == null) {
			sub = new Subtitle();
		}

		sub.Display(gameObject, subtitle, clip);
	}

	public void Hide() {
		sub.Hide (gameObject);
	}
}
