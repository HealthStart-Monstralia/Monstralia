using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPopupButton : MonoBehaviour {
    public GameObject currentPopup;
    public GameObject pagePrefab;
    public Button[] buttonsToControl;   // Any buttons in this list will be automatically disabled or enabled depending on if a popup exists

    // Create a popup instance from prefab, won't execute if there's already another popup.
    public void CreatePopupPage () {
        currentPopup = Instantiate (pagePrefab, transform.parent);
        currentPopup.SetActive (true);
        currentPopup.GetComponent<PopupPage> ().Close = EnableButtons;
        DisableButtons ();
    }

    // Delete the popup instance, won't execute if there's no popup.
    public void ClosePopupPage() {
        if (currentPopup) {
            currentPopup.GetComponent<PopupPage> ().OnButtonClose ();
            currentPopup = null;
        }

        EnableButtons ();
    }

    public void OnButtonPress () {
        if (!PopupPage.currentPopup) {
            CreatePopupPage ();
        }
        else {
            ClosePopupPage ();
        }
    }

    public void DisableButtons () {
        if (buttonsToControl.Length > 0) {
            for (int i = 0; i < buttonsToControl.Length; i++) {
                if (buttonsToControl[i] != null)
                    buttonsToControl[i].interactable = false;
            }
        }
    }

    public void EnableButtons () {
        for (int i = 0; i < buttonsToControl.Length; i++) {
            if (buttonsToControl[i] != null)
                buttonsToControl[i].interactable = true;
        }
    }
}
