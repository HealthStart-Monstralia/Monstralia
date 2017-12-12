using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsMonster : MonoBehaviour {
    private Animator animComp;
    private RuntimeAnimatorController controller;

    void Awake() {
        animComp = gameObject.AddComponent<Animator> ();
        animComp.runtimeAnimatorController = controller;
    }
}
