using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubtitlePanel : Singleton<SubtitlePanel> {
	private bool isDisplaying = false;
    private Coroutine currentCoroutine;
    private Animator animator;
    private float subtitleDuration;

    public GameObject subtitleObject;
	public Text subtitleText;

    private new void Awake () {
        base.Awake ();
        animator = GetComponent<Animator> ();
    }

    private void Start () {
        subtitleObject.SetActive (false);
    }

    /// <summary>
    /// Displays the given string in a subtitle, with optional voice over, voice over queueing, and wait duration.
    /// </summary>
    /// <param name="subtitle">Text to be displayed in the subtitle.</param>
    /// <param name="clip">Voice over audioclip to be played alongside subtitle.</param>
    /// <param name="queueVO">Add the voice over to a queue to be played.</param>
    public void Display (string subtitle = "", AudioClip clip = null, bool queueVO = false, float duration = 3f) {
        subtitleDuration = duration;
        subtitleObject.SetActive (true);
        subtitleText.text = subtitle;
        if (clip) {
            if (queueVO) {
                SoundManager.Instance.AddToVOQueue (clip);
            } else {
                SoundManager.Instance.PlayVoiceOverClip (clip);
            }
        }
        if (!isDisplaying) {
            currentCoroutine = StartCoroutine (FadeInSubtitle ());
            isDisplaying = true;
        } else {
            StopCoroutine (currentCoroutine);
            currentCoroutine = StartCoroutine (ShowSubtitle (subtitleDuration));
        }
    }

    public void Hide () {
        if (isDisplaying) {
            print ("StopCoroutine");
            StopCoroutine (currentCoroutine);
            StartCoroutine (FadeOutSubtitle ());
        }
    }

    IEnumerator FadeInSubtitle() {
        animator.Play ("Subtitle_In", -1, 0f);
        yield return new WaitForSeconds (0.25f);
        currentCoroutine = StartCoroutine (ShowSubtitle (subtitleDuration));
    }

    IEnumerator ShowSubtitle (float duration) {
        animator.Play ("Subtitle_Still", -1, 0f);
        yield return new WaitForSeconds (duration);
        currentCoroutine = StartCoroutine (FadeOutSubtitle ());
    }

    IEnumerator FadeOutSubtitle () {
        animator.Play ("Subtitle_Out", -1, 0f);
        yield return new WaitForSeconds (0.2f);
        subtitleObject.SetActive (false);
        isDisplaying = false;
    }
}
