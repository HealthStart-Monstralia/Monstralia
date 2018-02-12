using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StickerManager : Singleton<StickerManager> {
    private Dictionary<DataType.StickerType, GameManager.StickerStats> stickerDict;
    private Dictionary<DataType.StickerType, StickerSlot> stickerSlotDict;

    public bool debug;
    [Tooltip ("Filename starts with Prefabs/Stickers")]
    public string filePath;
	public StickerSlot[] stickerSlots;
	public Canvas mainCanvas;

    [SerializeField] private StickerContainer container;

	new void Awake() {
        base.Awake ();
        stickerDict = new Dictionary<DataType.StickerType, GameManager.StickerStats> ();
        stickerSlotDict = new Dictionary<DataType.StickerType, StickerSlot> ();

        AssignSlotsToDict ();

        if (SoundManager.Instance)
			SoundManager.Instance.ChangeAndPlayMusic(SoundManager.Instance.gameBackgroundMusic);
	}

    void AssignSlotsToDict() {
        foreach (StickerSlot sticker in stickerSlots) {
            stickerSlotDict.Add (sticker.typeOfSticker, sticker);
        }
    }

	public void CreateSticker(GameManager.StickerStats sticker, DataType.StickerType typeOfSticker) {
        GameObject stickerPrefab = GameManager.Instance.GetStickerObject (typeOfSticker);
        if (stickerPrefab) {
            if (sticker.isStickerPlaced) {
                GameObject stickerObject = Instantiate (stickerPrefab, container.transform.position, Quaternion.identity, container.transform);
                stickerSlotDict[typeOfSticker].ReceiveSticker (stickerObject.GetComponent<StickerBehaviour> (), true);
            } else {
                container.AddSticker (stickerPrefab);
            }
        }
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
        container.ChooseSticker ();
    }

    public void SpawnStickers () {
        stickerDict = GameManager.Instance.GetStickerDict ();

        foreach (DataType.StickerType sticker in stickerDict.Keys) {
            if (stickerDict[sticker].isStickerUnlocked) CreateSticker (stickerDict[sticker], sticker);
        }
    }

    public void OnDropSticker () {
        container.RemoveSticker ();
    }


}
