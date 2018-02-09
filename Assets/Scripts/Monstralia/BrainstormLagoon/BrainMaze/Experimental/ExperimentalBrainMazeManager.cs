using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalBrainMazeManager : AbstractGameManager<ExperimentalBrainMazeManager> {
    public IntVector2 size;
    public int score;
    public int scoreGoal = 1;
    public float timeLimit;

    public MazePickup[] pickupPrefab;
    public Maze mazePrefab;
    [HideInInspector] public MazeDoor doorInstance;
    public float generationStepDelay;

    private bool inputAllowed = false;
    private int numOfPickupsToSpawn;
    private int numOfPickups = 0;
    private List<MazePickup> pickupList;
    private float pickupProb = 0.1f;
    private Maze mazeInstance;
    private BMazeMonsterMovement monsterMovement;

    new void Awake () {
        base.Awake ();
        pickupList = new List<MazePickup> (pickupPrefab);

        int numOfSpawnableTiles = size.x * size.y - 2;
        if (numOfSpawnableTiles < 7) {
            size.x = 3;
            size.x = 3;
        }

        if (numOfPickupsToSpawn > numOfSpawnableTiles) {
            numOfPickupsToSpawn = numOfSpawnableTiles - 1;
        }
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            RestartGame ();
        }
    }

    public override void PregameSetup () {
        UpdateScoreGauge ();
        TimerClock.Instance.SetTimeLimit (timeLimit);
        StartCoroutine (GenerateMaze ());
    }

    IEnumerator GenerateMaze () {
        yield return null;
        score = 0;
        UpdateScoreGauge ();
        numOfPickups = 0;
        numOfPickupsToSpawn = scoreGoal;
        mazeInstance = Instantiate (mazePrefab, transform.position, Quaternion.identity) as Maze;
        StartCoroutine (mazeInstance.Generate (StartGame, generationStepDelay));
    }

    public void RestartGame () {
        StopAllCoroutines ();
        inputAllowed = false;
        Destroy (mazeInstance.gameObject);
        mazeInstance = null;
        StartCoroutine (GenerateMaze ());
    }

    void StartCountingDown () {
        StartCountdown (StartGame);
    }

    void StartGame () {
        inputAllowed = true;
    }

    public void UpdateScoreGauge () {
        if (ScoreGauge.Instance.gameObject.activeSelf)
            ScoreGauge.Instance.SetProgressTransition ((float)score / scoreGoal);
    }

    public GameObject CreatePickup (MazeCell cell) {
        if (numOfPickupsToSpawn > 0) {
            GameObject prefabSelected;

            // Create one of each pickup at least once, afterwards randomly select a pickup to spawn
            if (numOfPickups < pickupList.Count) {
                prefabSelected = pickupList[numOfPickups].gameObject;
            }
            else {
                prefabSelected = pickupList.GetRandomItem().gameObject;
            }

            numOfPickupsToSpawn--;
            numOfPickups++;
            print (numOfPickups);
            return Instantiate (prefabSelected, cell.transform.position, Quaternion.identity, cell.transform);
        } else {
            return null;
        }
    }

    public void OnPickup (MazePickup pickup) {
        numOfPickups--;
        score++;
        UpdateScoreGauge ();
        if (score >= scoreGoal) {
            Destroy (doorInstance.gameObject);
        }
    }

    public bool GetInputAllowed () {
        return inputAllowed;
    }
}
