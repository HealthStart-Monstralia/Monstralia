using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour {
    public Slider volumeSlider;
    public Slider sfxSlider;
    public AudioClip sfxTestClip;
    public Slider voiceOverSlider;
    public AudioClip voiceTestClip;

    void Start () {
        //set the value of the volume slider
        if (volumeSlider) {
            volumeSlider.value = SoundManager.Instance.musicScore.volume;
            sfxSlider.value = SoundManager.Instance.sfxSource.volume;
            voiceOverSlider.value = SoundManager.Instance.voiceOverSource.volume;
        }
    }
    /**
    * \brief Change the sound effect volume.
    * @param newVolume: the float value of the new volume, will go into the coroutine function ChangeSFXVolumeHelper
    */
    public void ChangeSFXVolume (float newVolume) {
        if (!SoundManager.Instance.GetIsPlayingClip ()) {
            StartCoroutine (SoundManager.Instance.ChangeSFXVolumeHelper (newVolume, sfxTestClip));
        }
    }

}
