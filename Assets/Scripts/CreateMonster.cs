using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {
	void Awake() {
        Instantiate (GameManager.GetInstance ().GetMonster (), transform);
    }
}
