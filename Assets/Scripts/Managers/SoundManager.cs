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
    public AudioSource musicScore;                  /*!< The AudioSource for the background music */
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
    private bool isPlayingClip = false;				/*!< Flag to prevent a clip from playing more than once at a time */
    private Queue<AudioClip> clipQueue = new Queue<AudioClip> ();
    private bool isQueuePlaying = false;
    private Coroutine queueCoroutine, voiceCoroutine;
    private bool isPlayingVoiceOver = false;        /*!< Way to check if a voice over is already playing */
    private int sourceCount;                        /*!< Keep track of how many sfx sources there are */
		
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
	public AudioSource PlaySFXClip(AudioClip clip) {
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

    /**
	 * \brief Play a single AudioClip through the SFXsource with a coroutine.
	 * @param clip: the sound effect AudioClip to be played.
	 */
    public void PlayVoiceOverClip(AudioClip clip) {
        if (isPlayingVoiceOver) {
            StopPlayingVoiceOver ();
        }
        voiceCoroutine = StartCoroutine (PlayVoiceOverClipCoroutine (clip));
	}

    IEnumerator PlayVoiceOverClipCoroutine (AudioClip clip) {
        isPlayingVoiceOver = true;
        voiceOverSource.clip = clip;
        voiceOverSource.Play ();
        yield return new WaitForSeconds(clip.length);
        isPlayingVoiceOver = false;
    }

    /**
     * \brief If a voice over is playing then stop it
     */
    public void StopPlayingVoiceOver () {
        if (voiceOverSource.isPlaying) {
            print ("Stopping VO");
            voiceOverSource.Stop ();
        }

        if (isQueuePlaying) {
            print ("Stopping Q");
            clipQueue.Clear ();
        }
    }

    /**
	 * \brief Change the background music to a new clip.
	 * @param newBackgroundMusic: the AudioClip of the new background music.
	 */
    public void ChangeAndPlayMusic(AudioClip newMusic) {
        if (!musicScore.clip.Equals (newMusic) && newMusic != null) {
            PlayBackgroundMusic (newMusic);
        }
	}

	/**
	 * \brief Change the background music volume.
	 * @param newVolume: the float value of the new volume.
	 */
	public void ChangeBackgroundVolume(float newVolume) {
		musicScore.volume = newVolume;
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
		ChangeAndPlayMusic(lagoonBG);
	}

    /**
	 * \brief Play the background music
	 */
    void PlayBackgroundMusic (AudioClip newBackgroundMusic) {
        musicScore.clip = newBackgroundMusic;
		musicScore.Play ();
	}

    /**
     * \brief Play the background music
     */
    public void StopBackgroundMusic () {
        musicScore.Stop ();
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
			ChangeAndPlayMusic(gameBackgroundMusic);
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
        clipQueue.Enqueue (clip);
        if (!isQueuePlaying) {
            queueCoroutine = StartCoroutine (PlayQueue ());
        }
    }

    public void StopVOQueue() {
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
            AudioClip clip = clipQueue.Dequeue();
            PlayVoiceOverClip (clip);
            yield return new WaitForSeconds (clip.length);
        }
        isQueuePlaying = false;
    }

    public bool GetIsQueuePlaying () {
        return isQueuePlaying;
    }

    public bool GetIsPlayingClip() {
        return isPlayingClip;
    }

    public bool GetIsPlayingVoiceOver () {
        return isPlayingVoiceOver;
    }
}
