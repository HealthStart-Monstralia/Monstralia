using UnityEngine;
using System.Collections;

public class SnapPointGenerator : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 * USAGE: Place the SnapPointGenerator GameObject in the very top left corner of grid
	 */

	// Prefab to be instantiated
	public GameObject snapPoint;

	// Amount of grid squares
	[Range(1,10)]
	public int gridCountX;
	[Range(1,10)]
	public int gridCountY;

	[Range(1,10)]
	public int startingGridX;
	[Range(1,10)]
	public int startingGridY;
	public GameObject[] snapPointList;

	// Size of individual grid square in Unity editor
	public float gridSize;

	private int totalSnaps, countingSnaps;
	private float offset;

	void Awake () {
		totalSnaps = gridCountX * gridCountY;
		snapPointList = new GameObject[totalSnaps];

		// Placing SnapPointGenerator in the center of grid
		offset = gridSize / 2;
		transform.position += new Vector3 (offset, -offset, 0f);

		countingSnaps = 0;
		for (int countColumn = 0; countColumn < gridCountY; countColumn++) {
			for (int countRow = 0; countRow < gridCountX; countRow++) {
				CreateSnapPoint (countColumn, countRow, countingSnaps);
				countingSnaps++;
			}
		}
	}

	void CreateSnapPoint (int column, int row, int count) {
		snapPointList[count] =					// Adding instantiated snap point to list
			Instantiate (snapPoint, 				// Object
			transform.position + 					// Position
			new Vector3 ( gridSize * row, 			// X
				gridSize * column * -1, 			// Y
				0f), 								// Z
			Quaternion.identity, 					// Rot
			transform)								// Parent
			as GameObject;
	}

	public GameObject SelectSnapPoint (int x, int y) {
		return snapPointList [(gridCountX * (y - 1)) + (x - 1)];
	}

	public Vector3 PositionSnapPoint (int x, int y) {
		return SelectSnapPoint (x, y).transform.position;
	}
}
