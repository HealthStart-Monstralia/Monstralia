using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour {
    public List<GameObject> goodFoods;

    public GameObject GetRandomGoodFood() {
        return goodFoods[Random.Range (0, goodFoods.Count)];
    }

}
