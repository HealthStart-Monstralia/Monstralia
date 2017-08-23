using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewGameCanvas : MonoBehaviour {

    private void Start() {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent <Camera>();
        Canvas myCanvas = GetComponent<Canvas>();
        myCanvas.sortingLayerName = "UI";
        myCanvas.worldCamera = mainCamera;
    }
}
