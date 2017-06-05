using UnityEngine;
using System.Collections;

/**
 * \class DishBehavior
 * \brief Defines the behavior of the dishes in the MemoryMatch game.
 * 
 * This script defines what a dish does when it is clicked on as well as
 * determines if a match has been made.
 */

public class DishBehavior : MonoBehaviour {
	
	private Food myFood; /*!< The food belonging to this dish. */
	private static bool isGuessing; /*!< Flag that keeps track of whether the player made a guess. */
	private bool matched = false;			/*!< Flag that keeps track of whether this dish has been matched. */

	public GameObject top; /*!< Reference to the top part of the dish. */
	public GameObject bottom; /*!< Reference to the bottom part of the dish. */

	/** \cond */
	void Start () {
		isGuessing = false;
	}
	/** \endcond */

	/**
	 * \brief Set the food of this dish.
	 * @param food: a GameObject with a Food component attached
	 */
	public void SetFood(Food food) {
		myFood = food;//gameObject.GetComponentsInChildren<Food>()[0];
	}

	public void Reset() {
		Destroy(myFood.gameObject);
		if(top.activeSelf == false) {
			top.SetActive(true);
			top.GetComponent<Animation>().Play (top.GetComponent<Animation>()["DishTopRevealClose"].name);
		}
	}

	/**
	 * \brief OnMouseDown is called when the player clicks (or taps) one of the dishes.
	 * 
	 * Check if this dish's myFood matches the foodToMatch. If it does match, deactivate
	 * the top part of the dish permenanatly, otherwise, cover the food again.
	 * @return WaitForSeconds for a delay.
	 */
	IEnumerator OnMouseDown() {
		Animation animation = top.GetComponent<Animation>();
		if(!isGuessing && (MemoryMatchGameManager.GetInstance().isGameStarted() || MemoryMatchGameManager.GetInstance().isRunningTutorial())) {
			isGuessing = true;

			//Reveal the food underneath the dish by setting the sprite renderer to disabled.
			top.GetComponent<Animation>().Play (animation["DishTopRevealLift"].name);
			MemoryMatchGameManager.GetInstance().subtitlePanel.Display(myFood.name, myFood.clipOfName);

			if(MemoryMatchGameManager.GetInstance().GetFoodToMatch().name != myFood.name) {
				if (!matched) {
					MemoryMatchGameManager.GetInstance ().SubtractTime (3.0f);
					yield return new WaitForSeconds (2f);
				}

				top.GetComponent<Animation>().Play (animation["DishTopRevealClose"].name);
			}
			else {
				top.GetComponent<Animation>().Play (animation["DishTopRevealLift"].name);
				SoundManager.GetInstance().PlayCorrectSFX();
				MemoryMatchGameManager.GetInstance().AddToMatchedList(myFood);
				yield return new WaitForSeconds(1.5f);
				top.GetComponent<SpriteRenderer>().enabled = false;
				matched = true;

				MemoryMatchGameManager.GetInstance().ChooseFoodToMatch();
			}
			//The player can now guess again.
			MemoryMatchGameManager.GetInstance().subtitlePanel.Hide ();
			isGuessing = false;
		}
	}

	public bool IsMatched() {
		return matched;
	}
	
}
