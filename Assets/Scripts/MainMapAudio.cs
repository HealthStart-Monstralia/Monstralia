using UnityEngine;
using System.Collections;

public class MainMapAudio : MonoBehaviour {
    /* For playing the voiceover */

	public AudioClip introAudio, welcomeBackClip;

	// Use this for initialization
	void Start () {
        if (GameManager.GetInstance ().firstTimeInMainMap) {
            if (introAudio != null)
                SoundManager.GetInstance ().PlayVoiceOverClip (introAudio);
            GameManager.GetInstance ().firstTimeInMainMap = false;
        } else
            SoundManager.GetInstance ().PlayVoiceOverClip (welcomeBackClip);		
	}

}
