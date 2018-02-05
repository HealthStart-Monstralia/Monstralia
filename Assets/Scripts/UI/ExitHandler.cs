using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added by Game Manager if platform is Android
public class ExitHandler : MonoBehaviour {
    public GameObject exitPrefab;

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!ExitSystem.Instance) {
                Instantiate (exitPrefab);
            } else {
                ExitSystem.Instance.OnExitNo ();
            }
        }
    }

}
