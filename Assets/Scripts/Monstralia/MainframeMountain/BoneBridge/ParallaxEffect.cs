using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour {
    [Range(0f,2f)]
    public float parallaxScale = 1f;
    public bool isParallaxOn = true;

    private Vector3 startPos;

    private void Awake () {
        startPos = transform.position;
    }

    private void LateUpdate () {
        if (isParallaxOn)
            transform.position = new Vector3 (
                Camera.main.transform.position.x * parallaxScale, 
                startPos.y, 
                startPos.z
                );
    }
}
