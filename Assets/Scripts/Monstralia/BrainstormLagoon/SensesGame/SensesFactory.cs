using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFactory : Factory {
    public override GameObject Manufacture (GameObject prefab, Transform parent) {
        GameObject item = Instantiate (prefab, parent);
        item.name = prefab.name;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = scale;
        item.AddComponent<SensesFirework> ();
        return item;
    }
}
