using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFireworksSystem : MonoBehaviour {
    public GameObject[] FireWorksPrefab;
    public GameObject[] LargeFireWorkPrefab;
    public Transform[] FireWorkTransform;
    public Transform LargeFireWorkTransform;

    public void CreateSmallFirework (Transform pos) {
        Instantiate (FireWorksPrefab.RandomItem(), pos.position, Quaternion.identity);
    }

    public void CreateLargeFirework (Transform pos) {
        Instantiate (LargeFireWorkPrefab.RandomItem (), pos.position, Quaternion.identity);
    }

    public void ActivateFireworks () {
        CreateSmallFirework (FireWorkTransform[0]);
        CreateSmallFirework (FireWorkTransform[1]);
        CreateSmallFirework (FireWorkTransform[2]);
        CreateSmallFirework (FireWorkTransform[3]);
        CreateLargeFirework(LargeFireWorkTransform);
    }
}
