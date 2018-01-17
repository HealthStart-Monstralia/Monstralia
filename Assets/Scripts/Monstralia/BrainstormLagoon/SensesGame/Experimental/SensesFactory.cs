using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFactory : MonoBehaviour {
    [SerializeField] private GameObject[] sensesPrefabs;

    public GameObject SelectRandomPrefab() {
        return sensesPrefabs.RandomItem();
    }

    public GameObject Manufacture (GameObject prefab) {
        GameObject item = Instantiate (prefab, transform);
        item.name = prefab.name;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one * 0.5f;
        item.AddComponent<SensesFirework> ();
        return item;
    }

    public GameObject Manufacture (GameObject prefab, Transform parent) {
        GameObject item = Instantiate (prefab, parent);
        item.name = prefab.name;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one * 0.5f;
        item.AddComponent<SensesFirework> ();
        return item;
    }

    public GameObject ManufactureRandomPrefab () {
        return Manufacture (SelectRandomPrefab ());
    }
}
