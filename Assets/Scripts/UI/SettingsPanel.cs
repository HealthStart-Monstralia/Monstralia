﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : PopupPage {
    public Slider musicSlider;
    public Slider ambientSlider;
    public Slider sfxSlider;
    public Slider voiceOverSlider;
    public AudioClip sfxTestClip;
    public AudioClip voiceTestClip;

    public GameObject settings, credits;
    public Text creditButtonText;

    // Prevent helper sound effects from firing on start
    private bool finishedSetup = false;
    private bool isSettingsShowing = true;

    public void ClosePanel () {
        EnableButtons ();
    }

    private new void OnDestroy () {
        base.OnDestroy ();
        print ("Destroyed");
    }

    private new void Start () {
        print ("Settings Panel Start");
        base.Start ();
        ShowSettings ();
        //set the value of the volume slider
        if (musicSlider)
            musicSlider.value = SoundManager.Instance.musicSource.volume;
        if (ambientSlider)
            ambientSlider.value = SoundManager.Instance.ambientSource.volume;

        if (sfxSlider)
            sfxSlider.value = SoundManager.Instance.sfxSource.volume;

        if (voiceOverSlider)
            voiceOverSlider.value = SoundManager.Instance.voiceOverSource.volume;

        finishedSetup = true;
    }

    public void SwitchPanels () {
        isSettingsShowing = !isSettingsShowing;

        if (isSettingsShowing) {
            ShowSettings ();
        }
        else {
            ShowCredits ();
        }
    }

    void ShowSettings () {
        settings.SetActive (true);
        credits.SetActive (false);
        creditButtonText.text = "Credits";
    }

    void ShowCredits () {
        settings.SetActive (false);
        credits.SetActive (true);
        creditButtonText.text = "Settings";
    }

    /**
    * \brief Change the background music volume.
    * @param newVolume: the float value of the new volume, will go into the coroutine function ChangeSFXVolumeHelper
    */
    public void ChangeMusicVolume (float newVolume) {
        SoundManager.Instance.ChangeBackgroundVolume (newVolume);
    }

    /**
    * \brief Change the ambient music volume.
    * @param newVolume: the float value of the new volume, will go into the coroutine function ChangeSFXVolumeHelper
    */
    public void ChangeAmbientVolume (float newVolume) {
        SoundManager.Instance.ChangeAmbientVolume (newVolume);
    }

    /**
    * \brief Change the sound effect volume.
    * @param newVolume: the float value of the new volume, will go into the coroutine function ChangeSFXVolumeHelper
    */
    public void ChangeSFXVolume (float newVolume) {
        if (finishedSetup) {
            SoundManager.Instance.ChangeSFXVolume (newVolume);
            SoundManager.Instance.MatchHelperVolume ();
            SoundManager.Instance.PlayVolumeHelper (sfxTestClip);
        }
    }

    public void ChangeVoiceOverVolume (float newVolume) {
        if (finishedSetup) {
            SoundManager.Instance.ChangeVoiceOverVolume (newVolume);
            SoundManager.Instance.PlayVoiceOverVolumeHelper (voiceTestClip);
        }
    }

}
