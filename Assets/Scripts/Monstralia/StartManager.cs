using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : Singleton<StartManager> {
    public GameObject introObject;
    public bool playIntro = true;
    public Button[] buttonsToDisable;
    public Fader fader;

    private SwitchScene sceneLoader;

    new void Awake () {
        base.Awake ();
        fader.gameObject.SetActive (true);
        fader.FadeIn ();
    }

    void Start () {
        sceneLoader = GetComponent<SwitchScene> ();

        if (playIntro) {
            DisableButtons ();
            StartCoroutine (PlayIntro ());
        }
    }

    IEnumerator PlayIntro() {
        yield return new WaitForSeconds (0.5f);
        introObject.SetActive (true);
    }

    public void DisableButtons() {
        foreach (Button button in buttonsToDisable) {
            button.interactable = false;
        }
    }

    public void EnableButtons () {
        foreach (Button button in buttonsToDisable) {
            button.interactable = true;
        }
    }

    public void SwitchScene() {
        sceneLoader.LoadScene ();
    }
}
