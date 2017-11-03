using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmo : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * On selection a bigger gizmo circle will appear.
	 */

	public Color normalColor = Color.white;
	public Color selectedColor = Color.white;
	[Range (0.05f,1f)]
	public float radius;
	[Range (0.05f,1f)]
	public float radiusSelected;

	void OnDrawGizmos() {
		Gizmos.color = normalColor;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = selectedColor;
		Gizmos.DrawWireSphere (transform.position, radiusSelected);
	}
}
