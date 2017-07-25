using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewMemoryMatch : MonoBehaviour {
	public GameObject monster;
	public Sprite[] spriteList;
	public bool isReviewRunning = false;
	public ReviewMemoryMatchDish[] dishes;
	public bool inputAllowed = false;
	public List<GameObject> foods;
	public Transform foodToMatchSpawnPos;
	public float foodScale, foodToMatchScale;

	private GameObject currentFoodToMatch;
	private static ReviewMemoryMatch instance;
	private List<GameObject> activeFoods = new List<GameObject>();

	void Awake() {
		if(instance == null) {
			instance = this;
		}

		else if(instance != this) {
			Destroy(gameObject);
		}

		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;

	}

	void Start() {
		// Change monster sprite depending on player choice
		switch (GameManager.GetMonsterType ()) {
		case GameManager.MonsterType.Blue:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)GameManager.MonsterType.Blue];
			break;
		case GameManager.MonsterType.Green:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)GameManager.MonsterType.Green];
			break;
		case GameManager.MonsterType.Red:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)GameManager.MonsterType.Red];
			break;
		case GameManager.MonsterType.Yellow:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)GameManager.MonsterType.Yellow];
			break;
		}
		PrepareReview ();
	}

	public static ReviewMemoryMatch GetInstance() {
		return instance;
	}

	public void PrepareReview() {
		RetrieveFoodsFromManager ();
		SelectFoods ();
		List<GameObject> copy = new List<GameObject>(activeFoods);

		for(int i = 0; i < dishes.Length; ++i) {
			GameObject newFood = SpawnFood(copy, true, dishes[i].lid.transform, dishes[i].dish.transform, foodScale);
			dishes[i].SetFood(newFood);
		}

		ChooseFoodToMatch ();

		StartCoroutine (BeginReview ());
	}

	IEnumerator BeginReview() {
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].SpawnLids (false);
		}

		yield return new WaitForSecondsRealtime (1f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].OpenLid ();
		}

		yield return new WaitForSecondsRealtime (3f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].CloseLid ();
		}

		isReviewRunning = true;
	}

	public void EndReview() {
		isReviewRunning = false;

		/* Insert function that tells the Review Manager
		 * the review is finished */
	}

	void RetrieveFoodsFromManager() {
		foods = GameManager.GetInstance ().GetComponent<FoodList> ().goodFoods;
	}

	void SelectFoods() {
		int foodCount = 0;
		while(foodCount < dishes.Length) {
			int randomIndex = Random.Range(0, foods.Count);
			GameObject newFood = foods[randomIndex];
			print ("activeFoods: " + activeFoods + " newFood: " + newFood);
			activeFoods.Add(newFood);
			foods.RemoveAt (randomIndex);
			++foodCount;
		}
	}

	GameObject SpawnFood(List<GameObject> foodsList, bool setAnchor, Transform spawnPos, Transform parent, float scale) {
		int randomIndex = Random.Range (0, foodsList.Count);
		GameObject newFood = Instantiate(foodsList[randomIndex]);
		newFood.name = foodsList[randomIndex].name;
		newFood.GetComponent<Food>().Spawn(spawnPos, parent, scale);
		newFood.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";

		if(setAnchor) {
			newFood.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
			newFood.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
		}

		foodsList.RemoveAt(randomIndex);
		newFood.GetComponent<Collider2D> ().enabled = false;

		return newFood;
	}

	public void ChooseFoodToMatch() {
		if(activeFoods.Count > 0) {
			currentFoodToMatch = SpawnFood(activeFoods, false, foodToMatchSpawnPos, foodToMatchSpawnPos, foodToMatchScale);
			currentFoodToMatch.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
			currentFoodToMatch.GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
	}

	public Food GetFoodToMatch() {
		return currentFoodToMatch.GetComponent<Food>();
	}
}
