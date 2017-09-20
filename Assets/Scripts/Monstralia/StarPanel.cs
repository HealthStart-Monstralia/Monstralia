using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : MonoBehaviour {
    /* Requires the Star Panel to be a child of a MinigameButton */
    public int numStars;

    private GameObject[] stars = new GameObject[3];

    private void Awake () {
        // Initialize stars array
        for (int i = 0; i < transform.childCount; i++) {
            stars[i] = transform.GetChild (i).gameObject;
            stars[i].SetActive (false);
        }

        if (GetComponentInParent<MinigameButton> ()) {
            DataType.Minigame minigame = GetComponentInParent<MinigameButton> ().typeOfGame;
            // Initialize numStars with amount of stars from Game Manager
            numStars = GameManager.GetInstance ().GetNumStars (minigame);
        }
        else {
            Debug.LogError ("StarPanel was not able to find a MinigameButton script in the parent.");
        }

        // Show number of stars
        for (int i = 0; i < numStars; ++i) {
            stars[i].SetActive (true);
        }
    }
}
