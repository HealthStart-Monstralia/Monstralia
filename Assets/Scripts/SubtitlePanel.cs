using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubtitlePanel : MonoBehaviour {

	private Subtitle sub;
	private bool isDisplaying = false;
	private Queue<Tuple<string, AudioClip>> displayQueue = new Queue<Tuple<string, AudioClip>>();

	public void Display(string subtitle, AudioClip clip = null, bool queue = false) {
		if(sub == null) {
			sub = new Subtitle();
		}

		if(!queue || !isDisplaying) {
			isDisplaying = true;
			sub.Display(gameObject, subtitle, clip);
		}
		else {
			Tuple<string, AudioClip> t = new Tuple<string, AudioClip>(subtitle, clip);
			displayQueue.Enqueue(t);
			t.ToString();
		}
	}

	public void Hide() {
		isDisplaying = false;
		Debug.Log ("In Hide! ");
		if(displayQueue.Count > 0) {
			Tuple<string, AudioClip> toDisplay = displayQueue.Dequeue();
			Display(toDisplay.first, toDisplay.second);
		}
		else {
			sub.Hide (gameObject);
		}
	}
}
