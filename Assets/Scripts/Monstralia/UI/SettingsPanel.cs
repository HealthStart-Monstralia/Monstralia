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
            volumeSlider.value = SoundManager.GetInstance().backgroundSource.volume;
            sfxSlider.value = SoundManager.GetInstance ().sfxSource.volume;
            voiceOverSlider.value = SoundManager.GetInstance ().voiceOverSource.volume;
        }
    }
    /**
    * \brief Change the sound effect volume.
    * @param newVolume: the float value of the new volume, will go into the coroutine function ChangeSFXVolumeHelper
    */
    public void ChangeSFXVolume (float newVolume) {
        if (!SoundManager.GetInstance ().GetIsPlayingClip ()) {
            StartCoroutine (SoundManager.GetInstance ().ChangeSFXVolumeHelper (newVolume, sfxTestClip));
        }
    }

}
