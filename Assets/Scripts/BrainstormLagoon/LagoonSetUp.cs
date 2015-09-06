using UnityEngine;
using System.Collections;

public class LagoonSetUp : MonoBehaviour {

	public AudioClip[] clips;

	// Use this for initialization
	void Awake () {
		Setup ();
	}

	void Setup ()
	{
		SoundManager.instance.LagoonSetup (clips);
	}

	public void Teardown() {
		SoundManager.instance.LagoonTearDown ();
	}
}
