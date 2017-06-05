using UnityEngine;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {

	public abstract void GameOver(); // Force GameOver() to be implemented in child classes
	public Canvas stickerPopupCanvas;

	public virtual void UnlockSticker(StickerManager.StickerType stickerType) {
		if (stickerPopupCanvas) {
			stickerPopupCanvas.gameObject.SetActive (true);
			SoundManager.GetInstance ().PlayUnlockStickerVO ();
			GameManager.GetInstance ().ActivateBrainstormLagoonReview ();


			if (GameManager.GetInstance ().LagoonFirstSticker) {
				GameManager.GetInstance ().LagoonFirstSticker = false;
				stickerPopupCanvas.transform.Find ("BackButton").gameObject.SetActive (false);
				stickerPopupCanvas.transform.Find ("StickerbookButton").gameObject.SetActive (true);
			} else {
				stickerPopupCanvas.transform.Find ("BackButton").gameObject.SetActive (true);
				stickerPopupCanvas.transform.Find ("StickerbookButton").gameObject.SetActive (false);
			}

			GameManager.GetInstance ().ActivateSticker (stickerType);
		} else {
			Debug.LogError ("Error: Sticker Popup Canvas not assigned to Manager.");
		}
	}
}
