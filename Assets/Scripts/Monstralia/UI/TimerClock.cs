using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimerClock : MonoBehaviour {

    public float timeLimit = 5f;         /*!< The time limit that this timer will use */
    public Text textObject;
    public Image fill;
    public Color fullColor, emptyColor;
    public GameObject timeUpNotification;
    public bool allowTimeNotification = false;
    public AudioClip tick, tock, alarm;
    public UnityEvent OutOfTimeEvent;

    [SerializeField] private bool timing = false;    /*!< Flag to keep track of when to start/stop counting down */
    [SerializeField] private bool timeIsLow = false;
    private float timeRemaining;    /*!< The time remaining */
    private static TimerClock instance;
    private AudioSource audioSrc;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        audioSrc = GetComponent<AudioSource> ();
    }

    public static TimerClock GetInstance() {
        return instance;
    }

    private void OnDestroy () {
        instance = null;
        audioSrc = null;
        timing = false;
        timeIsLow = false;
        timeLimit = 5f;
    }

    /** \cond */
    void Start () {
        timeRemaining = timeLimit;
        UpdateFill (timeRemaining / timeLimit);
    }

    void FixedUpdate () {
        if (timing) {
            float timePercentage = timeRemaining / timeLimit;
            if (timeRemaining >= 0f) {
                timeRemaining -= Time.deltaTime;
                UpdateFill (timePercentage);
                if (!timeIsLow && timeRemaining < timeLimit * 0.25f) {
                    timeIsLow = true;
                    StartCoroutine (TickTock ());
                }
                else if (timeIsLow && timeRemaining >= timeLimit * 0.25f) {
                    timeIsLow = false;
                }
            } else {
                StopTimer ();
                OutOfTimeEvent.Invoke ();
                if (allowTimeNotification)
                    StartCoroutine (ShowTimeUpNotification (3f));
            }
        }

        textObject.text = TimeRemaining ().ToString ();
    }

    /** \endcond */

    /**
	 * \brief Set the time limit for this timer
	 * @param timeLimit: a float that the timer will count down from
	 */
    public void SetTimeLimit (float timeLimit) {
        this.timeLimit = timeLimit;
        timeRemaining = timeLimit;
    }

    /**
    * \brief Tell the timer to start counting down
    */
    public void StartTimer () {
        timing = true;
    }

    /**
     * \brief Tell the timer to stop counting down
     */
    public void StopTimer () {
        timing = false;
    }

    /**
	 * \brief Get the time remaining without the decimal
	 * @return The timeRemaining without the decimal
	 */
    public int TimeRemaining () {
        return Mathf.CeilToInt (timeRemaining);
    }

    public void SubtractTime (float delta) {
        print ("function call: subtract time");
        if (timeRemaining > 0) {
            print ("subtracted time");
            timeRemaining -= delta;
        }
    }

    public void AddTime (float delta) {
        timeRemaining += delta;
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
        while (timing && timeIsLow) {
            audioSrc.PlayOneShot (tick);
            yield return new WaitForSeconds (0.5f);
            audioSrc.PlayOneShot (tock);
        }
    }
}
