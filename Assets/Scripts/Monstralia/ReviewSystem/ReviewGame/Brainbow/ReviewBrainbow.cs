﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReviewBrainbow : MonoBehaviour {
	public CreateMonster monster;
    public LayerMask mask;
    public Sprite[] spriteList;
	public bool isReviewRunning = false;
	public bool inputAllowed = false;
	public List<GameObject> foods;
    public BrainbowFoodPanel foodPanel;
    public float foodScale;
    public ReviewBrainbowStripe[] stripes;
	public SubtitlePanel subtitle;
	public Transform[] spawnSlots;
    public List<GameObject> restrictedFoods;

    private Monster monsterObject;
    private int numOfFilledSlots = 0;
	private GameObject currentFoodToMatch;
	private static ReviewBrainbow instance;
    private List<GameObject> redFoodsList = new List<GameObject> ();
    private List<GameObject> yellowFoodsList = new List<GameObject> ();
    private List<GameObject> greenFoodsList = new List<GameObject> ();
    private List<GameObject> purpleFoodsList = new List<GameObject> ();

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
		PrepareReview ();
	}

	public static ReviewBrainbow GetInstance() {
		return instance;
	}

	public void PrepareReview() {
		ChooseFoodsFromManager ();
        monsterObject = monster.SpawnMonster (GameManager.GetInstance().GetPlayerMonsterType());
        monsterObject.GetComponent<SpriteRenderer> ().sortingOrder = 4;
        monsterObject.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
        StartCoroutine (BeginReview ());
	}

	IEnumerator BeginReview() {
        StartCoroutine (TurnOnRainbows ());
        yield return new WaitForSecondsRealtime (1f);
        foodPanel.TurnOnNumOfSlots (4);
        yield return new WaitForSecondsRealtime (1f);
        CreateFoods ();
        subtitle.Display ("Drag the foods to the correct color!", null, false, 7f);
		isReviewRunning = true;
		inputAllowed = true;
	}

	public void EndReview() {
		isReviewRunning = false;
        StartCoroutine (TurnOffRainbows ());
		inputAllowed = false;
		subtitle.Display ("Great job!");
        ReviewManager.GetInstance ().EndReview ();
    }

    void ChooseFoodsFromManager () {
        // Retrieve food list from GameManager and sort them into colors
        SortBrainbowFoods ();

        // Pick five random foods from each category and store them in a food pool for SpawnFood
        AddFoodsToList (redFoodsList);
        AddFoodsToList (yellowFoodsList);
        AddFoodsToList (greenFoodsList);
        AddFoodsToList (purpleFoodsList);
    }

    void CreateFoods() {
        foreach (Transform slot in spawnSlots) {
            SpawnFood (slot);
        }
    }

    // Sort foods into categories
    void SortBrainbowFoods () {
        FoodList foodList = GameManager.GetInstance ().GetComponent<FoodList> ();
        Food foodComponent;

        // Remove any restricted foods
        List<GameObject> brainbowFoods = foodList.goodFoods;
        foreach (GameObject food in restrictedFoods) {
            brainbowFoods.Remove (food);
        }

        // Sort food into corresponding color categories
        foreach (GameObject food in brainbowFoods) {
            foodComponent = food.GetComponent<Food> ();
            if (foodComponent.foodType == Food.TypeOfFood.Fruit || foodComponent.foodType == Food.TypeOfFood.Vegetable) {
                switch (foodComponent.color) {
                    case Colorable.Color.Red:
                        redFoodsList.Add (food);
                        break;

                    case Colorable.Color.Yellow:
                        yellowFoodsList.Add (food);
                        break;

                    case Colorable.Color.Green:
                        greenFoodsList.Add (food);
                        break;

                    case Colorable.Color.Purple:
                        purpleFoodsList.Add (food);
                        break;
                }
            }
        }
    }

    void AddFoodsToList(List<GameObject> goList) {
		int randomIndex;
		randomIndex = Random.Range (0, goList.Count);
		foods.Add (goList [randomIndex]);
		goList.RemoveAt(randomIndex);
	}

	public void IncreaseNumOfFilledSlots() {
		numOfFilledSlots++;
		if (numOfFilledSlots == 4) {
			EndReview ();
		}
	}

    // Tell food panel to spawn a random food at given transform
    void SpawnFood (Transform spawnPos) {
        // Grab a random food item from food pool
        int randomIndex = Random.Range (0, foods.Count);

        // Create random food at given transform
        GameObject newFood = foodPanel.CreateItemAtSlot (foods[randomIndex], spawnPos);

        // Name the created food item and give it a BrainbowFoodItem component
        newFood.name = foods[randomIndex].name;
        ReviewBrainbowFood brainbowComponent = newFood.AddComponent<ReviewBrainbowFood> ();
        newFood.transform.localScale = new Vector3 (foodScale, foodScale, 1f);
        newFood.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
        newFood.GetComponent<SpriteRenderer> ().sortingOrder = 7;
        print (newFood);
        // Remove created food item from food pool
        foods.RemoveAt (randomIndex);
    }

    public void ShowRainbowStripe (int selection, bool show) {
        if (show) {
            stripes[selection].gameObject.SetActive (true);
        } else {
            stripes[selection].ClearStripe ();
            stripes[selection].GetComponent<Animator> ().Play ("StripeFadeOut");
            StartCoroutine (TurnOffStripe (stripes[selection].gameObject));
        }
    }

    IEnumerator TurnOffStripe (GameObject stripe) {
        yield return new WaitForSeconds (0.5f);
        stripe.SetActive (false);
    }

    IEnumerator TurnOnRainbows () {
        yield return new WaitForSeconds (0.5f);
        ShowRainbowStripe (0, true);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (1, true);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (2, true);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (3, true);
    }

    IEnumerator TurnOffRainbows () {
        yield return new WaitForSeconds (1.5f);
        ShowRainbowStripe (3, false);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (2, false);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (1, false);
        yield return new WaitForSeconds (0.25f);
        ShowRainbowStripe (0, false);
        yield return new WaitForSeconds (0.25f);
    }
}
