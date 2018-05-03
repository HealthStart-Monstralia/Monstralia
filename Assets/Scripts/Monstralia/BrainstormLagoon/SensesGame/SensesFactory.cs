using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFactory : Factory {
    public GameObject currentItem;

    public override GameObject Manufacture (GameObject prefab, Transform parent) {
        currentItem = Instantiate (prefab, parent);
        currentItem.name = prefab.name;
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localScale = scale;
        currentItem.AddComponent<SensesFirework> ();
        currentItem.AddComponent<SensesClickInput> ();
        return currentItem;
    }

    public override GameObject ManufactureRandom () {
        GameObject chosenItem = SelectRandom ();
        if (currentItem) {
            if (currentItem.name != chosenItem.name) {
                print (string.Format ("current Item: {0} chosenItem: {1}", currentItem, chosenItem));
                RemoveCurrentObject ();
                return Manufacture (chosenItem);
            } else {
                return currentItem;
            }
        }
        else {
            return Manufacture (chosenItem);
        }

    }

    public void RemoveCurrentObject () {
        Destroy (currentItem);
    }
}
