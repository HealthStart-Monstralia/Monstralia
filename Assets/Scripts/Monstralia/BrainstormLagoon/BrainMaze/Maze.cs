using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created with online tutorial at http://catlikecoding.com/unity/tutorials/maze/ */
public class Maze : Singleton<Maze> {
    [Header ("Prefabs")]
    public GameObject finishPrefab;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public MazeDoor doorPrefab;

    public delegate void Callback ();

    private MazePassage lastPassage;
    private MazeCell[,] cells;
    private List<MazeCell> cellList = new List<MazeCell> ();

    private bool backtracking = false;
    private IntVector2 size;
    private int numOfCells = 0;
    private int totalNumOfCells = 0;
    private MazeCell firstCell;

    new void Awake () {
        base.Awake ();
        transform.position = Vector3.one * 0.5f;
        size = BMazeManager.Instance.GetMazeSize();
        totalNumOfCells = size.x * size.y;
        print ("totalNumOfCells: " + totalNumOfCells);
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

    public IEnumerator Generate (Callback CallbackFunction, float generationStepDelay = 0.0f) {
        WaitForSeconds delay = new WaitForSeconds (generationStepDelay);
        cells = new MazeCell[size.x, size.y];
        List<MazeCell> activeCells = new List<MazeCell> ();
        DoFirstGenerationStep (activeCells);
        while (activeCells.Count > 0) {
            yield return delay;
            DoNextGenerationStep (activeCells);
        }

        // Prepare cellList for pickups
        // Remove first cell in cellList
        cellList.RemoveAt (0);

        // Retrieve last cell in cellList
        MazeCell lastCell = cellList[cellList.Count - 1];

        // Remove last cell in cellList
        cellList.RemoveAt (cellList.Count - 1);

        // Create door on last cell
        BMazeManager.Instance.doorInstance = CreateDoor (lastCell, lastPassage.direction);

        // Create finish line on last cell
        BMazeManager.Instance.finishLine = CreateFinishline (lastCell, lastPassage.direction).GetComponent<BMazeFinishline>();

        // Take all available cells and create pickups in them
        StartCoroutine (GeneratePickups ());

        // Clean up cellList
        cellList.Clear ();

        // Tell Manager that generation is done
        CallbackFunction ();
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
            if (!backtracking && numOfCells < totalNumOfCells) {
                BMazeManager.Instance.CreatePickup (currentCell);
                cellList.Remove (currentCell);
                backtracking = true;
            }
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
                backtracking = false;
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
        numOfCells++;
        cellList.Add (newCell);
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3 (coordinates.x - size.x * 0.5f + 0f, coordinates.y - size.y * 0.5f + 0f, 0f);
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

    private MazeDoor CreateDoor (MazeCell cell, MazeDirection direction) {
        MazeDoor door = Instantiate (doorPrefab) as MazeDoor;
        door.Initialize (cell, direction);
        return door;
    }

    private BMazeFinishline CreateFinishline (MazeCell cell, MazeDirection direction) {
        BMazeFinishline finish = Instantiate (finishPrefab).GetComponent<BMazeFinishline>();
        finish.Initialize (cell, direction);
        return finish;
    }

    IEnumerator GeneratePickups () {
        while (BMazeManager.Instance.CreatePickup (cellList.RemoveRandom ()));
        yield return null;
    }

    public MazeCell GetFirstCell() {
        return firstCell;
    }

    public void ScaleMaze () {
        // Scale maze to screen aspect ratio
        // sizeFactor calculated by Aspect Ratio * Number of Horizontal tiles * 9
        float sizeFactor = Camera.main.aspect * 10f / size.x;
        transform.localScale = Vector3.one * (sizeFactor);
        transform.position = new Vector2 (transform.position.x * transform.localScale.x, 0f);
    }
}
