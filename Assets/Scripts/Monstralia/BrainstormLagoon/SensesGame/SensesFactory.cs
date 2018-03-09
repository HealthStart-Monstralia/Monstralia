using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFactory : Factory {

    public override GameObject Manufacture (GameObject prefab, Transform parent) {
        GameObject item = base.Manufacture (prefab, parent);
        item.AddComponent<SensesFirework> ();
        return item;
    }
}
