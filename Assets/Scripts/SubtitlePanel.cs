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
			print ("if !queue || !isDisplaying");
			isDisplaying = true;
			sub.Display(gameObject, textComp, subtitle, clip);
			StartCoroutine (WaitTillHide(EmotionsGameManager.GetInstance().waitDuration));
		}
		else {
			print ("else");
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
			if(sub != null)
			sub.Hide (gameObject);
		}
	}

	public IEnumerator WaitTillHide(float duration) {
		yield return new WaitForSeconds (duration);
		Hide ();
	}
}
