using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    public float speed0;

    private int current;
    
    void Update()
    {
        //transform.position += Vector3.down * Time.deltaTime * speed0;

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            Destroy(this.gameObject);
        }
    }
}

