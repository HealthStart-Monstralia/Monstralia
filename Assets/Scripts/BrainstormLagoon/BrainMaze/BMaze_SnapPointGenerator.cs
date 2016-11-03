using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BMaze_SnapPointGenerator : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 * USAGE: Place the SnapPointGenerator GameObject in the very top left corner of grid
	 */

	/*! Prefab to be instantiated */
	public GameObject snapPoint;

	/*! Amount of grid squares */
	[Range(1,10)]
	public int gridCountX;
	[Range(1,10)]
	public int gridCountY;

	/*! Amount of grid squares */
	[Range(1,10)]
	public int startingGridX;
	[Range(1,10)]
	public int startingGridY;
	public GameObject[] snapPointList;

	public float gridSize; 		/*!< Detailed description after the member Size of individual grid square in Unity editor */

	private int totalSnaps; 	/*!< For creating the size of the list */
	private int countingSnaps; 	/*!< Counter for snap points */
	private float offset; 		/*!< Offsets the snap point to the center of the grid automatically because finding the corner on a square is way easier than the center */

	void Awake () {
		totalSnaps = gridCountX * gridCountY;
		snapPointList = new GameObject[totalSnaps];

		/*! Placing SnapPointGenerator in the center of grid */
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

	/*! Generate snap points into a list relative to this gameObject's position */
	void CreateSnapPoint (int column, int row, int count) {
		snapPointList[count] =						// Adding instantiated snap point to list
			Instantiate (snapPoint, 				// Object
			transform.position + 					// Position
			new Vector3 ( gridSize * row, 			// X
				gridSize * column * -1, 			// Y
				0f), 								// Z
			Quaternion.identity, 					// Rot
			transform)								// Parent
			as GameObject;
	}

	/*! Find the corresponding snap point in the snap point list and return it for the monster's movement */
	public GameObject SelectSnapPoint (int x, int y) {
		return snapPointList [(gridCountX * (y - 1)) + (x - 1)];
	}

	/*! Return the position of the snap point in the game for MoveToSnapPoint in BMaze_MonsterMovement  */
	public Vector3 PositionSnapPoint (int x, int y) {
		return SelectSnapPoint (x, y).transform.position;
	}
}
