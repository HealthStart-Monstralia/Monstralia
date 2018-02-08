using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour {
    public delegate void CountdownCallback ();
    [Tooltip ("Audioclips will be played from the bottom of the array")]
    [SerializeField] private AudioClip[] countClip;

    [SerializeField] private string[] countSign;
    [SerializeField] private Animator countdownAnimator;
    [SerializeField] private Text countdownText;
    private CountdownCallback CallbackFunction;

    public void StartCountDown (float waitDuration = 0.5f) {
        StartCoroutine (RunCountdown (waitDuration));
    }

    public void StartCountDown(CountdownCallback Callback, float waitDuration = 0.5f) {
        CallbackFunction = Callback;
        StartCoroutine (RunCountdown (waitDuration));
    }

	IEnumerator RunCountdown(float waitDuration) {
        yield return new WaitForSeconds (waitDuration);
		SoundManager.Instance.StopPlayingVoiceOver();
        for (int i = 3; i >= 0; i--) {
            countdownAnimator.gameObject.SetActive (true);
            countdownText.text = countSign[i];
            SoundManager.Instance.PlayVoiceOverClip (countClip[i]);
            countdownAnimator.Play ("CountdownGoAnimation", -1, 0f);
            yield return new WaitForSeconds (1.0f);
        }

        if (CallbackFunction != null)
            CallbackFunction ();

        Destroy (gameObject);
    }

}
