/* IntroChimesAudioSource_LJ.cs
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

public class IntroChimesAudioSource_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    //Create more public fields for AudioClips if needed
    [Header("Add Audio Files")]
    [Tooltip("Drag and drop the appropriate audio files to the appropriate function.")]
    public AudioClip introChimes;


    //-----PRIVATE FIELDS-----//
    //Make reference object for the audio source
    static AudioSource ChimesAudioSource;
    private float volume = .5f;


    //-----ON GAME START-----//
    void Start()
    {
        //Get the AudioSource in order to reference it in script below
        ChimesAudioSource = GetComponent<AudioSource>();
    }


    //-----METHODS-----//
    //This method is called by SceneManager_LJ.cs

    public void PlayChime()
    {
        ChimesAudioSource.clip = introChimes;
        ChimesAudioSource.volume = .5f;
        ChimesAudioSource.Play();
        Debug.Log("Played Chimes");
    }
}
