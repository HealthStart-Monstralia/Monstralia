using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_WaterPickup : BMaze_Pickup {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public float timeIncrease = 3f;

	/*! Increase time on pickup, called from BMaze_Pickup */
	public void IncreaseTime() {
		TimerClock.Instance.AddTime(timeIncrease);
	}

    new void OnTriggerEnter2D (Collider2D col) {
        base.OnTriggerEnter2D (col);
        IncreaseTime ();
    }
}
