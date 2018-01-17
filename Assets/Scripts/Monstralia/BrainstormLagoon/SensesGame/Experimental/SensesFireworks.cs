using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFireworks : MonoBehaviour {
    public GameObject[] FireWorksPrefab;
    public GameObject[] LargeFireWorkPrefab;
    public Transform[] FireWorkTransform;
    public Transform LargeFireWorkTransform;

    private int randInt1;
    private int randInt2;
    private int randInt3;
    private int randInt4;
    private int randInt5;

    private void Start () {

    }

    public void ActivateFireworks () {
        randInt1 = Random.Range (0, FireWorksPrefab.Length);
        randInt2 = Random.Range (0, FireWorksPrefab.Length);
        randInt3 = Random.Range (0, FireWorksPrefab.Length);
        randInt4 = Random.Range (0, FireWorksPrefab.Length);
        randInt5 = Random.Range (0, LargeFireWorkPrefab.Length);
        Instantiate (FireWorksPrefab[randInt1], FireWorkTransform[0].transform.position, Quaternion.identity);
        Instantiate (FireWorksPrefab[randInt2], FireWorkTransform[1].transform.position, Quaternion.identity);
        Instantiate (FireWorksPrefab[randInt3], FireWorkTransform[2].transform.position, Quaternion.identity);
        Instantiate (FireWorksPrefab[randInt4], FireWorkTransform[3].transform.position, Quaternion.identity);
        Instantiate (LargeFireWorkPrefab[randInt5], LargeFireWorkTransform.transform.position, Quaternion.identity);
    }
}
