using UnityEngine;
using System.Collections;

public class MoveableObject : MonoBehaviour {

	private Vector3 offset;
	private Vector3 screenPoint;
	private bool moving;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() {
		moving = true;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void FixedUpdate() {
		if(moving) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			gameObject.GetComponent<Rigidbody2D>().MovePosition(curPosition);
		}
	}

	void OnMouseUp() {
		moving = false;
	}
}
