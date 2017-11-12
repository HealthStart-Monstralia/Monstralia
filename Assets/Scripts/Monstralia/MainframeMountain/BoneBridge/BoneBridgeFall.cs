using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeFall : MonoBehaviour {
    public GameObject restartPosition, cameraPosition;
    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Monster") {
            BoneBridgeManager.GetInstance ().ChangePhase (BoneBridgeManager.BridgePhase.Falling);
            BoneBridgeManager.GetInstance ().ResetMonster (restartPosition.transform.position, cameraPosition);
        }
    }
}
