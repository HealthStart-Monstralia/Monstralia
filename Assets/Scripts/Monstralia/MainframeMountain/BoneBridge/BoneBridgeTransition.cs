using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeTransition : MonoBehaviour {
    public enum SectionType {
        StartZone = 0,
        PlayZone = 1,
        WinZone = 2,
        FallZone = 3
    }
    public GameObject focus, startPos;
    public SectionType typeOfSection;

    // Assign a monster responsible for this sector

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Monster" && collision.GetComponentInParent<BoneBridgeMonster>()) {
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
                case SectionType.FallZone:
                    RestartBridge ();
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
        BoneBridgeManager.GetInstance ().AddTime (10f);
    }

    void WinGame() {
        BoneBridgeManager.GetInstance ().GameOver ();
        if (focus)
            BoneBridgeManager.GetInstance ().CameraSwitch (focus);
    }

    void RestartBridge() {
        BoneBridgeManager.GetInstance ().ChangePhase (BoneBridgeManager.BridgePhase.Falling);
        BoneBridgeManager.GetInstance ().ResetMonster (startPos.transform.position, focus);
    }
}
