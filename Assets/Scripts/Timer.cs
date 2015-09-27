using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float timeLimit;

	private bool timing;
	private float timeRemaining;
	private float nextUpdateTime;

	void Start() {
		timing = false;
		nextUpdateTime = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(timing) {
			timeRemaining -= Time.deltaTime;
		}
	
	}

	public void SetTimeLimit(float timeLimit) {
		this.timeLimit = timeLimit;
		timeRemaining = timeLimit;
	}

	public void StartTimer() {
		timing = true;
	}

	public int TimeRemaining() {
		return (int)timeRemaining;
	}

	public void StopTimer() {
		timing = false;
	}
}
