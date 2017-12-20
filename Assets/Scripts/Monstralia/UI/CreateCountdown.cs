using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCountdown : MonoBehaviour {
	public GameObject countdownObject;

	public GameObject SpawnCountdown() {
		GameObject countdown = Instantiate (countdownObject);
		return countdown;
	}
}
