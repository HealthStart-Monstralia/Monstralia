/* ButtonAudioSource.cs
 * Description: This script exists as a workaround to allow two often playing sounds to exist simultaneously and not interrupt/cut one another.
 *              This is meant as a temporary solution.
 * Author: Lance C. Jasper
 * Created: 11AUGUST2017
 * Last Modified: 11AUGUST2017 
 */
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Ensures the script automatically adds an audio source when added to a game object that does not have one
[RequireComponent(typeof(AudioSource))]

public class ButtonAudioSource_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    //Create more public fields for AudioClips if needed
    [Header("Add Audio Files")]
    [Tooltip("Drag and drop the appropriate audio files to the appropriate function.")]
    public AudioClip buttonDown;
    public AudioClip buttonUp;
    public AudioClip positiveSound;


    //-----PRIVATE FIELDS-----//
    //Make reference object for the audio source
    static AudioSource ButtonAudioSource;
    private float volume = .5f;


    //-----ON GAME START-----//
    void Start()
    {
        //Get the AudioSource in order to reference it in script below
        ButtonAudioSource = GetComponent<AudioSource>();
    }


    //-----METHODS-----//
    //This method is called by InteractableObject_LJ.cs
    //In InteractableObject_LJ.cs, the OnTouch METHODS are called by PlayerInputController_LJ.cs 
    public void ButtonDown()
    {
        ButtonAudioSource.clip = buttonDown;
        ButtonAudioSource.Play();
    }

    //This method is called by InteractableObject_LJ.cs
    //In InteractableObject_LJ.cs, the OnTouch METHODS are called by PlayerInputController_LJ.cs 
    public void ButtonUp()
    {
        ButtonAudioSource.clip = buttonUp;
        ButtonAudioSource.Play();
    }

    public void PlayPositiveSound()
    {
        ButtonAudioSource.clip = positiveSound;
        ButtonAudioSource.volume = volume;
        ButtonAudioSource.Play();
    }
}
