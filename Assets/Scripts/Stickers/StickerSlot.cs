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
        StickerBehaviour sticker = eventData.pointerDrag.GetComponent<StickerBehaviour> ();
        if (sticker) {
			if (!isStickerFilled && sticker.typeOfSticker == typeOfSticker) {
				SoundManager.Instance.PlayCorrectSFX ();
                if (clipOfSticker)
                    SoundManager.Instance.AddToVOQueue (clipOfSticker);
				ReceiveSticker (sticker, false);
                StickerManager.Instance.OnDropSticker ();
			}
		}
	}

	public void DisableInput(bool disable) {
		if (!isStickerFilled) {
			GetComponent<Image> ().enabled = !disable;
		}
	}

    public void EnableDrop (bool enable) {
        if (!isStickerFilled) {
            GetComponent<Image> ().raycastTarget = enable;
        }
    }

    public void ReceiveSticker(StickerBehaviour sticker, bool isAlreadyPlaced) {
		isStickerFilled = true;
		sticker.gameObject.transform.position = transform.position;
		sticker.transform.SetParent (transform);
		GetComponent<Image> ().raycastTarget = false;
		sticker.GetComponent<CanvasGroup> ().blocksRaycasts = false;
        if (!isAlreadyPlaced)
		    GameManager.Instance.OnStickerPlaced(sticker.typeOfSticker);
		Canvas can = gameObject.AddComponent<Canvas> ();
        gameObject.AddComponent<GraphicRaycaster> ();
        can.overrideSorting = true;
		can.sortingOrder = -1;
        if (label) label.SetActive (true);
        sticker.OnSticked ();
        Destroy (sticker);
	}

	public bool GetIsStickerFilled () {
		return isStickerFilled;
	}

    public void PlayStickerVoiceOver () {
        print ("Play Voice Over");
        SoundManager.Instance.PlayVoiceOverClip (clipOfSticker);
    }
}
