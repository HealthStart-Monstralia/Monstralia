using UnityEngine;
using System.Collections;

public class PlayVoiceOver : MonoBehaviour {

	public AudioClip clipToPlay;

	// Use this for initialization
	void Start () {
		SoundManager.GetInstance().PlayVoiceOverClip(clipToPlay);
	}
}
