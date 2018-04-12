using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : Singleton<Notification> {
    public Animator animator;
    public Text textObject;

    public void DisplayNotification (string text = "", float time = 3f) {
        gameObject.SetActive (true);
        textObject.text = text;
        animator.Play ("PopupFadeIn");
        StartCoroutine (DisplayUntil (time));
    }

    IEnumerator DisplayUntil (float time) {
        yield return new WaitForSeconds (time);
        CloseNotification ();
    }

    public void CloseNotification () {
        animator.Play ("PopupFadeOut");
        Destroy (gameObject, 0.5f);
    }
}
