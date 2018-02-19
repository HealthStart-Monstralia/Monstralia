using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinEnemy : MonoBehaviour {
    private void OnEnable () {
        CatchToxinsManager.OnGameEnd += DestroyEnemy;
    }

    private void OnDisable () {
        CatchToxinsManager.OnGameEnd -= DestroyEnemy;
    }

    public void DestroyEnemy () {
        Destroy (gameObject);
    }
}
