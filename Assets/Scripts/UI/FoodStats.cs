using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStats : MonoBehaviour {
    public GameObject foodEntryPrefab;
    private Dictionary<string, FoodList.FoodStats> foodDictionary;

    private void Start () {
        foodDictionary = FoodList.GetFoodDictionary ();
        foreach (KeyValuePair<string, FoodList.FoodStats> foodStat in foodDictionary) {
            if (foodDictionary[foodStat.Key].foodEatenCount > 0) {
                CreateFoodEntry (foodStat);
            }
        }
    }

    void CreateFoodEntry (KeyValuePair<string, FoodList.FoodStats> foodStat) {
        FoodEntry entry = Instantiate (foodEntryPrefab, transform).GetComponent<FoodEntry> ();
        GameObject foodObject = foodDictionary[foodStat.Key].foodPrefab;
        entry.SetEntryDetails (
            foodObject.GetComponent<SpriteRenderer> ().sprite,
            foodObject.GetComponent<Food> ().foodName,
            foodDictionary[foodStat.Key].foodEatenCount
        );
    }
}
