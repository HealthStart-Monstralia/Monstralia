using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour {
    public GameObject toggleObject;
    public bool objectOffOnStart;

    private void Awake () {
        if (objectOffOnStart) toggleObject.SetActive(false);
    }

    public void Toggle() {
        toggleObject.SetActive(!toggleObject.activeSelf);
    }
}
