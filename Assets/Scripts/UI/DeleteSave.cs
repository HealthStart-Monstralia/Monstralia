using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSave : MonoBehaviour {
    public GameObject deleteSavePrefab;

    public void CreateBlockingPrompt () {
        Instantiate (deleteSavePrefab);
    }
}
