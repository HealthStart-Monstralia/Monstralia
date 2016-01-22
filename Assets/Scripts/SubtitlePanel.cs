using UnityEngine;
using System.Collections;

public class SubtitlePanel : MonoBehaviour {

	private Subtitle sub;

	// Use this for initialization
	void Start () {
		sub = new Subtitle();
	}

	public void Display(string subtitle, AudioClip clip) {
		sub.Display(gameObject, subtitle, clip);
	}

	public void Hide() {
		sub.Hide (gameObject);
	}
}
