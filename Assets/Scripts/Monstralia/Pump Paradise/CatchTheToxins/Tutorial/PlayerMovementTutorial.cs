using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    public GameObject monsterhand;
    public float speed0;
    bool stop;
    bool stop2;
    bool stop3;
	bool stop4;
	bool stop5;
	float timer;

    // Use this for initialization
    void Start()
    {
        stop = false;
        stop2 = false;
        stop3 = false;
		stop4 = false;
		stop5 = false;
		timer = 0f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (stop == false)
        {
            transform.position += Vector3.left * speed0;
        }

		if (transform.position == new Vector3(-7.650003f, -13.5f))
        {
            stop = true;
        }

		if (stop == true && stop2 == false)
        {
            transform.position += Vector3.right * speed0;
        }
		if (transform.position == new Vector3(7.650003f, -13.5f))
        {
            stop2 = true;
        }

		if (stop == true && stop2 == true && stop3 == false)
        {
            transform.position += Vector3.left * speed0;
        }

		if (stop == true && transform.position == new Vector3(-1.192093e-07f, -13.5f) && stop2 == true)
        {
            stop3 = true;
			monsterhand.SetActive (false);
        }

		if (timer >= 585 && stop3 == true && stop4 == false) 
		{
			transform.position += Vector3.left * speed0;
		}

		if (transform.position == new Vector3(-7.650003f, -13.5f))
		{
			stop4 = true;
		}

		if (stop4 == true && stop5 == false) 
		{
			transform.position += Vector3.right * speed0;
		}

		if (transform.position == new Vector3(-1.192093e-07f, -13.5f) && stop4 == true && stop5 == false)
		{
			stop5 = true;
		}

		timer += 1f;

    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
			//Destroy(this.gameObject);
			other.gameObject.SetActive(false);
		}
	}
}

