using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesReviewSenseButton : MonoBehaviour {
    public SensesReviewSenseItem.Senses typeOfSense;

    public void OnClick() {
        if (!ReviewSensesGame.Instance.isGuessing) {
            ReviewSensesGame.Instance.isGuessing = true;
            print ("typeOfSense: " + typeOfSense + " | Name: " + gameObject.name);
            ReviewSensesGame.Instance.CheckSense (typeOfSense);
        }
    }
}
