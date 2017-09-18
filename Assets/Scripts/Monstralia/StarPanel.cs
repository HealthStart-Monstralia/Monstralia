using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : MonoBehaviour {
    public DataType.Minigame minigame;
    public int numStars;

    private GameObject[] stars = new GameObject[3];

    private void Awake () {
        // Initialize stars array
        for (int i = 0; i < transform.childCount; i++) {
            stars[i] = transform.GetChild (i).gameObject;
            stars[i].SetActive (false);
        }

        // Initialize numStars with amount of stars from Game Manager
        numStars = GameManager.GetInstance ().GetNumStars (minigame);

        // Show number of stars
        for (int i = 0; i < numStars; ++i) {
            stars[i].SetActive (true);
        }
    }
}
