﻿using System.Collections;
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
        BoneBridgeManager.Instance.GameStart ();
        BoneBridgeManager.Instance.CameraSwitch (focus);
    }

    void SetupSection() {
        SoundManager.Instance.PlayCorrectSFX ();
        BoneBridgeManager.Instance.CameraSwitch (focus);
        BoneBridgeManager.Instance.ChangePhase (BoneBridgeManager.BridgePhase.Building);
        BoneBridgeManager.Instance.AddTime (10f);
    }

    void WinGame() {
        BoneBridgeManager.Instance.GameEnd ();
        if (focus)
            BoneBridgeManager.Instance.CameraSwitch (focus);
    }

    void RestartBridge() {
        BoneBridgeManager.Instance.ChangePhase (BoneBridgeManager.BridgePhase.Falling);
        BoneBridgeManager.Instance.ResetMonster (startPos.transform.position, focus);
    }
}
