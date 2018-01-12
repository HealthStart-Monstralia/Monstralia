using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerClock : MonoBehaviour {

    public float timeLimit = 5f;         /*!< The time limit that this timer will use */
    public Text textObject;
    public Image fill;
    public Color fullColor, emptyColor;
    public GameObject timeUpNotification;
    public bool allowTimeNotification = false;

    // Event
    public delegate void OutOfTimeAction ();
    public static event OutOfTimeAction OutOfTime;

    private bool timing = false;    /*!< Flag to keep track of when to start/stop counting down */
    private float timeRemaining;    /*!< The time remaining */
    private static TimerClock instance;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

    }

    public static TimerClock GetInstance() {
        return instance;
    }

    private void OnDestroy () {
        instance = null;
    }

    /** \cond */
    void Start () {
        timeRemaining = timeLimit;
        UpdateFill (timeRemaining / timeLimit);
    }

    void FixedUpdate () {
        if (timing) {
            if (timeRemaining >= 0f) {
                timeRemaining -= Time.deltaTime;
                UpdateFill (timeRemaining / timeLimit);
            } else {
                StopTimer ();
                if (OutOfTime != null)
                    OutOfTime ();
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
        GameObject notification = Instantiate (timeUpNotification);
        yield return new WaitForSeconds (time);
        notification.GetComponent<Animator> ().Play ("FoodPanelFadeOut");
        Destroy (notification, 1f);
    }
}
