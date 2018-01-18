using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StickerManager : Singleton<StickerManager> {
	private Vector3[] stickerPositions;
    private Dictionary<DataType.StickerType, GameManager.StickerStats> stickerDict;
    private Dictionary<DataType.StickerType, StickerSlot> stickerSlotDict;

    public bool debug;
	public GameObject[] stickerSpawns;
	public StickerSlot[] stickerSlots;
	public GameObject location;
	public Canvas mainCanvas;

	new void Awake() {
        base.Awake ();

        stickerDict = new Dictionary<DataType.StickerType, GameManager.StickerStats> ();
        stickerSlotDict = new Dictionary<DataType.StickerType, StickerSlot> ();

        AssignSlotsToDict ();

        if (!GameManager.Instance) {
            SwitchScene switchScene = gameObject.AddComponent<SwitchScene> ();
            switchScene.LoadSceneNoScreen ("Start");
        }

        if (SoundManager.Instance)
			SoundManager.Instance.ChangeAndPlayMusic(SoundManager.Instance.gameBackgroundMusic);
	}

    void AssignSlotsToDict() {
        foreach (StickerSlot sticker in stickerSlots) {
            stickerSlotDict.Add (sticker.typeOfSticker, sticker);
        }
    }

	public void CreateSticker(GameManager.StickerStats sticker, DataType.StickerType typeOfSticker) {
        GameObject stickerObject = Instantiate (sticker.stickerObject, location.transform.position, Quaternion.identity, mainCanvas.transform);
        if (sticker.isStickerPlaced)
            stickerSlotDict[typeOfSticker].ReceiveSticker (stickerObject.GetComponent<StickerBehaviour> (), true);
    }

    public void DisableOtherStickerSlots(DataType.StickerType type) {
		foreach (StickerSlot stickerSlot in stickerSlots) {
			if (stickerSlot.typeOfSticker != type) {
				stickerSlot.DisableInput (true);
			} else {
				stickerSlot.DisableInput (false);
			}
		}
	}

	public void EnableOtherStickerSlots(DataType.StickerType type) {
		foreach (StickerSlot stickerSlot in stickerSlots) {
			stickerSlot.DisableInput (false);
		}
	}

	void Start () {
		if (debug) GameManager.Instance.DebugStickers ();
		SpawnStickers ();
	}

    public void SpawnStickers () {
        stickerDict = GameManager.Instance.GetStickerDict ();

        foreach (DataType.StickerType sticker in stickerDict.Keys) {
            if (stickerDict[sticker].isStickerUnlocked) CreateSticker (stickerDict[sticker], sticker);
        }
    }
}
