using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimerClock : Singleton<TimerClock> {
    public float timeTickRate = 0.05f;  /*!< How often the timer updates */
    public float timeLimit = 5f;        /*!< The time limit that this timer will use */
    public Text textObject;
    public Image fill;
    public Color fullColor, emptyColor;
    public GameObject timeUpNotification;
    public bool allowTimeNotification = false;
    public AudioClip tick, tock, alarm;
    public UnityEvent OutOfTimeEvent;
    public float TimeRemaining {
        get { return timeRemaining; }
        set {
            timeRemaining = value;
            UpdateFill (timeRemaining / timeLimit);
            UpdateClockText ();
        }
    }

    private bool isTiming = false;    /*!< Flag to keep track of when to start/stop counting down */
    private bool isTimeLow = false;
    private bool isRunning;
    private float timeRemaining;    /*!< The time remaining */
    private AudioSource audioSrc;
    private Coroutine timeCoroutine;

    new void Awake () {
        base.Awake ();
        audioSrc = GetComponent<AudioSource> ();
    }

    private void OnDestroy () {
        audioSrc = null;
        isTiming = false;
        isTimeLow = false;
        timeLimit = 5f;
    }

    /** \cond */
    void Start () {
        TimeRemaining = timeLimit;
    }

    IEnumerator Timing (float seconds) {
        isRunning = true;
        while (isTiming) {
            yield return new WaitForSeconds (seconds);
            if (isTiming) {
                float timePercentage = TimeRemaining / timeLimit;
                if (TimeRemaining >= 0f) {
                    TimeRemaining -= seconds;
                    if (!isTimeLow && timeRemaining < timeLimit * 0.25f) {
                        isTimeLow = true;
                        StartCoroutine (TickTock ());
                    } else if (isTimeLow && timeRemaining >= timeLimit * 0.25f) {
                        isTimeLow = false;
                        audioSrc.Stop ();
                    }
                }
                else {
                    OutOfTime ();
                }
            }
        }
        isRunning = false;
    }

    void OutOfTime() {
        StopTimer ();
        OutOfTimeEvent.Invoke ();
        if (allowTimeNotification)
            StartCoroutine (ShowTimeUpNotification (3f));
    }

    void UpdateClockText () {
        textObject.text = GetTimeRemainingInt ().ToString ();
    }

    /** \endcond */

    /**
	 * \brief Set the time limit for this timer
	 * @param timeLimit: a float that the timer will count down from
	 */
    public void SetTimeLimit (float timeLimit) {
        this.timeLimit = timeLimit;
        TimeRemaining = timeLimit;
    }

    /**
    * \brief Tell the timer to start counting down
    */
    public void StartTimer () {
        isTiming = true;
        timeCoroutine = StartCoroutine (Timing (timeTickRate));
    }

    /**
     * \brief Tell the timer to stop counting down
     */
    public void StopTimer () {
        isTiming = false;
        if (isRunning)
            StopCoroutine (timeCoroutine);
    }

    /**
	 * \brief Get the time remaining without the decimal
	 * @return The timeRemaining without the decimal
	 */
    public int GetTimeRemainingInt () {
        return Mathf.CeilToInt (TimeRemaining);
    }

    public void SubtractTime (float delta) {
        print ("function call: subtract time");
        if (TimeRemaining > 0) {
            print ("subtracted time");
            TimeRemaining -= delta;
        }
    }

    public void AddTime (float delta) {
        TimeRemaining += delta;
    }

    void UpdateFill (float progress) {
        fill.fillAmount = progress;
        fill.color = Color.Lerp (emptyColor, fullColor, progress * 2f);
    }

    IEnumerator ShowTimeUpNotification(float time) {
        GameObject notification = Instantiate (timeUpNotification, transform.parent);
        audioSrc.PlayOneShot (alarm);
        yield return new WaitForSeconds (time);
        Destroy (notification, 1f);
    }

    IEnumerator TickTock () {
        while (isTiming && isTimeLow) {
            audioSrc.PlayOneShot (tick);
            yield return new WaitForSeconds (0.5f);
            audioSrc.PlayOneShot (tock);
        }
    }
}
