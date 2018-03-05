﻿using System.Collections;
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
        if (HasPlayerHasSeenIntro()) {
            sceneLoader.LoadScene ("Start");
        }
        else {
            sceneLoader.LoadScene ();
        }
    }

    bool HasPlayerHasSeenIntro () {
        if (!PlayerPrefs.HasKey ("MonstraliaIntro")) {
            print ("Player has not seen intro");
            SetPlayerWatchedIntro ();
            return false;
        } else {
            print ("Player has seen intro");
            return GetPlayerWatchedIntro ();
        }
    }

    void SetPlayerWatchedIntro () {
        PlayerPrefs.SetInt ("MonstraliaIntro", 1);
        PlayerPrefs.Save ();
    }

    bool GetPlayerWatchedIntro () {
        // Convert value into bool, 1 = true, not 1 = false
        return PlayerPrefs.GetInt ("MonstraliaIntro") == 1 ? true : false;
    }
}
