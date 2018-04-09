using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    [Tooltip ("Starts with Prefabs/")]
    public string prefabPath;
    public bool usePrefabPath = true;

    public List<GameObject> pickupPrefabList = new List<GameObject> ();
    public Vector3 scale = new Vector3 (1f, 1f, 1f);

    private void Awake () {
        if (usePrefabPath)
            pickupPrefabList.AddRange (Resources.LoadAll<GameObject> ("Prefabs/Monstralia/" + prefabPath));
    }

    public virtual GameObject SelectRandom () {
        return pickupPrefabList.GetRandomItem ();
    }

    public virtual GameObject SelectRandomAndRemove () {
        return pickupPrefabList.RemoveRandom ();
    }

    public virtual GameObject ManufactureRandom () {
        return Manufacture (SelectRandom ());
    }

    public virtual GameObject ManufactureRandom (Transform parent) {
        return Manufacture (SelectRandom (), parent);
    }

    public virtual GameObject ManufactureRandomAndRemove () {
        return Manufacture (SelectRandomAndRemove ());
    }

    public virtual GameObject ManufactureRandomAndRemove (Transform parent) {
        return Manufacture (SelectRandomAndRemove (), parent);
    }

    public virtual GameObject Manufacture (GameObject prefab) {
        return Manufacture (prefab, transform);
    }

    public virtual GameObject Manufacture (GameObject prefab, Transform parent) {
        GameObject item = Instantiate (prefab, parent);
        item.name = prefab.name;
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = scale;
        return item;
    }
}
