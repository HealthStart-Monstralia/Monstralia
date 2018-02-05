using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitSystem : Singleton<ExitSystem> {
    public Button yesButton, noButton;
    private Animator anim;
    private bool isExiting = false;

    private new void Awake () {
        base.Awake ();
        anim = GetComponent<Animator> ();
    }

    public void OnExitYes () {
        CloseNotification ();
        ExitGame ();
    }

    public void OnExitNo () {
        CloseNotification ();
    }

    private void CloseNotification () {
        if (!isExiting) {
            isExiting = true;
            if (anim)
                GetComponent<Animator> ().Play ("PopupFadeOut", -1, 0f);
            yesButton.interactable = false;
            noButton.interactable = false;
            Destroy (gameObject, 0.5f);
        }
    }

    private void ExitGame () {
        Application.Quit();
    }
}
