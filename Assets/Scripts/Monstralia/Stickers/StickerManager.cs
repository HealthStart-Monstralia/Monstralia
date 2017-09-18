using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StickerManager : MonoBehaviour {
	private static StickerManager instance;
	private Vector3[] stickerPositions;
    private Dictionary<DataType.StickerType, GameManager.StickerStats> stickerDict;
    private Dictionary<DataType.StickerType, StickerSlot> stickerSlotDict;

    public bool debug;
	public GameObject[] stickerSpawns;
	public StickerSlot[] stickerSlots;
	public GameObject location;
	public Canvas mainCanvas;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

        stickerDict = new Dictionary<DataType.StickerType, GameManager.StickerStats> ();
        stickerSlotDict = new Dictionary<DataType.StickerType, StickerSlot> ();

        AssignSlotsToDict ();

        if (!GameManager.GetInstance ()) {
            SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
            switchScene.LoadScene ("Start", false);
        }

        if (SoundManager.GetInstance())
			SoundManager.GetInstance().ChangeBackgroundMusic(SoundManager.GetInstance().gameBackgroundMusic);
	}

	public static StickerManager GetInstance() {
		return instance;
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

        /*
		GameObject stickerObject;
		stickerObject = Instantiate (stickersSpawnList [(int)stickerSelection], location.transform.position, Quaternion.identity, mainCanvas.transform);
		if (isPlaced)
			stickersSlotList [(int)stickerSelection].ReceiveSticker (stickerObject.GetComponent<StickerBehaviour>());
        */
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
		if (debug) GameManager.GetInstance ().DebugStickers ();
		SpawnStickers ();
	}

    public void SpawnStickers () {
        stickerDict = GameManager.GetInstance ().GetStickerDict ();

        foreach (DataType.StickerType sticker in stickerDict.Keys) {
            if (stickerDict[sticker].isStickerUnlocked) CreateSticker (stickerDict[sticker], sticker);
        }
    }
}
