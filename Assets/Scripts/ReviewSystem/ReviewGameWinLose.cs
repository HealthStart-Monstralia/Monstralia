using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewGameWinLose : MonoBehaviour {

    private static ReviewGameWinLose instance;

    public static ReviewGameWinLose GetInstance() {
        return instance;
    }

    private void Start() {
        instance = this;
    }

    public void WinCondition() {
        print("Win");
        if (FindObjectOfType<MemoryMatchGameManager>()) {
            FindObjectOfType<MemoryMatchGameManager>().PregameSetup();
            Destroy(GameObject.FindGameObjectWithTag("ReviewPrefab"));
        } else if (FindObjectOfType<EmotionsGameManager>()) {
            FindObjectOfType<EmotionsGameManager>().PregameSetup();
            Destroy(GameObject.FindGameObjectWithTag("ReviewPrefab"));
        } else if (FindObjectOfType<BrainbowGameManager>()) {
            FindObjectOfType<BrainbowGameManager>().PregameSetup();
            Destroy(GameObject.FindGameObjectWithTag("ReviewPrefab"));
        } else if (FindObjectOfType<MasterHandler_LJ>()) {
            FindObjectOfType<MasterHandler_LJ>().PregameSetup();
            Destroy(GameObject.FindGameObjectWithTag("ReviewPrefab"));
        }
    }
    public void LoseCondition() {
        print("Lose");
    }
}
