using UnityEngine;
using System.Collections;

public class BMaze_ArrowInput : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	//public enum Movement {Up, Down, Left, Right};
	public BMaze_MonsterMovement.Movement MoveDirection;

	private BMaze_MonsterMovement monster;

	void Start () {
		if (BMaze_Manager.monsterObject)
			monster = BMaze_Manager.monsterObject.GetComponent<BMaze_MonsterMovement> ();
	}

	public void OnClick() {
		if (MoveDirection == BMaze_MonsterMovement.Movement.Up) {
			monster.MoveUp ();
		} else if (MoveDirection == BMaze_MonsterMovement.Movement.Down) {
			monster.MoveDown ();
		} else if (MoveDirection == BMaze_MonsterMovement.Movement.Left) {
			monster.MoveLeft ();
		} else if (MoveDirection == BMaze_MonsterMovement.Movement.Right) {
			monster.MoveRight ();
		}
	}
}
