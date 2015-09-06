using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;
	bool muted = false;

	public AudioSource backgroundMusic;
	
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

	public void LagoonSetup (AudioClip[] clips) {

		if(!muted) {
			backgroundMusic.Stop ();
		}
		foreach(AudioClip clip in clips) {
			AudioSource newSource = gameObject.AddComponent<AudioSource>();
			newSource.clip = clip;
			newSource.loop = true;
			newSource.playOnAwake = true;
			newSource.volume = 0.5f;
			newSource.Play();
		}

	}

	public void LagoonTearDown() {
		AudioSource[] sources = gameObject.GetComponents<AudioSource>();
		foreach(AudioSource source in sources) {
			if(source != backgroundMusic) {
				Destroy(source);
			}
		}

	}

}
