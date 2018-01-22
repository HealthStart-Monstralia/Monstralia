using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFirework : MonoBehaviour {
    private SensesFireworksSystem fireworkSystem;

    private void Start () {
        fireworkSystem = SensesGameManager.Instance.fireworksSystem;
        CreateFirework ();
    }

    public void CreateFirework() {
        fireworkSystem.CreateSmallFirework (transform, true);
    }
}
