using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaterBehaviorBrainbow : MonoBehaviour {

	public List<Transform> spawnLocations;
	public AudioClip waterClip;
    public GameObject plus5;
    public Component[] renderers;

    private bool isActive;
	private float nextSpawnTime;
	private float despawnTime;

	// Use this for initialization
	void Start () {
        if (BrainbowGameManager.GetInstance().GameStarted()) {
            nextSpawnTime = Time.time + Random.Range(5, 10);
        }
        renderers = GetComponentsInChildren<SpriteRenderer>();
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
        foreach(SpriteRenderer rend in renderers)
        {
            rend.enabled = true;
        }
        // gameObject.GetComponent<SpriteRenderer>().enabled = true;
        // SPAWN CHILD SPRITE RENDERERS
        gameObject.transform.localPosition = spawnLocations[Random.Range (0, spawnLocations.Count)].localPosition;
		//gameObject.transform.localEulerAngles = new Vector3(0,0, Random.Range (-25, 25));
	}

	void Despawn() {
        foreach (SpriteRenderer rend in renderers)
        {
            rend.enabled = false;
        }
        isActive = false;
        // gameObject.GetComponent<SpriteRenderer>().enabled = false;
        // DESPAWN CHILD SPRITE RENDERERS
        nextSpawnTime += Random.Range (5,10);
	}

	void OnMouseDown() {
		if(isActive) {
            //			gameObject.transform.Find("Subtitle").GetComponent<SubtitlePanel>().Display("Water", waterClip);
            BrainbowGameManager.GetInstance().timer.AddTime(5.0f);
            plus5.GetComponent<SpriteRenderer>().enabled = true;    //initiates plus5 animation
            nextSpawnTime = Time.time + Random.Range (5, 10);
			Despawn();
		}
	}
}
