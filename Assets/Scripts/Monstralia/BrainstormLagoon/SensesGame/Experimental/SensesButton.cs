using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesButton : MonoBehaviour {
    public DataType.Senses typeOfSense;

    public void OnPress() {
        SensesGameManager.GetInstance ().OnSense (typeOfSense);
    }
}
