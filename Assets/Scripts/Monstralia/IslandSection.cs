using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSection : MonoBehaviour {
    public DataType.IslandSection island;
    public AudioClip introAudio, welcomeBackClip, backgroundMusic;

    private void Awake () {
        GameManager.GetInstance ().SetIslandSection (island);
    }

    private void Start () {
        PlayWelcomeVO ();
        SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
    }

    public void PlayWelcomeVO() {
        if (!GameManager.GetInstance ().GetVisitedArea (island)) {
            if (introAudio != null)
                SoundManager.GetInstance ().PlayVoiceOverClip (introAudio);
            GameManager.GetInstance ().SetVisitedArea (island, true);
        } else {
            if (welcomeBackClip != null)
                SoundManager.GetInstance ().PlayVoiceOverClip (welcomeBackClip);
        }
    }

}
