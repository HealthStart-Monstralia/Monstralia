using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : Singleton<MazeManager> {
    public Maze mazePrefab;
    private Maze mazeInstance;

    private void Start () {
        StartCoroutine (BeginGame ());
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            RestartGame ();
        }
    }

    IEnumerator BeginGame () {
        yield return null;
        mazeInstance = Instantiate (mazePrefab, transform.position, Quaternion.identity) as Maze;
        StartCoroutine (mazeInstance.Generate ());
    }

    public void RestartGame () {
        StopAllCoroutines ();
        Destroy (mazeInstance.gameObject);
        mazeInstance = null;
        StartCoroutine (BeginGame ());
    }
}
