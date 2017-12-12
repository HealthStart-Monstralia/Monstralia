using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickerSlot : MonoBehaviour, IDropHandler {
	public DataType.StickerType typeOfSticker;
	public bool isStickerFilled = false;
    public AudioClip clipOfSticker;

    private GameObject label;

	void Start () {
        if (transform.childCount > 0)
            label = transform.GetChild (0).gameObject;
        if (label) label.SetActive (false);
	}

	public void OnDrop (PointerEventData eventData) {
		if (eventData.pointerDrag.GetComponent<StickerBehaviour> ()) {
			StickerBehaviour sticker = eventData.pointerDrag.GetComponent<StickerBehaviour> ();
			if (!isStickerFilled && sticker.typeOfSticker == typeOfSticker) {
				SoundManager.GetInstance ().PlayCorrectSFX ();
                if (clipOfSticker)
                    SoundManager.GetInstance ().AddToVOQueue (clipOfSticker);
				ReceiveSticker (sticker, false);
			}
		}
	}

	public void DisableInput(bool disable) {
		if (!isStickerFilled) {
			GetComponent<Image> ().enabled = !disable;
		}
	}

	public void ReceiveSticker(StickerBehaviour sticker, bool isAlreadyPlaced) {
		sticker.OnSticked ();
		isStickerFilled = true;
		sticker.gameObject.transform.position = transform.position;
		sticker.transform.SetParent (transform);
		GetComponent<Image> ().raycastTarget = false;
		sticker.GetComponent<CanvasGroup> ().blocksRaycasts = false;
        if (!isAlreadyPlaced)
		    GameManager.GetInstance ().OnStickerPlaced(sticker.typeOfSticker);
		Canvas can = gameObject.AddComponent<Canvas> ();
		can.overrideSorting = true;
		can.sortingOrder = -1;
        if (label) label.SetActive (true);
        Destroy (sticker);
	}

	public bool GetIsStickerFilled () {
		return isStickerFilled;
	}
}
