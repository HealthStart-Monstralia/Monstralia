using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour {
    public string goodFoodsPath = "Food/Good Foods";
    public string badFoodsPath = "Food/Bad Foods";
    private List<GameObject> goodFoods = new List<GameObject>();
    private List<GameObject> badFoods = new List<GameObject> ();
    private static Dictionary<string, int> foodEatenDictionary = new Dictionary<string, int>();

    private void Start () {
        goodFoods.AddRange (Resources.LoadAll<GameObject> ("Prefabs/" + goodFoodsPath));
        badFoods.AddRange (Resources.LoadAll<GameObject> ("Prefabs/" + badFoodsPath));
        foreach (GameObject food in goodFoods) {
            foodEatenDictionary.Add (food.name, 0);
        }
    }

    public GameObject GetRandomGoodFood() {
        return goodFoods.GetRandomItem();
    }

    public List<GameObject> GetGoodFoodsList () {
        List<GameObject> newList = new List<GameObject>();
        newList.AddRange (goodFoods);
        return newList;
    }

    public int GetNumberOfFoodEaten (string foodName) {
        return foodEatenDictionary[foodName];
    }

    public int GetNumberOfFoodEaten (GameObject foodObject) {
        return GetNumberOfFoodEaten (foodObject.name);
    }

    public static void SetFoodDictionary (Dictionary<string, int> dictionary) {
        foodEatenDictionary = dictionary;
    }

    public static Dictionary<string, int> GetFoodDictionary () {
        return foodEatenDictionary;
    }
}
