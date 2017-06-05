using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BMaze_MonsterMovement : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	//public BMaze_SnapPointGenerator SnapGen;
	//public GameObject ArrowGUI;
	//public GameObject[] ArrowGUIIcons = new GameObject[4];
	public LayerMask layerMaze, layerTile;
	//public enum Movement {Up = 0, Down = 1, Right = 2, Left = 3};
	public float colliderRaycastDist;
	public bool allowMovement;

	private AudioSource audioSrc;
	private Vector3 pointerOffset;
	private Vector3 cursorPos;
	private Rigidbody2D rigBody;
	private BMaze_Manager manager;
	public Vector2 gotoPos;
	public bool finished = false;

	[SerializeField] private int locationX, locationY;

	void Start () {
		rigBody = GetComponent<Rigidbody2D> ();
		allowMovement = true;
		manager = BMaze_Manager.GetInstance ();

		/*
		if (!SnapGen) {
			SnapGen = GameObject.Find ("SnapPointCreator").GetComponent<BMaze_SnapPointGenerator>();
		}
		*/

		audioSrc = GetComponent<AudioSource> ();

		//locationX = SnapGen.startingGridX;
		//locationY = SnapGen.startingGridY;
		//MoveToSnapPoint ();
		//CheckAllCollisions ();
	}

	void FixedUpdate () {
		if (finished)
			FinishMove (gotoPos);
	}
		
	public void OnMouseDown() {
		if (manager.inputAllowed) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
		}
	}

	public void OnMouseDrag() {
		if (manager.inputAllowed && allowMovement) {
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

	/*
	public void CheckAllCollisions () {
		if (allowMovement) {
			RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, colliderRaycastDist, layerMaze);
			if (hitUp.collider) {
				ArrowGUIIcons [(int)Movement.Up].SetActive (false);
			} else {
				ArrowGUIIcons [(int)Movement.Up].SetActive (true);
			}
		
			RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, colliderRaycastDist, layerMaze);
			if (hitDown.collider) {
				RaycastHit2D tileSelect = Physics2D.Raycast (transform.position, Vector2.down, colliderRaycastDist, layerTile);
				ArrowGUIIcons [(int)Movement.Down].SetActive (false);
			}
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
	*/

	/*
	public bool CheckForCollision (Movement direction) {
		bool canMove = false;

		switch (direction) {
		case Movement.Up:
			RaycastHit2D hitUp = Physics2D.Raycast (transform.position, Vector2.up, colliderRaycastDist, layerMaze);
			if (!hitUp.collider) {
				canMove = true;
			}
			break;
		case Movement.Down:
			RaycastHit2D hitDown = Physics2D.Raycast (transform.position, Vector2.down, colliderRaycastDist, layerMaze);
			if (!hitDown.collider)
				canMove = true;
			break;
		case Movement.Right:
			RaycastHit2D hitRight = Physics2D.Raycast (transform.position, Vector2.right, colliderRaycastDist, layerMaze);
			if (!hitRight.collider)
				canMove = true;
			break;
		case Movement.Left:
			RaycastHit2D hitLeft = Physics2D.Raycast (transform.position, Vector2.left, colliderRaycastDist, layerMaze);
			if (!hitLeft.collider)
				canMove = true;
			break;
		default:
			break;
		}

		return canMove;
	}
	*/

	/*
	void Update () {
		//ArrowGUI.transform.position = transform.position;
		//CheckAllCollisions ();

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

	public void SetLocationGrid (int locX, int locY) {
		locationX = locX;
		locationY = locY;
	}
	*/
}