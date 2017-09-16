using UnityEngine;
using System.Collections;

public class PlayVoiceOver : MonoBehaviour {

	public AudioClip clipToPlay;

	// Use this for initialization
	void Start () {
		SoundManager.GetInstance().StopPlayingVoiceOver();
		if (GameManager.GetInstance ().playLagoonVoiceOver) {
			SoundManager.GetInstance ().PlayVoiceOverClip (clipToPlay);
			GameManager.GetInstance ().playLagoonVoiceOver = false;
		}
	}
}
