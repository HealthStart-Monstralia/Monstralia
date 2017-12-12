/* AudioManager_LJ.cs
 * Description: This cs script is meant to exist on a prefab game object which contains audio files.
 *              It handles playing audio files based on touchInputMask and touched/clicked objects.
 *              This script needs PlayerInputController_LJ.cs and InteractableObject_LJ.cs to run properly.
 * Author: Lance C. Jasper
 * Created: 15JUNE2017
 * Last Modified: 19JULY2017 
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Ensures the script automatically adds an audio source when added to a game object that does not have one
[RequireComponent(typeof(AudioSource))]

public class AudioManager_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    //Create more public fields for AudioClips if needed
    [Header("Add Audio Files")]
    [Tooltip("Drag and drop the appropriate audio files to the appropriate function.")]
    public AudioClip buttonDown;
    public AudioClip buttonUp;
    public AudioClip positiveSound;
    public AudioClip gameOverAudio;
    public AudioClip playerWonAudio;
    public AudioClip IntroJingle;
    public AudioClip MagicWoosh;


    //-----PRIVATE FIELDS-----//
    //Make reference object for the audio source
    static AudioSource SensesAudioSource;
    private float volume = .5f;


    //-----ON GAME START-----//
    void Start() {
        //Get the AudioSource in order to reference it in script below
        SensesAudioSource = GetComponent<AudioSource>();
    }


    //-----METHODS-----//
    // Called by SceneManager_LJ.cs
    public void PlayGameOverAudio() {
        SoundManager.GetInstance ().PlaySFXClip (gameOverAudio);
        /*
        SensesAudioSource.clip = gameOverAudio;
        SensesAudioSource.Play();
        SensesAudioSource.volume = .5f;
        */
    }

    public void PlayVictoryJingle() {
        SoundManager.GetInstance ().PlaySFXClip (playerWonAudio);
        /*
        SensesAudioSource.clip = playerWonAudio;
        SensesAudioSource.Play();
        SensesAudioSource.volume = .5f;
        */
    }

    public void PlayIntroJingle() {
        SoundManager.GetInstance ().PlaySFXClip (IntroJingle);
        /*
        SensesAudioSource.clip = IntroJingle;
        SensesAudioSource.volume = .5f;
        SensesAudioSource.Play();
        */
    }

    public void PlayMagicWoosh() {
        SoundManager.GetInstance ().PlaySFXClip (MagicWoosh);
        /*
        SensesAudioSource.clip = MagicWoosh;
        SensesAudioSource.volume = 1f;
        SensesAudioSource.Play();
        */
    }
}
