using UnityEngine;
using System.Collections;

public class BMaze_ArrowInput : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	//public enum Movement {Up, Down, Left, Right};
	public BMaze_MonsterMovement.Movement MoveDirection;
	public BMaze_MonsterMovement monster;

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
