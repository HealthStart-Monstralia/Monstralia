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
    public MazePickup[] pickupPrefab;

    private static Maze instance = null;
    private MazeCell firstCell, lastCell;
    private MazePassage lastPassage;
    private MazeDoor doorInstance;
    private MazeCell[,] cells;
    private List<MazePickup> pickupList;

    private float time = 0f;
    private float pickupProb = 0.1f;
    private bool finished = false;
    private bool backtracking = false;
    private int numOfPickups = 0;
    private int numOfCells = 0;
    private int totalNumOfCells = 0;

    private void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        totalNumOfCells = size.x * size.y;
        pickupList = new List<MazePickup> (pickupPrefab);
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
            Destroy (doorInstance.gameObject);
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
            yield return delay;
            //yield return null;
            DoNextGenerationStep (activeCells);
        }

        doorInstance = CreateDoor (lastCell, lastPassage.direction);
        //Instantiate (player, firstCell.transform.position, Quaternion.identity, transform.root);
        Instantiate (finish, lastCell.transform.position, Quaternion.identity, transform.root);

        CreateMonster monsterSpawn = gameObject.AddComponent<CreateMonster> ();
        monsterSpawn.spawnPosition = firstCell.transform;
        monsterSpawn.allowMonsterTickle = false;
        monsterSpawn.idleAnimationOn = false;
        Monster monster = monsterSpawn.SpawnMonster (GameManager.GetInstance().GetMonsterObject(DataType.MonsterType.Blue));
        monster.transform.parent.gameObject.AddComponent<BMaze_Monster> ();
        monster.transform.parent.gameObject.AddComponent<BMaze_MonsterMovement> ();
        monster.transform.parent.localScale = Vector3.one*0.1f;

        transform.position = new Vector3 (0.75f, 0.5f, 0f);
        transform.localScale = transform.localScale * 1.5f;
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
            if (!backtracking && numOfCells < totalNumOfCells) {
                CreatePickup (currentCell);
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
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3 (coordinates.x - size.x * 0.5f + 0f, coordinates.y - size.y * 0.5f + 0f, 0f);

        ProbabilitySpawnPickup (newCell);
        lastCell = newCell;
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

    private void ProbabilitySpawnPickup (MazeCell cell) {
        if (numOfPickupsToSpawn > 0) {
            if (Random.value < pickupProb) {
                pickupProb = 0.0f;
                CreatePickup (cell);
            } else {
                pickupProb += 0.1f;
            }
        }
    }

    private GameObject CreatePickup(MazeCell cell) {
        if (numOfPickupsToSpawn > 0 && !cell.hasPickup) {
            int selection = Random.Range (0, pickupList.Count);
            MazePickup pickup = Instantiate (pickupList[selection], cell.transform.position, Quaternion.identity, transform.root);
            if (selection != 0)
                pickupList.RemoveAt (selection);
            numOfPickupsToSpawn--;
            numOfPickups++;
            cell.hasPickup = true;
            return pickup.gameObject;
        }
        else {
            return null;
        }
    }
}
