using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MonstraliaIntro : MonoBehaviour {
    public Fader fader;
    public GameObject loadScreen;
    public VideoPlayer videoPlayer;
    private SwitchScene sceneLoader;

    private void Start () {
        SoundManager.Instance.StopBackgroundMusic ();
        videoPlayer.SetTargetAudioSource (0, SoundManager.Instance.GetVoiceOverSource ());
        videoPlayer.Play ();
        sceneLoader = GetComponent<SwitchScene> ();
        sceneLoader.loadingScreen = loadScreen;
        StartCoroutine (FadeIn ());
    }

    IEnumerator FadeIn () {
        fader.FadeStayBlack ();
        yield return new WaitForSeconds (0.5f);
        fader.FadeIn ();
        yield return new WaitForSeconds ((float)videoPlayer.clip.length - 0.5f);
        fader.FadeOut ();
        yield return new WaitForSeconds (1.5f);
        LoadStart ();
    }

    public void LoadStart () {
        sceneLoader.LoadScene ();
    }
}
