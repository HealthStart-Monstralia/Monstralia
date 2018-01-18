using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowWaterManager : MonoBehaviour {
    public GameObject waterPrefab;
    public Transform[] waterSpawnLocations;
    public List<GameObject> waterBottleList = new List<GameObject> ();
    [HideInInspector] public float waterTimeBoost = 5f;

    private float waterSpawnTime = 0.0f;
    private bool spawnWater = false;

    void OnEnable () {
        BrainbowGameManager.OnGameStart += OnGameStart;
        BrainbowGameManager.OnGameEnd += OnGameEnd;
    }

    void OnDisable () {
        BrainbowGameManager.OnGameStart -= OnGameStart;
        BrainbowGameManager.OnGameEnd -= OnGameEnd;
    }

    void OnGameStart() {
        spawnWater = true;
        waterSpawnTime = Random.Range (5, 10);
    }

    void OnGameEnd() {
        print ("Water Manager game end");
        spawnWater = false;
        foreach (GameObject bottle in waterBottleList) {
            Destroy (bottle);
        }
    }

    public void CreateWater () {
        print ("Create Water");
        int selection = Random.Range (0, 4);
        GameObject water = Instantiate (
            waterPrefab,
            waterSpawnLocations[selection]
        );

        water.GetComponent<BrainbowWaterPickup> ().manager = this;
        water.GetComponent<BrainbowWaterPickup> ().waterTimeBoost = waterTimeBoost;
        waterBottleList.Add (water);
        water.transform.localPosition = new Vector3 (0f, 0f, 0f);
    }

    void Update () {
        if (BrainbowGameManager.Instance.GetGameStarted()) {
            if (waterSpawnTime > 0f && spawnWater) {
                if (waterBottleList.Count < 1) {
                    waterSpawnTime -= Time.deltaTime;
                }
            }

            // Create water bottle when waterSpawnTime is 0
            else if (waterSpawnTime <= 0f && spawnWater && waterBottleList.Count < 1) {
                waterSpawnTime = Random.Range (10f, 15f);
                CreateWater ();
            }
        }
    }
}
