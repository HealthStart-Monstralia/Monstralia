using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * \class SoundManager
 * \brief This is the class that manages all sounds in the game.
 * 
 * A singleton class that will manipulate all sounds in the game.
 */

public class SoundManager : SingletonPersistent<SoundManager> {
    [Header ("Audio Sources")]
    public AudioSource musicSource;                 /*!< The AudioSource for the background music */
    public AudioSource ambientSource;               /*!< The AudioSource for the ambient sound */
    public AudioSource sfxSource;					/*!< The AudioSource for the sound effects */
	public AudioSource voiceOverSource;             /*!< The AudioSource for the voice-overs */

    [Header ("Audio Clips")]
    public AudioClip gameBackgroundMusic;           /*!< The game's main background music */
    public AudioClip voiceTestClip;					/*!< AudioClip used to test ChangeVoiceOverVolume */
	public AudioClip stickerVO;						/*!< AudioClip used to tell the player they unlocked a sticker */
	public AudioClip correctSfx;                    /*!< AudioClip used to tell the player they are correct */
    public AudioClip correctSfx2;                   /*!< AudioClip used to tell the player they are correct */
    public AudioClip reviewVO;                      /*!< AudioClip used to tell the player they are reviewing */

    [SerializeField] private AudioClip incorrectSfx;

    private bool isMuted = false;                   /*!< Flag for if the sound has been muted */
    private bool isPlayingClip = false;             /*!< Flag to prevent a clip from playing more than once at a time */
    private bool isPlayingVoiceOver = false;        /*!< Way to check if a voice over is already playing */
    private bool isQueuePlaying = false;
    private Queue<AudioClip> clipQueue = new Queue<AudioClip> ();
    private Coroutine queueCoroutine, voiceCoroutine;
    private int sourceCount;                        /*!< Keep track of how many sfx sources there are */

    // PlayerPref variables
    private string prefMusicVolume = "musicVolume";
    private string prefAmbientVolume = "ambientVolume";
    private string prefSfxVolume = "sfxVolume";
    private string prefVoiceOverVolume = "voiceOverVolume";

    private new void Awake () {
        base.Awake ();

        // Retrieve music volume setting, if does not exist create a new entry
        CheckAndLoadAudioVolumeFromPrefs (musicSource, prefMusicVolume);

        // Retrieve ambient sound volume setting, if does not exist create a new entry
        CheckAndLoadAudioVolumeFromPrefs (ambientSource, prefAmbientVolume);

        // Retrieve sound effects volume setting, if does not exist create a new entry
        CheckAndLoadAudioVolumeFromPrefs (sfxSource, prefSfxVolume);

        // Retrieve voice over volume setting, if does not exist create a new entry
        CheckAndLoadAudioVolumeFromPrefs (voiceOverSource, prefVoiceOverVolume);
    }

    /**
	 * \brief Mute the game music.
	 */
    public void Mute() {
		if(!isMuted) {
			AudioListener.pause = true;
            isMuted = true;
		}
		else {
			AudioListener.pause = false;
            isMuted = false;
		}
	}

    #region BackgroundMusic

    /**
     * \brief Stop the background music
     */
    public void StopBackgroundMusic () {
        musicSource.Stop ();
    }

    /**
     * \brief Play the background music
     */
    public void PlayBackgroundMusic () {
        if (!musicSource.isPlaying) {
            musicSource.Play ();
        }
    }

    /**
	 * \brief Change the background music to a new clip.
	 * @param newBackgroundMusic: the AudioClip of the new background music.
	 */
    public void ChangeAndPlayMusic(AudioClip newMusic) {
        if (!musicSource.clip.Equals (newMusic) && newMusic != null) {
            musicSource.clip = newMusic;
        }
        PlayBackgroundMusic ();
    }

    /**
     * \brief Return the background music AudioSource
     */
    public AudioSource GetMusicSource () {
        return musicSource;
    }

	/**
	 * \brief Change the background music volume.
	 * @param newVolume: the float value of the new volume.
	 */
	public void ChangeBackgroundVolume(float newVolume) {
		musicSource.volume = newVolume;
        SaveAudioVolumeInPrefs (musicSource, prefMusicVolume);
    }

    #endregion

    #region AmbientSound

    /**
     * \brief Play the ambient sound
     */
    public void PlayAmbientSound (AudioClip newAmbientSound) {
        ambientSource.clip = newAmbientSound;
        ambientSource.Play ();
    }

    /**
     * \brief Stop the ambient sound
     */
    public void StopAmbientSound () {
        ambientSource.Stop ();
    }

    /**
     * \brief Change the ambient sound to a new clip.
     * @param newAmbientSound: the AudioClip of the new ambient sound.
     */
    public void ChangeAmbientSound (AudioClip newAmbientSound) {
        if (newAmbientSound != null && newAmbientSound != ambientSource.clip) {
            PlayAmbientSound (newAmbientSound);
        }
        else if (newAmbientSound == null) {
            StopAmbientSound ();
        }
    }

    /**
     * \brief Change the ambient sound volume.
     * @param newVolume: the float value of the new volume.
     */
    public void ChangeAmbientVolume (float newVolume) {
        ambientSource.volume = newVolume;
        SaveAudioVolumeInPrefs (ambientSource, prefAmbientVolume);
    }

    /**
     * \brief Return the ambient sound AudioSource
     */
    public AudioSource GetAmbientSource () {
        return ambientSource;
    }

    #endregion

    #region SoundEffects
    //      NOTE: SoundManager handles sound effects in a unique way, it creates instances of SFX Source for each sound 
    //  that requests to play a sound effect up to a hardcoded amount of NUM_OF_SFXSOURCES.

    /**
	 * \brief Creates a SFX Source and play a single AudioClip through it. Only allows certain amount of sources.
	 * @param clip: the sound effect AudioClip to be played.
	 */
    public AudioSource PlaySFXClip (AudioClip clip) {
        if (sourceCount < Constants.NUM_OF_SFXSOURCES) {
            AudioSource source = Instantiate (sfxSource, transform);
            sourceCount++;
            source.clip = clip;
            StartCoroutine (UseSFXSource (source));
            return source;
        }
        print (string.Format ("{0} was ignored due to exceeding maximum sourceCount ({1})", clip, Constants.NUM_OF_SFXSOURCES));
        return null;
    }

    IEnumerator UseSFXSource (AudioSource source) {
        isPlayingClip = true;
        source.Play ();
        yield return new WaitForSeconds (source.clip.length);

        sourceCount--;
        isPlayingClip = false;
        Destroy (source.gameObject);
    }

    public bool GetIsPlayingClip () {
        return isPlayingClip;
    }

    /**
    * \brief Change the volume of the sound effect source.
    * @param newVolume: the float value of the new volume.
    */
    public void ChangeSFXVolume (float newVolume) {
        sfxSource.volume = newVolume;
        SaveAudioVolumeInPrefs (sfxSource, prefSfxVolume);
    }

    /**
    * \brief Change the volume of the sound effect source and play a helper.
    * @param newVolume: the float value of the new volume.
    * @param testClip: the AudioClip to test with
    */
    public IEnumerator ChangeSFXVolumeHelper (float newVolume, AudioClip testClip) {
        ChangeSFXVolume (newVolume);
        
        // Test SFX volume clip
        if (!isPlayingClip) {
            isPlayingClip = true;
            PlaySFXClip (testClip);
        }
        yield return new WaitForSeconds (testClip.length);
        isPlayingClip = false;
    }

    #endregion

    #region VoiceOvers
    /**
	 * \brief Play a single AudioClip through the SFXsource with a coroutine.
	 * @param clip: the sound effect AudioClip to be played.
	 */
    public void PlayVoiceOverClip (AudioClip clip) {
        if (clip != null) {
            if (isPlayingVoiceOver) {
                StopPlayingVoiceOver ();
            }
            StartCoroutine (PlayVoiceOverClipCoroutine (clip));
        }
    }

    IEnumerator PlayVoiceOverClipCoroutine (AudioClip clip) {
        isPlayingVoiceOver = true;
        voiceOverSource.clip = clip;
        voiceOverSource.Play ();
        yield return new WaitForSeconds (clip.length);
        isPlayingVoiceOver = false;
    }

    /**
     * \brief If a voice over is playing then stop it
     */
    public void StopPlayingVoiceOver () {
        if (voiceOverSource.isPlaying) {
            print ("Stopping VO");
            voiceOverSource.Stop ();
            isPlayingVoiceOver = false;
        }

        if (isQueuePlaying) {
            print ("Stopping Q");
            clipQueue.Clear ();
        }
    }

    public bool GetIsPlayingVoiceOver () {
        return isPlayingVoiceOver;
    }

    /**
     * \brief Change the voice over volume.
     * @param newVolume: the float value of the new volume.
     */
    public void ChangeVoiceOverVolume (float newVolume) {
        if (!isPlayingVoiceOver) {
            StartCoroutine (ChangeVoiceOverVolumeHelper (newVolume));
        }
    }

    /**
     * \brief Change the volume of the voice over helper.
     * @param newVolume: the float value of the new volume.
     */
    private IEnumerator ChangeVoiceOverVolumeHelper (float newVolume) {
        voiceOverSource.volume = newVolume;
        if (!isPlayingVoiceOver) {
            isPlayingVoiceOver = true;
            PlayVoiceOverClip (voiceTestClip);
        }
        yield return new WaitForSeconds (voiceTestClip.length);
        isPlayingVoiceOver = false;
    }

    #endregion

    #region VOQueue
    /**
     * \brief Play the next voice over in the queue
     */
    public void AddToVOQueue (AudioClip clip) {
        if (clip != null) {
            clipQueue.Enqueue (clip);
            if (!isQueuePlaying) {
                queueCoroutine = StartCoroutine (PlayQueue ());
            }
        }
    }

    public void StopVOQueue () {
        if (isQueuePlaying) {
            StopCoroutine (queueCoroutine);
            clipQueue.Clear ();
        }
    }

    /**
     * \brief Play the queue
     */
    IEnumerator PlayQueue () {
        isQueuePlaying = true;
        while (clipQueue.Count > 0) {
            AudioClip clip = clipQueue.Dequeue ();
            PlayVoiceOverClip (clip);
            yield return new WaitForSeconds (clip.length);
        }
        isQueuePlaying = false;
    }

    public bool GetIsQueuePlaying () {
        return isQueuePlaying;
    }

    public AudioSource GetVoiceOverSource () {
        return voiceOverSource;
    }

    #endregion

    #region PlayGenericSoundClips

    /**
     * \brief Play the voice over associated with unlocking a sticker
     */
    public void PlayUnlockStickerVO() {
		PlayVoiceOverClip (stickerVO);
	}

    /**
     * \brief Play the voice over associated with starting a review game
     */
    public void PlayReviewVO () {
        PlayVoiceOverClip (reviewVO);
    }

    /**
    * \brief Play a generic sound effect for when something is correct
    */
    public void PlayCorrectSFX() {
		PlaySFXClip (correctSfx);
	}

    /**
    * \brief Play a generic sound effect for when something is incorrect
    */
    public void PlayIncorrectSFX () {
        PlaySFXClip (incorrectSfx);
    }

    #endregion

    #region PlayerPrefs

    void CheckAndLoadAudioVolumeFromPrefs (AudioSource source, string prefsName) {
        if (!PlayerPrefs.HasKey (prefsName)) {
            SaveAudioVolumeInPrefs (source, prefsName);
        } else {
            LoadAudioVolumeFromPrefs (source, prefsName);
        }
    }

    void SaveAudioVolumeInPrefs(AudioSource source, string prefsName) {
        PlayerPrefs.SetFloat (prefsName, source.volume);
        PlayerPrefs.Save ();
    }

    void LoadAudioVolumeFromPrefs (AudioSource source, string prefsName) {
        source.volume = PlayerPrefs.GetFloat (prefsName);
    }

    #endregion
}
