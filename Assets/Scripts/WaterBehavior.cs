using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaterBehavior : MonoBehaviour {

	public List<Transform> spawnLocations;
	public AudioClip waterClip;

	private bool isActive;
	private float nextSpawnTime;
	private float despawnTime;

	// Use this for initialization
	void Start () {
		nextSpawnTime = Time.time + Random.Range (5,10);
	}
	
	// Update is called once per frame
	void Update () {
		if(!isActive && BrainbowGameManager.GetInstance ().GameStarted() && Time.time > nextSpawnTime) {
			isActive = true;
			despawnTime = Time.time + Random.Range (5,10);
			Spawn();
		}
		if(isActive && Time.time > despawnTime) {
			Despawn();
		}
	}

	void Spawn() {
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		gameObject.transform.localPosition = spawnLocations[Random.Range (0, spawnLocations.Count)].localPosition;
		gameObject.transform.localEulerAngles = new Vector3(0,0, Random.Range (-45, 45));
	}

	void Despawn() {
		isActive = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		nextSpawnTime += Random.Range (5,10);
	}

	void OnMouseDown() {
		if(isActive) {
//			gameObject.transform.Find("Subtitle").GetComponent<SubtitlePanel>().Display("Water", waterClip);
			BrainbowGameManager.GetInstance().timer.AddTime(5.0f);
			nextSpawnTime = Time.time + Random.Range (5, 10);
			Despawn();
		}
	}
}
