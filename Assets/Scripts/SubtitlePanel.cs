using UnityEngine;
using System.Collections;

public class SubtitlePanel : MonoBehaviour {

	private Subtitle sub;

	public void Display(string subtitle, AudioClip clip) {
		if(sub == null) {
			sub = new Subtitle();
		}

		sub.Display(gameObject, subtitle, clip);
	}

	public void Hide() {
		sub.Hide (gameObject);
	}
}
