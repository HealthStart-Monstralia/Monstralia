using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour {

	public Transform circle;
	public float stopTimer;
	public float endTimer;

	private float timer = 0f;
    private Transform prefabCopy;

    // Use this for initialization
    void Start ()
	{

	}

	// Update is called once per frame
	void FixedUpdate () {

		
		if (timer == stopTimer)
		{
			//circle.gameObject.SetActive (true);
			prefabCopy = Instantiate(circle, new Vector2(0, 0), Quaternion.identity);

		}

		else if (timer == endTimer)
		{
            //circle.gameObject.SetActive(false);
            Destroy(prefabCopy.gameObject);
            
		}
        timer += 1;

    }
}
