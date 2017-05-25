using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickerSlot : MonoBehaviour, IDropHandler {

	public StickerManager.StickerType typeOfSticker;
	public bool isStickerFilled = false;

	void Start () {
		if (isStickerFilled) {
			GetComponent<Image> ().raycastTarget = false;
		}
	}

	public void OnDrop (PointerEventData eventData) {
		print ("Ondrop");
		if (eventData.pointerDrag.GetComponent<StickerBehaviour> ()) {
			StickerBehaviour sticker = eventData.pointerDrag.GetComponent<StickerBehaviour> ();
			if (!isStickerFilled && sticker.typeOfSticker == typeOfSticker) {
				SoundManager.GetInstance ().PlayCorrectSFX ();
				ReceiveSticker (sticker);
			}
		}
	}

	public void ReceiveSticker(StickerBehaviour sticker) {
		sticker.OnSticked ();
		isStickerFilled = true;
		sticker.gameObject.transform.position = transform.position;
		GetComponent<Image> ().raycastTarget = false;
		sticker.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		GameManager.GetInstance ().OnStickerPlaced(sticker.typeOfSticker);
		Destroy (sticker);
	}

	public bool GetIsStickerFilled () {
		return isStickerFilled;
	}
}
