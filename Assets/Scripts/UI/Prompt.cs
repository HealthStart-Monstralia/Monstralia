using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Prompt : MonoBehaviour {
    public Button yesButton, noButton;

    public void OnClose () {
        yesButton.interactable = false;
        noButton.interactable = false;
    }
}
