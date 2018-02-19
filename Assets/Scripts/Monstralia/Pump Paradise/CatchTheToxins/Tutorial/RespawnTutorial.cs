using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTutorial : MonoBehaviour {

    public List<Transform> EnemyList;

	float timer;
	float timer2;

	void Start()
	{
		timer = 0f;
		timer2 = 0f;
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		if (timer2 >= 1350) {
			timer += 1;
			Transform enemy = EnemyList [UnityEngine.Random.Range (0, EnemyList.Count)];
			if (timer == 150) {
				Instantiate (enemy, new Vector2 (UnityEngine.Random.Range (-5f, 5f), 15f), Quaternion.identity);
				timer = 0;
			}
		}
		timer2 += 1f;
    }
}
