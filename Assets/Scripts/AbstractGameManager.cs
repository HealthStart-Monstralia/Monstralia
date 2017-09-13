using UnityEngine;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {
    public DataType.Minigame typeOfGame;
	public abstract void GameOver(); // Force GameOver() to be implemented in child classes
    public abstract void PregameSetup (); // Force PregameSetup() to be implemented in child classes
    public Canvas stickerPopupCanvas;

    void OnDisable () {
        ReviewManager.OnFinishReview -= EndReview;
    }

    // Go to Start scene if no Game Manager is present
    protected void CheckForGameManager () {
        if (!GameManager.GetInstance ()) {
            SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
            switchScene.loadScene ("Start", false);
        }
    }

    protected void Start() {
        print ("AbstractGameManager Start running");
        if (ReviewManager.GetInstance ().needReview) {
            StartReview ();
        } else {
            PregameSetup ();
        }
    }

    protected void StartReview () {
        print ("AbstractGameManager StartReview");
        ReviewManager.OnFinishReview += EndReview;
        ReviewManager.GetInstance ().StartReview (typeOfGame);
    }

    protected void EndReview () {
        print ("AbstractGameManager EndReview");
        ReviewManager.OnFinishReview -= EndReview;
        PregameSetup ();
    }

    public virtual void UnlockSticker() {
		if (stickerPopupCanvas) {
			stickerPopupCanvas.gameObject.SetActive (true);
			SoundManager.GetInstance ().PlayUnlockStickerVO ();

			if (GameManager.GetInstance ().lagoonFirstSticker) {
				GameManager.GetInstance ().lagoonFirstSticker = false;
				stickerPopupCanvas.transform.Find ("BackButton").gameObject.SetActive (false);
				stickerPopupCanvas.transform.Find ("StickerbookButton").gameObject.SetActive (true);
			} else {
				stickerPopupCanvas.transform.Find ("BackButton").gameObject.SetActive (true);
				stickerPopupCanvas.transform.Find ("StickerbookButton").gameObject.SetActive (false);
			}

		} else {
			//Debug.LogError ("Error: Sticker Popup Canvas not assigned to Manager.");
		}

        GameManager.GetInstance ().ActivateSticker (typeOfGame);
    }
}
