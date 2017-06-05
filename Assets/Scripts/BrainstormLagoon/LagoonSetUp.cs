using UnityEngine;
using System.Collections;

public class LagoonSetUp : MonoBehaviour {

	public AudioClip lagoonBGMusic;

	// Use this for initialization
	void Awake () {
		Setup ();
	}

	void Setup ()
	{
		SoundManager.GetInstance().LagoonSetup (lagoonBGMusic);
	}

	public void Teardown(bool toMainMap) {
		GameManager.GetInstance ().PlayLagoonVoiceOver = true;
		SoundManager.GetInstance().LagoonTearDown (toMainMap);
	}
}
