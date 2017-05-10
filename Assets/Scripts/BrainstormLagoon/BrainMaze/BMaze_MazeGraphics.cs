using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMaze_MazeGraphics : MonoBehaviour {
	public Sprite[] mazeBackgrounds;

	private Image imgComp;

	void Awake() {
		imgComp = GetComponent<Image> ();
	}

	public void ChangeMaze(int selection) {
		imgComp.sprite = mazeBackgrounds [selection];
	}
}
