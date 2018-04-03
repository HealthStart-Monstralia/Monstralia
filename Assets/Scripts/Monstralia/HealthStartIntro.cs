using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthStartIntro : MonoBehaviour {
    public Fader fader;
    public GameObject loadScreen;
    private SwitchScene sceneLoader;

    private void Start () {
        sceneLoader = GetComponent<SwitchScene> ();
        sceneLoader.loadingScreen = loadScreen;
        StartCoroutine (Intro ());
    }

    IEnumerator Intro() {
        fader.FadeStayBlack ();
        yield return new WaitForSeconds (0.5f);
        fader.FadeIn ();
        yield return new WaitForSeconds (3f);
        fader.FadeOut ();
        yield return new WaitForSeconds (1.5f);
        sceneLoader.LoadScene ();
    }
}
