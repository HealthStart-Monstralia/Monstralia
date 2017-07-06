using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MMReview : MonoBehaviour {

	private GameObject foodToMatch;
	private List<GameObject> activeBadFoods;

	public List<GameObject> badFoods;
	public List<GameObject> goodFood;

	public List<DishBehavior> dishes;

	// Use this for initialization
	void Start () {
		
		foodToMatch = goodFood[Random.Range(0, goodFood.Count)];
		activeBadFoods = new List<GameObject>();
		ChooseBadFoods();
		SpawnFood();
		StartCoroutine(RevealDishes ());
	}

	void ChooseBadFoods() {
		int foodCount = 0;
		while(foodCount < dishes.Count - 1){
			int randomIndex = Random.Range(0, badFoods.Count);
			GameObject newFood = badFoods[randomIndex];
			if(!activeBadFoods.Contains(newFood)){
				activeBadFoods.Add(newFood);
				++foodCount;
			}

		}
	}

	void SpawnFood() {
		int goodFoodIndex = Random.Range(0, dishes.Count);
		GameObject newFood;
		int badFoodCount = 0;

		for(int i = 0; i < dishes.Count; ++i) {
			if(i == goodFoodIndex) {
				newFood = Instantiate(foodToMatch);
				newFood.name = foodToMatch.name;
				DishBehavior dish = dishes[goodFoodIndex].GetComponent<DishBehavior>();
				newFood.GetComponent<Food>().Spawn(dish.top.transform, dish.bottom.transform);
				dishes[goodFoodIndex].SetFood(foodToMatch.GetComponent<Food>());
			}
			else {
				newFood = Instantiate(activeBadFoods[badFoodCount]);
				newFood.name = foodToMatch.name;
				DishBehavior dish = dishes[i].GetComponent<DishBehavior>();
				newFood.GetComponent<Food>().Spawn(dish.top.transform, dish.bottom.transform);
				dishes[i].SetFood(foodToMatch.GetComponent<Food>());
			}
		}
	}

	IEnumerator RevealDishes ()
	{
		for (int i = 0; i < dishes.Count; ++i) {
			DishBehavior d = dishes [i];
			Animation animation = d.GetComponent<DishBehavior> ().top.GetComponent<Animation> ();
			d.GetComponent<DishBehavior> ().top.GetComponent<Animation> ().Play (animation ["DishTopRevealLift"].name);
		}

		yield return new WaitForSeconds(3.0f);

		for (int j = 0; j < dishes.Count; ++j) {
			DishBehavior d = dishes [j];
			Animation animation = d.GetComponent<DishBehavior> ().top.GetComponent<Animation> ();
			d.GetComponent<DishBehavior> ().top.GetComponent<Animation> ().Play (animation ["DishTopRevealClose"].name);
		}
	}
}
