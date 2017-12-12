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
	public SubtitlePanel subtitle;

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
		switch (GameManager.GetInstance().GetMonsterType ()) {
		case DataType.MonsterType.Blue:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)DataType.MonsterType.Blue];
			break;
		case DataType.MonsterType.Green:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)DataType.MonsterType.Green];
			break;
		case DataType.MonsterType.Red:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)DataType.MonsterType.Red];
			break;
		case DataType.MonsterType.Yellow:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = spriteList [(int)DataType.MonsterType.Yellow];
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
		subtitle.Display ("Remember the foods!", null, false, 7f);
		yield return new WaitForSecondsRealtime (1f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].OpenLid ();
		}

		yield return new WaitForSecondsRealtime (6f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].CloseLid ();
		}

		yield return new WaitForSecondsRealtime (1f);

		subtitle.Display ("Tap on the dish with the same food as below!", null, false, 0f);

		isReviewRunning = true;
		inputAllowed = true;
		currentFoodToMatch.SetActive (true);
	}

	public void EndReview() {
		isReviewRunning = false;
		inputAllowed = false;
		subtitle.Display ("Great job!");

        ReviewManager.GetInstance ().EndReview ();
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
			currentFoodToMatch.transform.localPosition = new Vector3 (0f, 0f, 0f);
			currentFoodToMatch.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
			currentFoodToMatch.SetActive (false);
		}
	}

	public Food GetFoodToMatch() {
		return currentFoodToMatch.GetComponent<Food>();
	}
}
