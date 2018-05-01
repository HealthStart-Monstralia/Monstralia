using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPage : Singleton<PopupPage> {
    public static GameObject currentPopup;
    public delegate void OnClose ();
    public OnClose Close;
    public Button[] buttonsToDisable;
    public GameObject[] itemsToDisable;

    protected void Start () {
        currentPopup = gameObject;
    }

    protected void OnEnable () {
        Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale (gameObject, originalScale, 0.3f).setEaseOutBack ();
    }

    protected void OnDestroy () {
        currentPopup = null;
    }

    public void DisableButtons () {
        for (int i = 0; i < buttonsToDisable.Length; i++) {
            if (buttonsToDisable[i] != null)
                buttonsToDisable[i].interactable = false;
        }

        for (int i = 0; i < itemsToDisable.Length; i++) {
            if (itemsToDisable[i] != null)
                itemsToDisable[i].SetActive (false);
        }
    }

    public void EnableButtons () {
        for (int i = 0; i < buttonsToDisable.Length; i++) {
            if (buttonsToDisable[i] != null)
                buttonsToDisable[i].interactable = true;
        }

        for (int i = 0; i < itemsToDisable.Length; i++) {
            if (itemsToDisable[i] != null)
                itemsToDisable[i].SetActive (true);
        }
    }

    public void OnButtonClose () {
        if (Close != null)
            Close ();
        LeanTween.scale (gameObject, Vector3.zero, 0.3f).setEaseInBack ();
        Destroy (gameObject, 0.3f);
    }

    public void DisableFromAnim () {
        Destroy (gameObject);
    }
}
