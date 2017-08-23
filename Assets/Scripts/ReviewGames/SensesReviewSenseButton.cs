using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesReviewSenseButton : MonoBehaviour {

    ReviewGameWinLose winLose;
    SensesReviewSenseItem item;

    private void Start() {
        item = FindObjectOfType<SensesReviewSenseItem>();
        winLose = FindObjectOfType<ReviewGameWinLose>();
    }
    public void Touch() {
        for (int i = 0; i < item.touchImages.Length; i++) {
            if (item.touchImages[i] == item.mySprite) {
               winLose.WinCondition();
                return;
            }
        }
        winLose.LoseCondition();
    }
    public void Smell() {
        for (int i = 0; i < item.smellImages.Length; i++) {
            if (item.smellImages[i] == item.mySprite) {
                winLose.WinCondition();
                return;
            }
        }
        winLose.LoseCondition();
    }
    public void Hear() {
        for (int i = 0; i < item.hearImages.Length; i++) {
            if (item.hearImages[i] == item.mySprite) {
                winLose.WinCondition();
                return;
            }
        }
        winLose.LoseCondition();
    }
    public void Taste() {
        for (int i = 0; i < item.tasteImages.Length; i++) {
            if (item.tasteImages[i] == item.mySprite) {
                winLose.WinCondition();
                return;
            }
        }
        winLose.LoseCondition();
    }
    public void See() {
        for (int i = 0; i < item.seeImages.Length; i++) {
            if (item.seeImages[i] == item.mySprite) {
                winLose.WinCondition();
                return;
            }
        }
        winLose.LoseCondition();
    }
}
