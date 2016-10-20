using UnityEngine;
using System.Collections;

public class BMaze_MonsterMovement : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public BMaze_SnapPointGenerator SnapGen;
	public GameObject ArrowGUI;
	public GameObject[] ArrowGUIIcons = new GameObject[4];
	public LayerMask layerMaze;
	public enum Movement {Up = 0, Down = 1, Right = 2, Left = 3};
	public float colliderRaycastDist;
	public bool allowMovement;

	private AudioSource audioSrc;

	[SerializeField] private int locationX, locationY;

	void Start () {
		allowMovement = true;

		if (!SnapGen) {
			SnapGen = GameObject.Find ("SnapPointCreator").GetComponent<BMaze_SnapPointGenerator>();
		}

		audioSrc = GetComponent<AudioSource> ();

		locationX = SnapGen.startingGridX;
		locationY = SnapGen.startingGridY;
		MoveToSnapPoint ();
		CheckAllCollisions ();
	}

	public void CheckAllCollisions () {
		if (allowMovement) {
			RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, colliderRaycastDist, layerMaze);
			if (hitUp.collider)
				ArrowGUIIcons [(int)Movement.Up].SetActive (false);
			else
				ArrowGUIIcons [(int)Movement.Up].SetActive (true);
		
			RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, colliderRaycastDist, layerMaze);
			if (hitDown.collider)
				ArrowGUIIcons [(int)Movement.Down].SetActive (false);
			else
				ArrowGUIIcons [(int)Movement.Down].SetActive (true);

			RaycastHit2D hitRight = Physics2D.Raycast (transform.position, Vector2.right, colliderRaycastDist, layerMaze);
			if (hitRight.collider)
				ArrowGUIIcons [(int)Movement.Right].SetActive (false);
			else
				ArrowGUIIcons [(int)Movement.Right].SetActive (true);
		
			RaycastHit2D hitLeft = Physics2D.Raycast (transform.position, Vector2.left, colliderRaycastDist, layerMaze);
			if (hitLeft.collider)
				ArrowGUIIcons [(int)Movement.Left].SetActive (false);
			else
				ArrowGUIIcons [(int)Movement.Left].SetActive (true);
		} else {
			ArrowGUIIcons [(int)Movement.Up].SetActive (false);
			ArrowGUIIcons [(int)Movement.Down].SetActive (false);
			ArrowGUIIcons [(int)Movement.Left].SetActive (false);
			ArrowGUIIcons [(int)Movement.Right].SetActive (false);
		}
	}

	public bool CheckForCollision (Movement direction) {
		switch (direction) {
		case Movement.Up:
			RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, colliderRaycastDist, layerMaze);
			if (!hitUp.collider)
				return true;
			else
				return false;
			break;
		case Movement.Down:
			RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, colliderRaycastDist, layerMaze);
			if (!hitDown.collider)
				return true;
			else
				return false;
			break;
		case Movement.Right:
			RaycastHit2D hitRight = Physics2D.Raycast (transform.position, Vector2.right, colliderRaycastDist, layerMaze);
			if (!hitRight.collider)
				return true;
			else
				return false;
			break;
		case Movement.Left:
			RaycastHit2D hitLeft = Physics2D.Raycast (transform.position, Vector2.left, colliderRaycastDist, layerMaze);
			if (!hitLeft.collider)
				return true;
			else
				return false;
			break;
		default:
			return false;
			break;
		}
	}

	void Update () {
		ArrowGUI.transform.position = transform.position;
		CheckAllCollisions ();

		if (Input.GetKeyDown ("up")) {
			MoveUp ();
		}

		if (Input.GetKeyDown ("down")) {
			MoveDown ();
		}

		if (Input.GetKeyDown ("left")) {
			MoveLeft ();
		}

		if (Input.GetKeyDown ("right")) {
			MoveRight ();
		}

	}

	public void MoveUp() {
		bool canMove = CheckForCollision (Movement.Up);
		if (canMove && locationY > 1) {
			locationY -= 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveDown() {
		bool canMove = CheckForCollision (Movement.Down);
		if (canMove && locationY < SnapGen.gridCountY) {
			locationY += 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveLeft() {
		bool canMove = CheckForCollision (Movement.Left);
		if (canMove && locationX > 1) {
			locationX -= 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveRight() {
		bool canMove = CheckForCollision (Movement.Right);
		if (canMove && locationX < SnapGen.gridCountX) {
			locationX += 1;
			MoveToSnapPoint ();
		}
	}

	void MoveToSnapPoint() {
		Vector3 position = SnapGen.PositionSnapPoint (locationX, locationY);
		transform.position = position;
	}

	public void Pickup(string obj) {
		audioSrc.Play ();
	}
}