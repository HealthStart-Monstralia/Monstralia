using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSection : MonoBehaviour {
    public DataType.IslandSection island;
    public GameObject monsterLocation;
    [HideInInspector] public Monster monster;
    public AudioClip introAudio, welcomeBackClip, ambientSound, backgroundMusic;

    private void Start () {
        CreateMonsterOnMap ();
        SoundManager.Instance.StopPlayingVoiceOver ();
        GameManager.Instance.SetIslandSection (island);

        if (!GameManager.Instance.GetVisitedArea (island)) {
            PlayFirstVisitWelcome ();
        }
        else {
            SoundManager.Instance.PlayVoiceOverClip (welcomeBackClip);
        }

        SoundManager.Instance.StopAmbientSound ();
        SoundManager.Instance.ChangeAmbientSound (ambientSound);
        SoundManager.Instance.ChangeAndPlayMusic (backgroundMusic);
    }

    public void PlayFirstVisitWelcome () {
        GameManager.Instance.SetVisitedArea (island, true);
        SoundManager.Instance.PlayVoiceOverClip (introAudio);
    }

    void CreateMonsterOnMap() {
        GameObject monsterObject = GameManager.Instance.GetPlayerMonsterObject ();
        monster = Instantiate (monsterObject, monsterLocation.transform).GetComponent<Monster>();
        monster.AllowMonsterTickle = true;
        monster.IdleAnimationOn = true;
    }
}
