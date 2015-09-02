using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;
	bool muted = false;

	AudioSource backgroundMusic;
	
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
	}

	public void mute() {
		if(!muted) {
			AudioListener.pause = true;
			muted = true;
		}
		else {
			AudioListener.pause = false;
			muted = false;
		}
	}

}
