using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MonstraliaIntro : MonoBehaviour {
    public Fader fader;
    public GameObject loadScreen;
    public VideoPlayer videoPlayer;
    private SwitchScene sceneLoader;

    private void Awake () {
        sceneLoader = GetComponent<SwitchScene> ();
        //sceneLoader.loadingScreen = loadScreen;
    }

    private void Start () {
        print ("Loading intro");
        SoundManager.Instance.StopBackgroundMusic ();
        videoPlayer.SetTargetAudioSource (0, SoundManager.Instance.GetVoiceOverSource ());
        videoPlayer.Play ();

        StartCoroutine (FadeIn ());
    }

    IEnumerator FadeIn () {
        fader.FadeStayBlack ();
        yield return new WaitForSeconds (0.5f);

        fader.FadeIn ();
        yield return new WaitForSeconds ((float)videoPlayer.clip.length - 0.5f);

        fader.FadeOut ();
        yield return new WaitForSeconds (0.5f);

        LoadStart ();
    }

    public void LoadStart () {
        print ("Loading start");
        sceneLoader.LoadScene ();
    }
}
