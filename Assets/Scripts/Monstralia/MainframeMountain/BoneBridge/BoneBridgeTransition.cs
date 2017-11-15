using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeTransition : MonoBehaviour {
    public enum SectionType {
        StartZone = 0,
        PlayZone = 1,
        WinZone = 2
    }
    public GameObject focus, startPos;
    public SectionType typeOfSection;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Monster") {
            switch (typeOfSection) {
                case SectionType.StartZone:
                    StartGame ();
                    break;
                case SectionType.PlayZone:
                    SetupSection ();
                    break;
                case SectionType.WinZone:
                    WinGame ();
                    break;
            }
        }
    }

    void StartGame() {
        print ("Starting game");
        BoneBridgeManager.GetInstance ().GameStart ();
        BoneBridgeManager.GetInstance ().CameraSwitch (focus);
    }

    void SetupSection() {
        SoundManager.GetInstance ().PlayCorrectSFX ();
        BoneBridgeManager.GetInstance ().CameraSwitch (focus);
        BoneBridgeManager.GetInstance ().ChangePhase (BoneBridgeManager.BridgePhase.Building);
    }

    void WinGame() {
        BoneBridgeManager.GetInstance ().GameOver ();
        if (focus)
            BoneBridgeManager.GetInstance ().CameraSwitch (focus);
    }
}
