using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour {

	public string name;
	public AudioClip clipOfName;
	public GameObject subtitleBackground;

	public string GetName() {
		return name;
	}

	public AudioClip GetClip() {
		return clipOfName;
	}

	public void Display(Vector3 newPos) {
		Text subText = subtitleBackground.GetComponent<Text>();
		//subText.text = "" + name;
		subtitleBackground.transform.position = newPos;
		subtitleBackground.gameObject.SetActive(true);
	}
}
