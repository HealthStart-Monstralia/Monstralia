using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private bool isAnEnemySpawner;
    private float minimumSpawnDelay, maximumSpawnDelay;
    private bool startSpawning = false;
    private GameObject[] spawnArray;
    private Vector3 leftBoundary;
    private Vector3 rightBoundary;

    private void OnEnable () {
        CatchToxinsManager.OnGameStart += BeginSpawning;
        CatchToxinsManager.OnGameEnd += StopSpawning;
    }

    private void OnDisable () {
        CatchToxinsManager.OnGameStart-= BeginSpawning;
        CatchToxinsManager.OnGameEnd -= StopSpawning;
    }

    private void Start () {
        leftBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0.1f, 1, Camera.main.nearClipPlane));
        rightBoundary = Camera.main.ViewportToWorldPoint (new Vector3 (0.9f, 1, Camera.main.nearClipPlane));
    }

    public void SetSpawnSettings (CatchToxinsManager.LevelConfig config) {
        minimumSpawnDelay = config.minimumSpawnDelay;
        maximumSpawnDelay = config.maximumSpawnDelay;

        if (isAnEnemySpawner) {
            spawnArray = config.enemyArray;
        }
        else {
            spawnArray = config.friendlyArray;
        }

    }

    void BeginSpawning () {
        StartCoroutine (Spawn ());
    }

    IEnumerator Spawn () {
        startSpawning = true;
        while (startSpawning) {
            yield return new WaitForSeconds (Random.Range (minimumSpawnDelay, maximumSpawnDelay));
            if (startSpawning) {
                GameObject obj = spawnArray.GetRandomItem();
                Instantiate (obj, new Vector2 (Random.Range (leftBoundary.x, rightBoundary.x), transform.position.y), Quaternion.identity);
            }
        }
    }

    void StopSpawning () {
        startSpawning = false;
    }
}
