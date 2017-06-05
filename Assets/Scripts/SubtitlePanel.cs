using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubtitlePanel : MonoBehaviour {

	private Subtitle sub;
	private bool isDisplaying = false;
	private Queue<Tuple<string, AudioClip>> displayQueue = new Queue<Tuple<string, AudioClip>>();

	public Text textComp;

	public void Display(string subtitle, AudioClip clip = null, bool queue = false) {
		if (!gameObject.activeSelf)
			gameObject.SetActive (true);

		if(sub == null) {
			sub = new Subtitle();
		}

		if(!queue || !isDisplaying) {
			isDisplaying = true;
			sub.Display(gameObject, textComp, subtitle, clip);
			if (EmotionsGameManager.GetInstance ()) {
				StartCoroutine (WaitTillHide (EmotionsGameManager.GetInstance ().waitDuration));
			}
			else if (BMaze_Manager.GetInstance ()) {
				StartCoroutine (WaitTillHide (2f));
			} 
			/*
			else if (MemoryMatchGameManager.GetInstance ()) {
				StopCoroutine (WaitTillHide(3f));
				StartCoroutine (WaitTillHide (3f));
			}
			*/
		}
		else {
			Tuple<string, AudioClip> t = new Tuple<string, AudioClip>(subtitle, clip);
			displayQueue.Enqueue(t);
			t.ToString();
		}
	}

	public void Hide() {
		isDisplaying = false;
		if(displayQueue.Count > 0) {
			Tuple<string, AudioClip> toDisplay = displayQueue.Dequeue();
			Display(toDisplay.first, toDisplay.second);
		}
		else {
			if(sub != null)
			sub.Hide (gameObject);
		}
	}

	public IEnumerator WaitTillHide(float duration) {
		yield return new WaitForSeconds (duration);
		Hide ();
	}
}
