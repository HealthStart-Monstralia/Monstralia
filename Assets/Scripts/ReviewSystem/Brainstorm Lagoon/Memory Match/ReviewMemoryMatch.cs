using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewMemoryMatch : Singleton<ReviewMemoryMatch> {
	public CreateMonster monsterCreator;
	public Sprite[] spriteList;
	public bool isReviewRunning = false;
	public ReviewMemoryMatchDish[] dishes;
	public bool inputAllowed = false;
	public List<GameObject> foods;
	public Transform foodToMatchSpawnPos;
	public float foodScale, foodToMatchScale;

	private GameObject currentFoodToMatch;
	private List<GameObject> activeFoods = new List<GameObject>();

	new void Awake() {
        base.Awake ();
		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
	}

	void Start() {
		PrepareReview ();
	}

	public void PrepareReview() {
		RetrieveFoodsFromManager ();
		SelectFoods ();
        Monster monsterObject = monsterCreator.SpawnPlayerMonster ();
        monsterObject.spriteRenderer.sortingLayerName = "UI";
        monsterObject.spriteRenderer.sortingOrder = 5;

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
		SubtitlePanel.Instance.Display ("Remember the foods!", null, false, 7f);
		yield return new WaitForSecondsRealtime (1f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].OpenLid ();
		}

		yield return new WaitForSecondsRealtime (6f);
		for (int i = 0; i < dishes.Length; ++i) {
			dishes[i].CloseLid ();
		}

		yield return new WaitForSecondsRealtime (1f);

		SubtitlePanel.Instance.Display ("Tap on the dish with the same food as below!", null, false, 5f);

		isReviewRunning = true;
		inputAllowed = true;
		currentFoodToMatch.SetActive (true);
	}

	public void EndReview() {
		isReviewRunning = false;
		inputAllowed = false;
		SubtitlePanel.Instance.Display ("Great job!");

        ReviewManager.Instance.EndReview ();
	}

	void RetrieveFoodsFromManager() {
		foods = GameManager.Instance.GetComponent<FoodList> ().goodFoods;
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
        newFood.transform.localPosition = new Vector3 (0f, 1.1f, 0f);
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
