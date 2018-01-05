using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour {
    public Maze mazePrefab;
    private Maze mazeInstance;

    private static MazeManager instance;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    public static MazeManager GetInstance() {
        return instance;
    }

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
