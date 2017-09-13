using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BMaze_MonsterMovement : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public LayerMask layerMaze, layerTile;
	//public enum Movement {Up = 0, Down = 1, Right = 2, Left = 3};
	public bool allowMovement;

	private AudioSource audioSrc;
	private Vector3 pointerOffset;
	private Vector3 cursorPos;
	private Rigidbody2D rigBody;
	public Vector2 gotoPos;
	public bool finished = false;

	void Start () {
		rigBody = GetComponent<Rigidbody2D> ();
		allowMovement = true;
		audioSrc = GetComponent<AudioSource> ();
	}

	void FixedUpdate () {
		if (finished)
			FinishMove (gotoPos);
	}
		
	public void OnMouseDown() {
		if (allowMovement) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
		}
	}

	public void OnMouseDrag() {
		if (allowMovement) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
		}
	}

	public void Pickup(BMaze_Pickup.TypeOfPickup obj) {
		audioSrc.Play ();
	}

	public void MoveTowards (Vector2 pos) {
		rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.8f));
	}

	public void FinishMove (Vector2 pos) {
		transform.position = Vector2.MoveTowards (transform.position, pos, 0.3f);
		if (transform.position.x == pos.x && transform.position.y == pos.y)
			finished = false;
	}
}