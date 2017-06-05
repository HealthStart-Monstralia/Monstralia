using UnityEngine;
using System.Collections;

/**
 * \class Timer
 * \brief A reusable countdown timer
 */

public class Timer : MonoBehaviour {

	public float timeLimit; /*!< The time limit that this timer will use */

	private bool timing;	/*!< Flag to keep track of when to start/stop counting down */
	private float timeRemaining; /*!< The time remaining */

	/** \cond */
	void Start() {
		timing = false;
	}
	
	void FixedUpdate () {
		if(timing && timeRemaining >= 0f) {
			timeRemaining -= Time.deltaTime;
		}
	
	}
	/** \endcond */

	/**
	 * \brief Set the time limit for this timer
	 * @param timeLimit: a float that the timer will count down from
	 */
	public void SetTimeLimit(float timeLimit) {
		this.timeLimit = timeLimit;
		timeRemaining = timeLimit;
	}

	/**
	 * \brief Tell the timer to start counting down
	 */
	public void StartTimer() {
		timing = true;
	}

	/**
	 * \brief Get the time remaining without the decimal
	 * @return The timeRemaining without the decimal
	 */
	public int TimeRemaining() {
		return Mathf.CeilToInt(timeRemaining);
	}

	/**
	 * \brief Tell the timer to stop counting down
	 */
	public void StopTimer() {
		timing = false;
	}

	public void SubtractTime(float delta) {
		print ("function call: subtract time");
		if (timeRemaining > 0) {
			print ("subtracted time");
			timeRemaining -= delta;
		}
	}

	public void AddTime(float delta) {
		timeRemaining += delta;
	}
}
