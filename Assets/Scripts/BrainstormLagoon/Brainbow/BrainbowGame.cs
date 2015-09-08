using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrainbowGame : MonoBehaviour {

	public static BrainbowGame instance;

	public GameObject instructionsCanvas;
	public List<GameObject> foods;
	public Transform[] spawnPoints;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}
	void Start(){
		for(int i = 0; i < 4; ++i) {
			print(i);
			SpawnFood(spawnPoints[i]);
		}
		print("size of list: " + foods.Count);
	}

	public void Replace(GameObject toReplace) {
		if(toReplace.GetComponent<Food>() != null) {
			SpawnFood(toReplace.GetComponent<Food>().getOrigin());
		}
	}

	void SpawnFood(Transform spawnPos) {
		int randomIndex = Random.Range (0, foods.Count);
		GameObject newFood = Instantiate(foods[randomIndex]);
		newFood.GetComponent<Food>().SetOrigin(spawnPos);
		newFood.transform.SetParent (GameObject.Find ("FruitSpawnPanel").transform);
		newFood.transform.localPosition = spawnPos.localPosition;
		newFood.transform.localScale = new Vector3(17, 17, 0);
		newFood.GetComponent<SpriteRenderer>().sortingOrder = 1;
		foods.RemoveAt(randomIndex);
	}
	
	// Update is called once per frame
	void Update () {
		// print ("in Update of Brainbow game");
		//if (Time.timeSinceLevelLoad < 3.0f)
		//	print ("instructionsCanvas.name: " + instructionsCanvas.name);
	}


}