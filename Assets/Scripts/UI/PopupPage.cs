using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPage : MonoBehaviour {
    public static GameObject currentPopup;
    public delegate void OnClose ();
    public OnClose Close;

    private void Awake () {
        currentPopup = gameObject;
    }

    private void OnDestroy () {
        currentPopup = null;
    }

    public void OnButtonClose () {
        Animator anim = GetComponent<Animator> ();
        Close ();
        if (anim) {
            anim.Play ("PopupFadeOut", -1, 0f);
        }
        else {
            Destroy (gameObject);
        }

    }

    public void DisableFromAnim () {
        Destroy (gameObject);
    }
}
