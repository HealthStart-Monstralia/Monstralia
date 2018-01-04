using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created with online tutorial at http://catlikecoding.com/unity/tutorials/maze/ */
public class Maze : MonoBehaviour {

    public IntVector2 size;
    public GameObject player;
    public GameObject finish;
    public MazeCell cellPrefab;
    public int numOfPickupsToSpawn;
    public float generationStepDelay;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public MazeDoor doorPrefab;
    public MazePickup pickupPrefab;

    private static Maze instance = null;
    private MazeCell firstCell, lastCell;
    private MazePassage lastPassage;
    private MazeDoor door;
    private MazeCell[,] cells;

    private float time = 0f;
    private float pickupProb = 0.1f;
    private bool finished = false;
    private int numOfPickups = 0;

    private void Awake () {
        print ("MAZE CREATED");
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    private void OnDestroy () {
        instance = null;
    }

    public static Maze GetInstance() {
        return instance;
    }

    public void OnPickup(MazePickup pickup) {
        numOfPickups--;
        if (numOfPickups <= 0) {
            Destroy (door);
        }

    }

    public IntVector2 RandomCoordinates {
        get {
            return new IntVector2 (Random.Range (0, size.x), Random.Range (0, size.y));
        }
    }

    public bool ContainsCoordinates (IntVector2 coordinate) {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
    }

    public MazeCell GetCell (IntVector2 coordinates) {
        return cells[coordinates.x, coordinates.y];
    }

    public IEnumerator Generate () {
        WaitForSeconds delay = new WaitForSeconds (generationStepDelay);
        cells = new MazeCell[size.x, size.y];
        List<MazeCell> activeCells = new List<MazeCell> ();
        DoFirstGenerationStep (activeCells);
        while (activeCells.Count > 0) {
            //yield return delay;
            yield return null;
            DoNextGenerationStep (activeCells);
        }

        lastCell = transform.GetChild (transform.childCount - 1).GetComponent<MazeCell> ();
        CreateDoor (lastCell, lastPassage.direction);
        Instantiate (player, firstCell.transform.position, Quaternion.identity, transform.root);
        Instantiate (finish, lastCell.transform.position, Quaternion.identity, transform.root);
    }

    public void FixedUpdate () {
        if (!finished)
            time += Time.deltaTime;
    }

    private void DoFirstGenerationStep (List<MazeCell> activeCells) {
        firstCell = CreateCell (RandomCoordinates);
        activeCells.Add (firstCell);
    }

    private void DoNextGenerationStep (List<MazeCell> activeCells) {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized) {
            activeCells.RemoveAt (currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2 ();
        if (ContainsCoordinates (coordinates)) {
            MazeCell neighbor = GetCell (coordinates);
            if (neighbor == null) {
                neighbor = CreateCell (coordinates);
                CreatePassage (currentCell, neighbor, direction);
                activeCells.Add (neighbor);
            } else {
                CreateWall (currentCell, neighbor, direction);
            }
        } else {
            CreateWall (currentCell, null, direction);
        }
    }

    private MazeCell CreateCell (IntVector2 coordinates) {
        MazeCell newCell = Instantiate (cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.y] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        print (transform);
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3 (coordinates.x - size.x * 0.5f + 0f, coordinates.y - size.y * 0.5f + 0f, 0f);

        if (Random.value < pickupProb && numOfPickupsToSpawn > 0) {
            Instantiate (pickupPrefab, newCell.transform.position, Quaternion.identity, transform.root);
            numOfPickupsToSpawn--;
            numOfPickups++;
        }
        return newCell;
    }

    private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
        MazePassage passage = Instantiate (passagePrefab) as MazePassage;
        passage.Initialize (cell, otherCell, direction);
        passage = Instantiate (passagePrefab) as MazePassage;
        passage.Initialize (otherCell, cell, direction.GetOpposite ());
        lastPassage = passage;
    }

    private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
        MazeWall wall = Instantiate (wallPrefab) as MazeWall;
        wall.Initialize (cell, otherCell, direction);
        if (otherCell != null) {
            wall = Instantiate (wallPrefab) as MazeWall;
            wall.Initialize (otherCell, cell, direction.GetOpposite ());
        }
    }

    private void CreateDoor (MazeCell cell, MazeDirection direction) {
        door = Instantiate (doorPrefab) as MazeDoor;
        door.Initialize (cell, direction);
    }
}
