using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour {
    public List<GameObject> goodFoods;
    public List<GameObject> badFoods;

    public GameObject GetRandomGoodFood() {
        return goodFoods.GetRandomItem();
    }

}
