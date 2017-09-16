using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReviewBrainbowFood : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
	private bool isPlaced = false;
	private Vector2 pointerOffset;
	private CanvasGroup canvasGroup;
	private Vector3 origin;

	void Awake() {
        canvasGroup = gameObject.AddComponent<CanvasGroup> ();
		SetOrigin ();
	}

	public void OnPointerDown (PointerEventData eventData) {
		if (!isPlaced) {
            canvasGroup.blocksRaycasts = false;
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!isPlaced) {
            RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 1.0f, ReviewBrainbow.GetInstance().mask);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<ReviewBrainbowSlot> ().color == GetComponent<Food> ().color) {
                SoundManager.GetInstance ().PlayCorrectSFX();
                SetPlaced (true, hit.collider.gameObject.transform);
                ReviewBrainbow.GetInstance ().IncreaseNumOfFilledSlots ();
            }
            else {
                transform.position = origin;
            }

            canvasGroup.blocksRaycasts = true;
        }
	}

	public void OnDrag (PointerEventData eventData) {
		if (!isPlaced) {
			Vector3 screenPoint = new Vector3 (eventData.position.x, eventData.position.y, 0f);
			screenPoint.z = -Camera.main.transform.position.z;
			transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
		}
	}

	public void SetPlaced (bool placed, Transform trans) {
		isPlaced = placed;
		gameObject.transform.SetParent (trans);
		gameObject.transform.localPosition = Vector3.zero;
	}

	public void SetOrigin () {
		origin = transform.position;
	}
}
