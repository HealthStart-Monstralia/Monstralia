﻿using UnityEngine;
using UnityEngine.EventSystems;

public class StickerBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	private bool isSticked = false;
	private CanvasGroup canvasGroup;
	private int sortLayer;
    private float originalWidth, originalHeight;    // For rescaling the stickers

    public DataType.StickerType typeOfSticker;

	public void OnPointerDown (PointerEventData eventData) {
		if (!isSticked) {
            canvasGroup.blocksRaycasts = false;
			StickerManager.Instance.DisableOtherStickerSlots (typeOfSticker);
            RestoreSize ();
            transform.position = new Vector3 (
                eventData.position.x,
                eventData.position.y,
                0f);
        }
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!isSticked) {
            canvasGroup.blocksRaycasts = true;
			StickerManager.Instance.EnableOtherStickerSlots (typeOfSticker);
            transform.localPosition = Vector3.zero;
            ShrinkSize (GetComponentInParent<StickerContainer>().shrinkSize);
        }
	}

	public void OnDrag (PointerEventData eventData) {
		if (!isSticked) {
			transform.position = new Vector3 (
				eventData.position.x, 
				eventData.position.y, 
				0f);
		}
	}

	void Awake() {
		canvasGroup = GetComponent<CanvasGroup> ();
        originalWidth = GetComponent<RectTransform> ().rect.width;
        originalHeight = GetComponent<RectTransform> ().rect.height;
    }

	public void OnSticked() {
		isSticked = true;
        RestoreSize ();
        transform.SetAsFirstSibling ();
    }

    public void RestoreSize () {
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, originalWidth);
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, originalHeight);
    }

    public void ShrinkSize (float size) {
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, size);
        GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, size);
    }
}
