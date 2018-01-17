using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSection : MonoBehaviour {
    public DataType.IslandSection island;
    public GameObject monsterLocation;
    [HideInInspector] public GameObject monster;
    public AudioClip introAudio, welcomeBackClip, ambientSound, backgroundMusic;
    public AudioClip[] voice;

    private void Awake () {
        GameManager.GetInstance ().SetIslandSection (island);
    }

    private void Start () {
        CreateMonsterOnMap ();
        //PlayWelcomeVO ();
        SoundManager.GetInstance ().StopAmbientSound ();
        SoundManager.GetInstance ().ChangeAmbientSound (ambientSound);
        SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusic);
    }

    public void PlayWelcomeVO() {
        if (!GameManager.GetInstance ().GetVisitedArea (island)) {
            if (introAudio != null)
                SoundManager.GetInstance ().PlayVoiceOverClip (introAudio);
            GameManager.GetInstance ().SetVisitedArea (island, true);
        } else {
            if (welcomeBackClip != null)
                SoundManager.GetInstance ().PlayVoiceOverClip (welcomeBackClip);
        }
    }

    void CreateMonsterOnMap() {
        monster = GameManager.GetInstance ().GetPlayerMonsterType ();
        Instantiate (monster, monsterLocation.transform);
    }
}
