using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFirework : MonoBehaviour {
    private SensesFireworksSystem fireworkSystem;

    private void Start () {
        fireworkSystem = SensesGameManager.GetInstance ().fireworksSystem;
        CreateFirework ();
    }

    public void CreateFirework() {
        fireworkSystem.CreateSmallFirework (transform);
    }
}
