using UnityEngine;
using System.Collections;

public class PlayVoiceOver : MonoBehaviour {
    public bool playOnStart;
    public AudioClip clipToPlay;

    void Start () {
        if (playOnStart)
            PlayVO (clipToPlay);
    }

    public void PlayVO(AudioClip clip) {
        SoundManager.Instance.StopPlayingVoiceOver ();
        SoundManager.Instance.PlayVoiceOverClip (clip);
    }
}