using UnityEngine;
using System.Collections;

public class BMaze_DrawGizmo : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 * USAGE:
	 * Draw gizmos to show location of the snap point in Brain Maze.
	 * On selection a bigger gizmo circle will appear.
	 */

	[Range (0.05f,1f)]
	public float radius;
	[Range (0.05f,1f)]
	public float radiusSelected;

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere (transform.position, radiusSelected);
	}
}
