using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMazeMonster : MonoBehaviour {

	private Animator animComp;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
    }

	public void PlaySpawn() {
        animComp.StopPlayback ();
        animComp.Play ("BMaze_Spawn", -1, 0f);
	}

	public void PlayDance() {
        animComp.StopPlayback ();
        animComp.Play ("BMaze_Dance", -1, 0f);
    }
}
