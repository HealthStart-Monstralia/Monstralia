using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RBCMovementTutorial : MonoBehaviour
{

    public float speed0;
	public GameObject goodCircle;
    private float timer = 0f;
    private Rigidbody2D rb2d2;

    void Start()
    {
        //initial screen
        rb2d2 = FindObjectOfType<Rigidbody2D>();
    }


    void FixedUpdate()
    {


        if (transform.position == new Vector3(0f, 0.099979f))
        {
            rb2d2.velocity = Vector2.zero;
            if (timer == 300)
            {
                transform.position += Vector3.down * speed0;
				goodCircle.SetActive (false);
            }
        }

        else
        {
            transform.position += Vector3.down * speed0;
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
    }
}