using UnityEngine;
using System.Collections;

public class LagoonSetUp : MonoBehaviour {

	public AudioClip[] clips;

	// Use this for initialization
	void Awake () {
		SoundManager.instance.LagoonSetup(clips);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
