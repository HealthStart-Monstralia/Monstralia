using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * \class SoundManager
 * \brief This is the class that manages all sounds in the game.
 * 
 * A singleton class that will manipulate all sounds in the game.
 */

public class SoundManager : MonoBehaviour {

	private static SoundManager instance = null; 	/*!< The singleton instance of this class */
	private bool muted = false;						/*!< Flag for if the sound has been muted */
	private bool setup;

	public AudioClip gameBackgroundMusic;			/*!< The main background music for the game */
	public AudioSource backgroundSource;			/*!< The AudioSource for the background music */
	public AudioSource SFXsource;					/*!< The AudioSource for the sound effects */
	public Slider volumeSlider;						/*!< The Slider that controls the background music volume*/
	public AudioClip testClip;						/*!< AudioClip used to test ChangeSoundEffectsVolume*/
	public Slider SFXslider;						/*!< The Slider that controls the sound effects volume*/

	/** \cond */
	void Awake () {
		//check to see if we already have an instance of the SoundManager
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
	}

	void Start() {
		setup = true;
		//set the value of the volume slider
		volumeSlider.value = backgroundSource.volume;
		SFXslider.value = SFXsource.volume;
		setup = false;
	}
	/** /endcond */

	/**
	 * \brief Access the singleton instance of the class.
	 * @return the singleton instance of the SoundManager class.
	 */ 
	public static SoundManager GetInstance() {
		return instance;
	}

	/**
	 * \brief Mute the game music.
	 */ 
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

	/**
	 * \brief Play a single AudioClip through the SFXsource.
	 * @param clip: the sound effect AudioClip to be played.
	 */
	public void PlayClip(AudioClip clip) {
		SFXsource.clip = clip;
		SFXsource.Play ();
	}

	/**
	 * \brief Change the background music to a new clip.
	 * @param newBackgroundMusic: the AudioClip of the new background music.
	 */
	public void ChangeBackgroundMusic(AudioClip newBackgroundMusic) {
		backgroundSource.clip = newBackgroundMusic;
		backgroundSource.Play ();
	}

	/**
	 * \brief Change the background music volume.
	 * @param newVolume: the float value of the new volume.
	 */
	public void ChangeBackgroundVolume(float newVolume) {
		backgroundSource.volume = newVolume;
	}

	public void ChangeSoundEffectsVolume(float newVolume) {
		SFXsource.volume = newVolume;
		if(!setup) {
			PlayClip(testClip);
		}
	}

	/**
	 * \brief Setup the sounds for Brainstorm Lagoon
	 * @param clips: an array of AudioClips to be played
	 */
	public void LagoonSetup (AudioClip[] clips) {

		if(!muted) {
			backgroundSource.Stop ();
		}
		//create a new AudioSource to play each clip
		foreach(AudioClip clip in clips) {
			AudioSource newSource = gameObject.AddComponent<AudioSource>();
			newSource.clip = clip;
			newSource.loop = true;
			newSource.playOnAwake = true;
			newSource.volume = 0.5f;
			newSource.Play();
		}

	}

	/**
	 * \brief Play the background music
	 */
	void PlayBackgroundMusic() {
		backgroundSource.Play ();
	}

	/**
	 * \brief Teardown the sounds in Brainstorm Lagoon and prepare sounds for MainMap
	 */
	public void LagoonTearDown(bool toMainMap) {
		AudioSource[] sources = gameObject.GetComponents<AudioSource>();
		//get rid of all AudioSources except the original bg source and SFX source
		foreach(AudioSource source in sources) {
			if(source != backgroundSource && source != SFXsource) {
				Destroy(source);
			}
		}
		//only play the original bg music if returning to MainMap
		if(toMainMap)
			PlayBackgroundMusic();
	}

}
