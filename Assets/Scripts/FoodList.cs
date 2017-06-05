using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodList : MonoBehaviour {
	[SerializeField] private List<GameObject> redFoods = new List<GameObject>();
	[SerializeField] private List<GameObject> yellowFoods = new List<GameObject>();
	[SerializeField] private List<GameObject> greenFoods = new List<GameObject>();
	[SerializeField] private List<GameObject> purpleFoods = new List<GameObject>();
	public List<GameObject> goodFoods;

	void Start () {
		SortFoods ();
	}

	/* Sort foods into categories for Brainbow */
	void SortFoods() {
		foreach (GameObject food in goodFoods) {
			switch (food.GetComponent<Food> ().color) {
			case Colorable.Color.Red:
				redFoods.Add (food);
				break;

				case Colorable.Color.Yellow:
				yellowFoods.Add(food);
				break;

				case Colorable.Color.Green:
				greenFoods.Add(food);
				break;

				case Colorable.Color.Purple:
				purpleFoods.Add(food);
				break;
			}
		}
	}

	public List<GameObject> GetBrainbowFoods(Colorable.Color colour) {
		switch (colour) {
			case Colorable.Color.Red:
				return redFoods;
			case Colorable.Color.Yellow:
				return yellowFoods;
			case Colorable.Color.Green:
				return greenFoods;
			case Colorable.Color.Purple:
				return purpleFoods;
			default:
				return null;
		}
	}

}
