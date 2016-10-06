using UnityEngine;
using System.Collections;

public class MonsterMovement : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
	public SnapPointGenerator SnapGen;
	public GameObject ArrowGUI;

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
	}

	void Update () {
		ArrowGUI.transform.position = transform.position;

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
		if (locationY > 1) {
			locationY -= 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveDown() {
		if (locationY < SnapGen.gridCountY) {
			locationY += 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveLeft() {
		if (locationX > 1) {
			locationX -= 1;
			MoveToSnapPoint ();
		}
	}

	public void MoveRight() {
		if (locationX < SnapGen.gridCountX) {
			locationX += 1;
			MoveToSnapPoint ();
		}
	}

	void MoveToSnapPoint() {
		transform.position = SnapGen.PositionSnapPoint (locationX, locationY);
	}

	public void Pickup(string obj) {
		audioSrc.Play ();
	}
}
