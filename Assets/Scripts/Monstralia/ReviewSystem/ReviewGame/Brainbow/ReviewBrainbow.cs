using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReviewBrainbow : MonoBehaviour {
	public GameObject monster;
    public LayerMask mask;
    public Sprite[] spriteList;
	public bool isReviewRunning = false;
	public bool inputAllowed = false;
	public List<GameObject> foods;
	public float foodScale;
	public SubtitlePanel subtitle;
	public ReviewBrainbowSlot[] slots;
	public GameObject[] spawnSlots;

	private int numOfFilledSlots = 0;
	private GameObject currentFoodToMatch;
	private static ReviewBrainbow instance;

	void Awake() {
		if(instance == null) {
			instance = this;
		}

		else if(instance != this) {
			Destroy(gameObject);
		}

		GetComponentInChildren<Canvas> ().worldCamera = Camera.main;
		Camera.main.gameObject.AddComponent<Physics2DRaycaster> ();
	}

	void Start() {
		// Change monster sprite depending on player choice
		switch (GameManager.GetInstance().GetMonster()) {
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

	public static ReviewBrainbow GetInstance() {
		return instance;
	}

	public void PrepareReview() {
		ChooseFoodsFromManager ();
		StartCoroutine (BeginReview ());
	}

	IEnumerator BeginReview() {
		yield return new WaitForSecondsRealtime (1f);
        ChooseFoodToSpawn ();
        subtitle.Display ("Drag the foods to the correct color!", null, false, 7f);
		isReviewRunning = true;
		inputAllowed = true;
	}

	public void EndReview() {
		isReviewRunning = false;
		inputAllowed = false;
		subtitle.Display ("Great job!");
        Invoke ("FadeOut", 2.5f);
        ReviewManager.GetInstance ().EndReview ();
    }

	void ChooseFoodsFromManager() {
		FoodList listOfFoods = GameManager.GetInstance ().GetComponent<FoodList> ();
		List<GameObject> redFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Red));
		List<GameObject> yellowFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Yellow));
		List<GameObject> greenFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Green));
		List<GameObject> purpleFoodsList = new List<GameObject>(listOfFoods.GetBrainbowFoods (Colorable.Color.Purple));

		AddFoodsToList (redFoodsList);
		AddFoodsToList (yellowFoodsList);
		AddFoodsToList (greenFoodsList);
		AddFoodsToList (purpleFoodsList);
	}

	void AddFoodsToList(List<GameObject> goList) {
		int randomIndex;
		randomIndex = Random.Range (0, goList.Count);
		foods.Add (goList [randomIndex]);
		goList.RemoveAt(randomIndex);
	}

	public void IncreaseNumOfFilledSlots() {
		numOfFilledSlots++;
		if (numOfFilledSlots == slots.Length) {
			EndReview ();
		}
	}

	public void SpawnFood(GameObject foodObject, Transform trans) {
		GameObject newFood = Instantiate (foodObject, trans.position, Quaternion.identity, transform);
		newFood.AddComponent<ReviewBrainbowFood> ();
		newFood.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
		newFood.GetComponent<SpriteRenderer> ().sortingOrder = 7;
		newFood.transform.localScale = new Vector3 (foodScale, foodScale, 1f);
		newFood.transform.SetParent (trans);
	}

	void ChooseFoodToSpawn() {
		int randomIndex;
		for (int i = 0; i < spawnSlots.Length; i++) {
			randomIndex = Random.Range (0, foods.Count);
			SpawnFood (foods [randomIndex], spawnSlots[i].transform);
			foods.RemoveAt (randomIndex);
		}
	}

    public void FadeOut () {
        GetComponent<Animator> ().Play ("ReviewBrainbowGameFadeOut");
    }
}
