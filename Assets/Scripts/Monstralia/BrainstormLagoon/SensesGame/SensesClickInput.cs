using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesClickInput : MonoBehaviour {

    private void OnMouseDown () {
        print ("Click");
        SensesGameManager.Instance.currentLevelManager.DoesObjectHaveSense (GetComponent<SensesItem> ());
    }
}
