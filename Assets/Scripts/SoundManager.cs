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
	private bool setup;								/*!< Flag for if the sound manager is being set up */
	private bool isPlayingClip = false;				/*!< Flag to prevent a clip from playing more than once at a time */
	public AudioClip gameBackgroundMusic;			/*!< The game's main background music */
	public bool isPlayingVoiceOver = false;

	public AudioSource backgroundSource;			/*!< The AudioSource for the background music */
	public AudioSource SFXsource;					/*!< The AudioSource for the sound effects */
	public AudioSource voiceOverSource;				/*!< The AudioSource for the voice-overs */
	public Slider volumeSlider;						/*!< The Slider that controls the background music volume */
	public AudioClip SFXtestClip;					/*!< AudioClip used to test ChangeSoundEffectsVolume */
	public Slider SFXslider;						/*!< The Slider that controls the sound effects volume */
	public AudioClip voiceTestClip;					/*!< AudioClip used to test ChangeVoiceOverVolume */
	public Slider VoiceOverSlider;					/*!< The Slider that controls the voice-over volume */
	public AudioClip stickerVO;						/*!< AudioClip used to tell the player they unlocked a sticker */
	public AudioClip correctSFX;

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
		VoiceOverSlider.value = voiceOverSource.volume;
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
	public void PlaySFXClip(AudioClip clip) {
			SFXsource.clip = clip;
			SFXsource.Play ();
	}

	public void PlayVoiceOverClip(AudioClip clip) {
		voiceOverSource.clip = clip;
		voiceOverSource.Play();
	}

	/**
	 * \brief Change the background music to a new clip.
	 * @param newBackgroundMusic: the AudioClip of the new background music.
	 */
	public void ChangeBackgroundMusic(AudioClip newBackgroundMusic) {
		if(!backgroundSource.clip.Equals(newBackgroundMusic)) {
			backgroundSource.clip = newBackgroundMusic;
			backgroundSource.Play ();
		}
	}

	// CT
	public void StopPlayingVoiceOver() {
		if (voiceOverSource.isPlaying)
			voiceOverSource.Stop();
	}

	/**
	 * \brief Change the background music volume.
	 * @param newVolume: the float value of the new volume.
	 */
	public void ChangeBackgroundVolume(float newVolume) {
		backgroundSource.volume = newVolume;
	}

	public void ChangeSFXVolume(float newVolume) {
		if(!isPlayingClip) {
			StartCoroutine(ChangeSFXVolumeHelper(newVolume));
		}
	}

	private IEnumerator ChangeSFXVolumeHelper(float newVolume) {
		SFXsource.volume = newVolume;
		if(!setup && !isPlayingClip) {
			isPlayingClip = true;
			PlaySFXClip(SFXtestClip);
		}
		yield return new WaitForSeconds(SFXtestClip.length);
		isPlayingClip = false;
	}

	public void ChangeVoiceOverVolume(float newVolume) {
		if(!isPlayingVoiceOver) {
			StartCoroutine(ChangeVoiceOverVolumeHelper(newVolume));
		}
	}

	private IEnumerator ChangeVoiceOverVolumeHelper(float newVolume) {
		voiceOverSource.volume = newVolume;
		if(!setup && !isPlayingVoiceOver) {
			isPlayingClip = true;
			PlayVoiceOverClip(voiceTestClip);
		}
		yield return new WaitForSeconds(voiceTestClip.length);
		isPlayingVoiceOver = false;
	}

	/**
	 * \brief Setup the sounds for Brainstorm Lagoon
	 * @param clips: an array of AudioClips to be played
	 */
	public void LagoonSetup (AudioClip lagoonBG) {
		ChangeBackgroundMusic(lagoonBG);
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
		//only play the original bg music if returning to MainMap
		if(toMainMap) 
			ChangeBackgroundMusic(gameBackgroundMusic);
	}

	public void PlayUnlockStickerVO() {
		PlayVoiceOverClip (stickerVO);
	}

	public void PlayCorrectSFX() {
		PlaySFXClip (correctSFX);
	}
}
