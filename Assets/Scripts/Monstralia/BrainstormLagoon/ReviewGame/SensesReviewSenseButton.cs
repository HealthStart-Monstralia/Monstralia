using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesReviewSenseButton : MonoBehaviour {
    public SensesReviewSenseItem.Senses typeOfSense;

    public void OnClick() {
        if (!ReviewSensesGame.GetInstance ().isGuessing) {
            ReviewSensesGame.GetInstance ().isGuessing = true;
            print ("typeOfSense: " + typeOfSense + " | Name: " + gameObject.name);
            ReviewSensesGame.GetInstance ().CheckSense (typeOfSense);
        }
    }
}
