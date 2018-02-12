using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMazeWater : BMazePickup {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public float timeIncrease = 3f;

	public void IncreaseTime() {
		TimerClock.Instance.AddTime(timeIncrease);
	}

    new void OnTriggerEnter2D (Collider2D col) {
        base.OnTriggerEnter2D (col);
        IncreaseTime ();
    }
}
