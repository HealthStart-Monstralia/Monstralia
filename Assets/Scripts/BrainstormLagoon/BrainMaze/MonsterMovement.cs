using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public SnapPointGenerator SnapGen;
	public GameObject ArrowGUI;
	public GameObject[] ArrowGUIIcons = new GameObject[4];
	public LayerMask layerMaze;
	public enum Movement {Up = 0, Down = 1, Right = 2, Left = 3};

	private AudioSource audioSrc;

	[SerializeField] private int locationX, locationY;

	void Start () {
		if (!SnapGen) {
			SnapGen = GameObject.Find ("SnapPointCreator").GetComponent<SnapPointGenerator>();
		}

		audioSrc = GetComponent<AudioSource> ();

		locationX = SnapGen.startingGridX;
		locationY = SnapGen.startingGridY;
		MoveToSnapPoint ();
		CheckAllCollisions ();
	}

	public void CheckAllCollisions () {
		RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, 0.5f, layerMaze);
		if (hitUp.collider)
			ArrowGUIIcons [(int)Movement.Up].SetActive (false);
		else
			ArrowGUIIcons [(int)Movement.Up].SetActive (true);
		
		RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, 0.5f, layerMaze);
		if (hitDown.collider)
			ArrowGUIIcons [(int)Movement.Down].SetActive (false);
		else
			ArrowGUIIcons [(int)Movement.Down].SetActive (true);

		RaycastHit2D hitRight = Physics2D.Raycast (transform.position, Vector2.right, 0.5f, layerMaze);
		if (hitRight.collider)
			ArrowGUIIcons [(int)Movement.Right].SetActive (false);
		else
			ArrowGUIIcons [(int)Movement.Right].SetActive (true);
		
		RaycastHit2D hitLeft = Physics2D.Raycast (transform.position, Vector2.left, 0.5f, layerMaze);
		if (hitLeft.collider)
			ArrowGUIIcons [(int)Movement.Left].SetActive (false);
		else
			ArrowGUIIcons [(int)Movement.Left].SetActive (true);
	}

	public bool CheckForCollision (Movement direction) {
		if (direction == Movement.Up) {
			RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, 0.5f, layerMaze);
			if (!hitUp.collider)
				return true;
			else
				return false;
			
		} else if (direction == Movement.Down) {
			RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, 0.5f, layerMaze);
			if (!hitDown.collider)
				return true;
			else
				return false;
		} else if (direction == Movement.Right) {
			RaycastHit2D hitRight = Physics2D.Raycast (transform.position, Vector2.right, 0.5f, layerMaze);
			if (!hitRight.collider)
				return true;
			else
				return false;
		} else if (direction == Movement.Left) {
			RaycastHit2D hitLeft = Physics2D.Raycast (transform.position, Vector2.left, 0.5f, layerMaze);
			if (!hitLeft.collider)
				return true;
			else
				return false;
		} else
			return false;
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
