using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StickerManager : MonoBehaviour {

	public bool debug;

	public List<GameObject> stickers;


	void Awake() {
		SoundManager.GetInstance().ChangeBackgroundMusic(SoundManager.GetInstance().gameBackgroundMusic);
	}

	// Use this for initialization
	void Start () {
		List<string> activeStickers = new List<string>();
		activeStickers = GameManager.GetInstance().GetStickers();

		if(debug) {
			activeStickers.Add("Cerebellum");
			activeStickers.Add ("Frontal");
			activeStickers.Add("Amygdala");
		}

		foreach(string sticker in activeStickers) {
			if(sticker == "Amygdala") {
				stickers[0].SetActive(true);
			}
			else if (sticker == "Cerebellum") {
				stickers[1].SetActive(true);
			}
			else if (sticker == "Frontal") {
				stickers[2].SetActive(true);
			}
			else if (sticker == "Hippocampus") {
				stickers[3].SetActive (true);
			}
			else if(sticker == "Brainbow") {
				stickers[4].SetActive(true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
