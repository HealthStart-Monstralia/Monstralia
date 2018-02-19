using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementTutorial : MonoBehaviour
{
    public float speed0;
	public GameObject badCircle;
	public float stopHeight;
    private float timer = 0f;
    private Rigidbody2D rb2d2;

    void Start()
    {
        //initial screen
        rb2d2 = FindObjectOfType<Rigidbody2D>();
    }

    void FixedUpdate()
    {
		if (transform.position == new Vector3(0f, stopHeight) && timer <=300)
        {
            rb2d2.velocity = Vector2.zero;
            
        }


        else
        {
            transform.position += Vector3.down * Time.deltaTime * speed0;
        }

		if (timer >= 300)
		{

			badCircle.SetActive (false);
		}

        timer += 1;
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

