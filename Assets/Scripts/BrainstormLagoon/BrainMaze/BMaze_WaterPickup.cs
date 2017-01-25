using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_WaterPickup : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public float timeIncrease = 3f;

	/*! Increase time on pickup, called from BMaze_Pickup */
	public void IncreaseTime() {
		BMaze_Manager.timeLeft += timeIncrease;
	}
}
