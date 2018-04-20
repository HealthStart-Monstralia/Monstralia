using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodEntry : MonoBehaviour {
    public Image foodImage;
    public Text foodNameText;
    public Text foodCountText;

    public void SetEntryDetails (Sprite img, string name, int count) {
        foodImage.sprite = img;
        foodNameText.text = name;
        foodCountText.text = count.ToString();
    }
}
