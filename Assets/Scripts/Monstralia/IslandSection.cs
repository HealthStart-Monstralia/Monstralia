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
        GameManager.Instance.SetIslandSection (island);
    }

    private void Start () {
        CreateMonsterOnMap ();
        //PlayWelcomeVO ();
        SoundManager.Instance.StopAmbientSound ();
        SoundManager.Instance.ChangeAmbientSound (ambientSound);
        SoundManager.Instance.ChangeAndPlayMusic (backgroundMusic);
    }

    public void PlayWelcomeVO() {
        if (!GameManager.Instance.GetVisitedArea (island)) {
            if (introAudio != null)
                SoundManager.Instance.PlayVoiceOverClip (introAudio);
            GameManager.Instance.SetVisitedArea (island, true);
        } else {
            if (welcomeBackClip != null)
                SoundManager.Instance.PlayVoiceOverClip (welcomeBackClip);
        }
    }

    void CreateMonsterOnMap() {
        monster = GameManager.Instance.GetPlayerMonsterType ();
        Instantiate (monster, monsterLocation.transform);
    }
}
