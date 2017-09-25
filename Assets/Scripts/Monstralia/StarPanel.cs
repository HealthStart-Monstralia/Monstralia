using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : MonoBehaviour {
    /* Requires the Star Panel to be a child of a MinigameButton or End Screen */
    public int numStars = 0;

    private GameObject[] stars = new GameObject[3];

    private void Start () {
        // Initialize stars array
        for (int i = 0; i < transform.childCount; i++) {
            stars[i] = transform.GetChild (i).gameObject;
            stars[i].SetActive (false);
        }

        if (GetComponentInParent<MinigameButton> ()) {
            DataType.Minigame minigame = GetComponentInParent<MinigameButton> ().typeOfGame;
            // Initialize numStars with amount of stars from Game Manager
            numStars = GameManager.GetInstance ().GetNumStars (minigame);
            for (int i = 0; i < numStars; ++i) {
                stars[i].SetActive (true);
            }
        }

        else if (GetComponentInParent<EndScreen> ()) {
            DataType.Minigame minigame = transform.parent.GetComponentInParent<EndScreen> ().typeOfGame;
            // Initialize numStars with amount of stars from Game Manager
            numStars = GameManager.GetInstance ().GetNumStars (minigame);
            for (int i = 0; i < numStars; ++i) {
                stars[i].SetActive (true);
            }
        }

        else {
            Debug.LogError ("StarPanel was not able to find a MinigameButton or End Screen script in the parent.");
        }

        // Show number of stars

    }
}
