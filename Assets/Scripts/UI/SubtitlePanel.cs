using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubtitlePanel : Singleton<SubtitlePanel> {
	private bool isDisplaying = false;
	private Coroutine waitCoroutine;
    private Animator animator;

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
        if (subtitle != subtitleText.text || !isDisplaying) {
            subtitleObject.SetActive (true);
            if (!isDisplaying) {
                waitCoroutine = StartCoroutine (DisplayCoroutine (subtitle, clip, queueVO, duration));
            } else {
                StopCoroutine (waitCoroutine);
                waitCoroutine = StartCoroutine (DisplayCoroutine (subtitle, clip, queueVO, duration));
            }
        }
    }

    IEnumerator DisplayCoroutine (string subtitle, AudioClip clip, bool queueVO, float duration) {
        isDisplaying = true;

        subtitleText.text = subtitle;
        ShowSubtitle ();

        if (clip) {
            if (queueVO) {
                SoundManager.Instance.AddToVOQueue (clip);
            } else {
                SoundManager.Instance.PlayVoiceOverClip (clip);
            }
        }

        yield return new WaitForSeconds (duration);
        HideSubtitle ();
        yield return new WaitForSeconds (0.2f);
        subtitleObject.SetActive (false);
        isDisplaying = false;
    }

    public void Hide () {
        if (isDisplaying) {
            StopCoroutine (waitCoroutine);
            HideSubtitle ();
        }
    }

    void ShowSubtitle() {
        animator.Play ("Subtitle_In", -1, 0f);
    }

    void HideSubtitle () {
        animator.Play ("Subtitle_Out", -1, 0f);
    }
}
