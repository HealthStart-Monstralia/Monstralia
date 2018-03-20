using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFactory : Factory {
    public GameObject currentItem;

    public override GameObject Manufacture (GameObject prefab, Transform parent) {
        currentItem = base.Manufacture (prefab, parent);
        currentItem.AddComponent<SensesFirework> ();
        return currentItem;
    }

    public void RemoveCurrentObject () {
        Destroy (currentItem);
    }
}
