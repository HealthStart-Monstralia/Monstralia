using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour {
    [Serializable]
    public struct FoodStats {
        public int foodEatenCount;
        public GameObject foodPrefab;
    }

    public string goodFoodsPath = "Food/Good Foods";
    public string badFoodsPath = "Food/Bad Foods";
    private static List<GameObject> goodFoods = new List<GameObject>();
    private static List<GameObject> badFoods = new List<GameObject> ();
    private static Dictionary<string, FoodStats> foodStatsDictionary = new Dictionary<string, FoodStats> ();
    private static Dictionary<string, int> foodEatenCountDictionary = new Dictionary<string, int> ();

    private void Start () {
        goodFoods.AddRange (Resources.LoadAll<GameObject> ("Prefabs/" + goodFoodsPath));
        badFoods.AddRange (Resources.LoadAll<GameObject> ("Prefabs/" + badFoodsPath));

        // Initialize FoodStats for each food object
        foreach (GameObject food in goodFoods) {
            FoodStats newStats;
            newStats.foodEatenCount = 0;
            newStats.foodPrefab = food;
            foodStatsDictionary.Add (food.GetComponent<Food>().foodName, newStats);
        }
    }

    public static GameObject GetRandomGoodFood () {
        return goodFoods.GetRandomItem();
    }

    public static List<GameObject> GetGoodFoodsList () {
        List<GameObject> newList = new List<GameObject>();
        newList.AddRange (goodFoods);
        return newList;
    }

    public static int GetNumberOfFoodEaten (GameObject foodObject) {
        foreach (KeyValuePair<string, FoodStats> foodStat in foodStatsDictionary) {
            if (foodStat.Key == foodObject.GetComponent<Food>().foodName) {
                return foodStatsDictionary[foodStat.Key].foodEatenCount;
            }
        }

        return 0;
    }

    public static int GetNumberOfFoodEaten (string foodName) {
        return foodStatsDictionary[foodName].foodEatenCount;
    }

    public static void LoadFoodDictionary (Dictionary<string, int> dictionary) {
        foreach (KeyValuePair<string, int> foodCount in dictionary) {
            FoodStats stats;
            if (foodStatsDictionary.ContainsKey (foodCount.Key)) {
                stats = foodStatsDictionary[foodCount.Key];
                stats.foodEatenCount = dictionary[foodCount.Key];
                foodStatsDictionary[foodCount.Key] = stats;
            }
        }
    }

    public static GameObject GetFoodPrefab (string foodName) {
        return foodStatsDictionary[foodName].foodPrefab;
    }

    public static void IncreaseFoodCount (string foodName) {
        FoodStats stats;

        if (foodStatsDictionary.ContainsKey(foodName)) {
            stats = foodStatsDictionary[foodName];
            stats.foodEatenCount++;
            foodStatsDictionary[foodName] = stats;
        }
        else {
            print ("Key does not exist for " + foodName);
        }

    }

    public static Dictionary<string, FoodStats> GetFoodDictionary () {
        return foodStatsDictionary;
    }

    public static Dictionary<string, int> SaveDictionary () {
        foreach (KeyValuePair<string, FoodStats> foodStat in foodStatsDictionary) {
            if (foodEatenCountDictionary.ContainsKey(foodStat.Key)) {
                foodEatenCountDictionary[foodStat.Key] = foodStat.Value.foodEatenCount;
            }
            else {
                foodEatenCountDictionary.Add (foodStat.Key, foodStat.Value.foodEatenCount);
            }
        }

        return foodEatenCountDictionary;
    }
}
