using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementTutorial2 : MonoBehaviour
{
    
    private Rigidbody2D rb2d2;

    void Start()
    {
        //initial screen
        rb2d2 = FindObjectOfType<Rigidbody2D>();
    }

    void FixedUpdate()
    {
		
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            //Destroy(this.gameObject);
			this.gameObject.SetActive(false);
            
        }
		if (other.gameObject.CompareTag("Player"))
		{
			//Destroy(this.gameObject);
			this.gameObject.SetActive(false);

		}
    }
}

