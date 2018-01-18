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

    public void StartCountDown () {
        StartCoroutine (RunCountdown ());
    }

    public void StartCountDown(CountdownCallback Callback) {
        CallbackFunction = Callback;
        StartCoroutine (RunCountdown ());
    }

	IEnumerator RunCountdown() {
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
