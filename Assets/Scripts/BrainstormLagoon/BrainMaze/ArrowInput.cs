using UnityEngine;
using System.Collections;

public class ArrowInput : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public enum Movement {Up, Down, Left, Right};
	public Movement MoveDirection;

	public MonsterMovement monster;

	public void OnClick() {
		if (MoveDirection == Movement.Up) {
			monster.MoveUp ();
		} else if (MoveDirection == Movement.Down) {
			monster.MoveDown ();
		} else if (MoveDirection == Movement.Left) {
			monster.MoveLeft ();
		} else if (MoveDirection == Movement.Right) {
			monster.MoveRight ();
		}
	}
}
