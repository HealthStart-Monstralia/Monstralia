using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour {
    private Vector3 offset;
	private bool moving;

	void OnMouseDown() {
		moving = true;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
	}

	void FixedUpdate() {
		if(moving) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			gameObject.GetComponent<Rigidbody2D>().MovePosition(curPosition);
		}
	}

	void OnMouseUp() {
		moving = false;
	}
}
