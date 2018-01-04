using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationTest : MonoBehaviour {
    public GameObject objToSpawn;
    public Transform placeToSpawn;

    private void Start () {
        print (placeToSpawn.position);
        Instantiate(objToSpawn, placeToSpawn.position, Quaternion.identity);
    }
}
