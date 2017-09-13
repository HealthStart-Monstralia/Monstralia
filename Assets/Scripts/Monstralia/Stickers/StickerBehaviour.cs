using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickerBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler {

	private bool isSticked = false;
	private Vector2 pointerOffset;
	private CanvasGroup canvasGroup;
	private int sortLayer;

	public DataType.StickerType typeOfSticker;

	public void OnPointerDown (PointerEventData eventData) {
		if (!isSticked) {
            canvasGroup.blocksRaycasts = false;
			StickerManager.GetInstance ().DisableOtherStickerSlots (typeOfSticker);
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!isSticked) {
            canvasGroup.blocksRaycasts = true;
			StickerManager.GetInstance ().EnableOtherStickerSlots (typeOfSticker);
		}
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (!isSticked)
			pointerOffset = new Vector3 (eventData.position.x, eventData.position.y, 0) - transform.position;
	}

	public void OnDrag (PointerEventData eventData) {
		if (!isSticked) {
			transform.position = new Vector3 (
				eventData.position.x, 
				eventData.position.y, 
				0f) - new Vector3 (pointerOffset.x, pointerOffset.y, 0f);
		}
	}

	void Awake() {
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	public void OnSticked() {
		isSticked = true;
	}

}
