using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPanel : MonoBehaviour {
    /* Requires the Star Panel to be a child of a MinigameButton or End Screen */
    public int numStars = 0;
    public Color missingColor, achievedColor;
    private GameObject[] stars = new GameObject[3];

    private void Start () {
        // Initialize stars array
        for (int i = 0; i < transform.childCount; i++) {
            stars[i] = transform.GetChild (i).gameObject;
            TurnOffStar (stars[i]);
        }

        if (GetComponentInParent<MinigameButton> ()) {
            DataType.Minigame minigame = GetComponentInParent<MinigameButton> ().typeOfGame;
            // Initialize numStars with amount of stars from Game Manager
            numStars = GameManager.GetInstance ().GetNumStars (minigame);
            for (int i = 0; i < numStars; ++i) {
                TurnOnStar (stars[i]);
            }
        }

        else if (GetComponentInParent<EndScreen> ()) {
            DataType.Minigame minigame = transform.parent.GetComponentInParent<EndScreen> ().typeOfGame;
            // Initialize numStars with amount of stars from Game Manager
            numStars = GameManager.GetInstance ().GetNumStars (minigame);
            for (int i = 0; i < numStars; ++i) {
                TurnOnStar (stars[i]);
            }
        }

        else {
            Debug.LogError ("StarPanel was not able to find a MinigameButton or End Screen script in the parent.");
        }

        // Show number of stars

    }

    void TurnOffStar(GameObject star) {
        star.GetComponent<Image>().color = missingColor;
    }

    void TurnOnStar (GameObject star) {
        star.GetComponent<Image> ().color = achievedColor;
    }
}
