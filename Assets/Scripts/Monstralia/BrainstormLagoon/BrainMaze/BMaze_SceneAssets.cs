using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_SceneAssets : MonoBehaviour {
	public BMaze_Door doorObject;
	public BMaze_Finishline finishLine;
	public GameObject startingLocation;
    public BMaze_Pickup[] pickups;

	void Start() {
		if (!doorObject)
			Debug.LogError ("No Door assigned in " + gameObject.name);
		if (!finishLine)
			Debug.LogError ("No Finish Line assigned in " + gameObject.name);
		if (!startingLocation)
			Debug.LogError ("No Starting Location assigned in " + gameObject.name);
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
