using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * \class SoundManager
 * \brief This is the class that manages all sounds in the game.
 * 
 * A singleton class that will manipulate all sounds in the game.
 */

public class SoundManager : MonoBehaviour {
    [Header ("Audio Sources")]
    public AudioSource backgroundSource;            /*!< The AudioSource for the background music */
    public AudioSource ambientSource;            /*!< The AudioSource for the ambient sound */
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
    private static SoundManager instance = null;    /*!< The singleton instance of this class */
    private bool isMuted = false;                   /*!< Flag for if the sound has been muted */
    private bool isPlayingClip = false;				/*!< Flag to prevent a clip from playing more than once at a time */
    private List<AudioClip> clipQueue = new List<AudioClip> ();
    private bool isQueuePlaying = false;
    private Coroutine queueCoroutine, voiceCoroutine;
    private bool isPlayingVoiceOver = false;        /*!< Way to check if a voice over is already playing */
    private int sourceCount;                        /*!< Keep track of how many sfx sources there are */

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

	/**
	 * \brief Creates a SFX Source and play a single AudioClip through it. Only allows certain amount of sources.
	 * @param clip: the sound effect AudioClip to be played.
	 */
	public void PlaySFXClip(AudioClip clip) {
        if (sourceCount < Constants.NUM_OF_SFXSOURCES)
            StartCoroutine (PlaySFX (clip));
        else {
            print (string.Format("{0} was ignored due to exceeding maximum sourceCount ({1})", clip, Constants.NUM_OF_SFXSOURCES));
        }
	}

    IEnumerator PlaySFX (AudioClip clip) {
        isPlayingClip = true;
        AudioSource source = Instantiate (sfxSource, transform);
        sourceCount++;
        source.clip = clip;
        source.Play ();
        yield return new WaitForSeconds (clip.length);

        sourceCount--;
        isPlayingClip = false;
        Destroy (source.gameObject);
    }

    /**
	 * \brief Play a single AudioClip through the SFXsource with a coroutine.
	 * @param clip: the sound effect AudioClip to be played.
	 */
    public void PlayVoiceOverClip(AudioClip clip) {
        if (isPlayingVoiceOver) {
            if (voiceCoroutine != null)
                StopCoroutine (voiceCoroutine);
            StopPlayingVoiceOver ();
        }
        voiceCoroutine = StartCoroutine (PlayVoiceOverClipCoroutine (clip));
	}

    IEnumerator PlayVoiceOverClipCoroutine (AudioClip clip) {
        voiceOverSource.clip = clip;
        voiceOverSource.Play ();
        isPlayingVoiceOver = true;
        yield return new WaitForSeconds(clip.length);
        isPlayingVoiceOver = false;
    }

    /**
	 * \brief Change the background music to a new clip.
	 * @param newBackgroundMusic: the AudioClip of the new background music.
	 */
    public void ChangeBackgroundMusic(AudioClip newBackgroundMusic) {
        if (!backgroundSource.clip.Equals (newBackgroundMusic) && newBackgroundMusic != null) {
            PlayBackgroundMusic (newBackgroundMusic);
        }
	}

    /**
     * \brief If a voice over is playing then stop it
     */
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

    /**
     * \brief Change the ambient sound to a new clip.
     * @param newAmbientSound: the AudioClip of the new ambient sound.
     */
    public void ChangeAmbientSound (AudioClip newAmbientSound) {
        if (newAmbientSound != null) {
            PlayAmbientSound (newAmbientSound);
        }
    }

    /**
    * \brief Change the volume of the sound effect helper.
    * @param newVolume: the float value of the new volume.
    */
    public IEnumerator ChangeSFXVolumeHelper (float newVolume, AudioClip testClip) {
        sfxSource.volume = newVolume;
        if (!isPlayingClip) {
            isPlayingClip = true;
            PlaySFXClip (testClip);
        }
        yield return new WaitForSeconds (testClip.length);
        isPlayingClip = false;
    }

    /**
	 * \brief Change the voice over volume.
	 * @param newVolume: the float value of the new volume.
	 */
    public void ChangeVoiceOverVolume(float newVolume) {
		if(!isPlayingVoiceOver) {
			StartCoroutine(ChangeVoiceOverVolumeHelper(newVolume));
		}
	}

    /**
	 * \brief Change the volume of the voice over helper.
	 * @param newVolume: the float value of the new volume.
	 */
    private IEnumerator ChangeVoiceOverVolumeHelper(float newVolume) {
		voiceOverSource.volume = newVolume;
		if(!isPlayingVoiceOver) {
            isPlayingVoiceOver = true;
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
    public void PlayBackgroundMusic (AudioClip newBackgroundMusic) {
		backgroundSource.Play ();
	}

    /**
     * \brief Play the background music
     */
    public void StopBackgroundMusic () {
        backgroundSource.Stop ();
    }

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
	 * \brief Teardown the sounds in Brainstorm Lagoon and prepare sounds for MainMap
	 */
    public void LagoonTearDown(bool toMainMap) {
		//only play the original bg music if returning to MainMap
		if(toMainMap) 
			ChangeBackgroundMusic(gameBackgroundMusic);
	}

    /**
     * \brief Play the voice over associated with unlocking a sticker
     */
    public void PlayUnlockStickerVO() {
		PlayVoiceOverClip (stickerVO);
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

    /**
     * \brief Play the voice over associated with starting a review game
     */
    public void PlayReviewVO () {
        PlayVoiceOverClip (reviewVO);
    }

    /**
     * \brief Play the next voice over in the queue
     */
    public void AddToVOQueue(AudioClip clip) {
        clipQueue.Add (clip);
        if (!isQueuePlaying) {
            queueCoroutine = StartCoroutine (PlayQueue ());
        }
    }

    public void StopVOQueue() {
        if (isQueuePlaying) {
            StopCoroutine (queueCoroutine);
        }
    }

    /**
     * \brief Play the queue
     */
    IEnumerator PlayQueue () {
        //print ("PlayQueue");
        isQueuePlaying = true;
        while (clipQueue.Count > 0) {
            AudioClip clip = clipQueue[0];
            //print ("clipQueue[0]: " + clipQueue[0]);
            PlayVoiceOverClip (clip);
            yield return new WaitForSeconds (clip.length);
            clipQueue.RemoveAt (0);
        }
        isQueuePlaying = false;
        //print ("PlayQueue Stop");
    }

    public bool GetIsPlayingClip() {
        return isPlayingClip;
    }

    public bool GetIsPlayingVoiceOver () {
        return isPlayingVoiceOver;
    }
}
