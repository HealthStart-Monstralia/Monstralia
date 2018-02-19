using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RBCMovement: MonoBehaviour {

	public float speed0;

	//private Rigidbody2D rb2d2; 

	void Start()
	{
	//initial screen
	//rb2d2 = FindObjectOfType<Rigidbody2D> ();
	}


	void FixedUpdate()
	{
            //transform.position += Vector3.down * Time.deltaTime * speed0;
    }


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Target")) 
		{
			Destroy (this.gameObject);
		}
	}
}