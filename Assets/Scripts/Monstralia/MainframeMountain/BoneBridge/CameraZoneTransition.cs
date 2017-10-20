using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneTransition : MonoBehaviour {
    public GameObject focus;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Monster") {
            print ("Camera switch");
            BoneBridgeManager.GetInstance ().CameraSwitch (focus);
            BoneBridgeManager.GetInstance ().SwitchGoal (1);
            Destroy (gameObject);
        }
    }
}
