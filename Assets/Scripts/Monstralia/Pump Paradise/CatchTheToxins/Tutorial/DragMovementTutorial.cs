using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragMovementTutorial : MonoBehaviour {
	
	float timer;

	void Start()
	{
		timer = 0f;
	}

	void FixedUpdate()
	{
		timer += 1;
	}

	void OnMouseDrag()
	{
		if (timer >= 1300) {
			Vector2 mousePosition = new Vector2 (Input.mousePosition.x, 95);
			Vector2 objectPosition = Camera.main.ScreenToWorldPoint (mousePosition);

			transform.position = objectPosition;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
//		if (other.gameObject.CompareTag ("Gameobject")) 
//		{
//
//			print ("Collide");
//		}
	}
		

}
