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
        ReviewManager.Instance.EndReview ();
    }

    public void LoseCondition() {
        print("Lose");
    }
}
