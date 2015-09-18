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
	void Update () {
		if(timing) {
			timeRemaining -= Time.deltaTime;
			if(Time.time > nextUpdateTime) {

			}
		}
	
	}

	public void SetTimeLimit(float timeLimit) {
		this.timeLimit = timeLimit;
		timeRemaining = timeLimit;
	}

	public void StartTimer() {
		timing = true;
		nextUpdateTime = Time.time + 1.0f;
	}

	public int TimeRemaining() {
		return (int)timeRemaining;
	}

	public void StopTimer() {
		timing = false;
	}
}
