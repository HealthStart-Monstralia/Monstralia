using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour {

	public string name;
	public AudioClip clip;
	public GameObject subtitleBackground;

	public string GetName() {
		return name;
	}

	public AudioClip GetClip() {
		return clip;
	}

	public void Display(GameObject parent, Vector2 newPos) {
		Debug.Log ("In Subtitle.Display");
		GameObject background = (GameObject)Instantiate(subtitleBackground);
		background.transform.parent = parent.transform;
		Text subText = background.GetComponentsInChildren<Text>()[0];
		subText.text = "" + name;
		background.transform.position = newPos;
		background.gameObject.SetActive(true);
	}
}
