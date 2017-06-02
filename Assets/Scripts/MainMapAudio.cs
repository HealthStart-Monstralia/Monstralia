using UnityEngine;
using System.Collections;

public class MainMapAudio : MonoBehaviour {

	public AudioClip introAudio;

	// Use this for initialization
	void Start () {
		SoundManager.GetInstance().PlayVoiceOverClip(introAudio);
	}

}
