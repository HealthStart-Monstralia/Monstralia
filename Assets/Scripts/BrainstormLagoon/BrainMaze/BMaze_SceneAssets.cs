using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_SceneAssets : MonoBehaviour {
	public BMaze_PickupManager pickupManager;
	public BMaze_Door doorObject;
	public BMaze_Finishline finishLine;
	public GameObject startingLocation;

	void Start() {
		if (!pickupManager)
			Debug.LogError ("No Pickup Manager assigned in " + gameObject.name);
		if (!doorObject)
			Debug.LogError ("No Door assigned in " + gameObject.name);
		if (!finishLine)
			Debug.LogError ("No Finish Line assigned in " + gameObject.name);
		if (!startingLocation)
			Debug.LogError ("No Starting Location assigned in " + gameObject.name);
	}

	public BMaze_PickupManager GetPickupManager() {
		return pickupManager;
	}

	public BMaze_Door GetDoor() {
		return doorObject;
	}

	public BMaze_Finishline GetFinishline() {
		return finishLine;
	}

	public GameObject GetStartLocation() {
		return startingLocation;
	}
}
