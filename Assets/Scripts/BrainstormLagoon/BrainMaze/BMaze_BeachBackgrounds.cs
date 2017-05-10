using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMaze_BeachBackgrounds : MonoBehaviour {
	public Sprite[] beachBackgrounds;

	private Image imgComp;

	void Awake() {
		imgComp = GetComponent<Image> ();
	}

	public void ChangeBackground(int selection) {
		imgComp.sprite = beachBackgrounds [selection];
	}
}
