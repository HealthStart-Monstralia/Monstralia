using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesReviewSenseButton : MonoBehaviour {
    public DataType.Senses typeOfSense;

    public void OnClick() {
        ReviewSensesGame.Instance.CheckSense (typeOfSense);
    }
}
