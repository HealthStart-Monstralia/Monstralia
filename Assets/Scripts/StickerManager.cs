using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StickerManager : MonoBehaviour {
	public enum StickerType { 
		Amygdala = 0, 
		Cerebellum = 1, 
		Frontal = 2, 
		Hippocampus = 3, 
		RainbowBrain = 4 };
	
	private static StickerManager instance;
	private Vector3[] stickerPositions;

	public bool debug;
	public GameObject[] stickersSpawnList;
	public StickerSlot[] stickersSlotList;
	public GameObject location;
	public Canvas mainCanvas;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		if (!GameManager.GetInstance ()) {
			SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
			switchScene.loadScene ("Start");
		}

		if (SoundManager.GetInstance())
			SoundManager.GetInstance().ChangeBackgroundMusic(SoundManager.GetInstance().gameBackgroundMusic);
	}

	public static StickerManager GetInstance() {
		return instance;
	}

	public void SpawnSticker(StickerManager.StickerType stickerSelection, bool isPlaced) {
		GameObject stickerObject;
		stickerObject = Instantiate (stickersSpawnList [(int)stickerSelection], location.transform.position, Quaternion.identity, mainCanvas.transform);
		if (isPlaced)
			stickersSlotList [(int)stickerSelection].ReceiveSticker (stickerObject.GetComponent<StickerBehaviour>());
	}

	public void DisableOtherStickerSlots(StickerType type) {
		foreach (StickerSlot stickerSlot in stickersSlotList) {
			if (stickerSlot.typeOfSticker != type) {
				stickerSlot.DisableInput (true);
			} else {
				stickerSlot.DisableInput (false);
			}
		}
	}

	public void EnableOtherStickerSlots(StickerType type) {
		foreach (StickerSlot stickerSlot in stickersSlotList) {
			stickerSlot.DisableInput (false);
		}
	}

	void Start () {
		if(debug) {
			GameManager.GetInstance ().DebugStickers ();
		}
		GameManager.GetInstance ().FetchStickers ();
	}
}
