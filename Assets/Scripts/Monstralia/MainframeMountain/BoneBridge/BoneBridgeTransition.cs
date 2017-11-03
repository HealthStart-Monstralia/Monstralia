using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeTransition : MonoBehaviour {
    public GameObject focus, waypoint, startPos;
    public int section;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Monster") {
            SetupSection();
            //Destroy (gameObject);
        }
    }

    public void ResetMonster() {
        BoneBridgeManager.GetInstance ().ResetMonster (startPos.transform.position);
        SetupSection();
    }

    void SetupSection() {
        BoneBridgeManager.GetInstance ().CameraSwitch (focus);
        BoneBridgeManager.GetInstance ().ChangeWaypoint (waypoint);
        BoneBridgeManager.GetInstance ().ChangePhase (BoneBridgeManager.BridgePhase.Building);
    }
}
