using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * \class DishObject
 * \brief Defines the behavior of the dishes in the MemoryMatch game.
 * 
 * This script defines what a dish does when it is clicked on as well as
 * determines if a match has been made.
 */

public class DishObject : MonoBehaviour {
	private Food myFood;
	private static bool isGuessing = false;
	private bool matched = false;
	private Animator dishAnim;
	private SpriteRenderer lidSpriteComponent, dishSpriteComponent, foodSpriteComponent;
	private int initialSortingLayer;

	[HideInInspector] public GameObject dish, lid, foodObject;
	public AudioClip lidSfx;

	void Awake() {
		dish = gameObject;
		lid = gameObject.transform.Find ("DishLid").gameObject;
		dishAnim = GetComponent<Animator> ();
		dishAnim.Play ("Dish_NoLid");
		lidSpriteComponent = lid.GetComponent<SpriteRenderer> ();
		dishSpriteComponent = dish.GetComponent<SpriteRenderer> ();
		initialSortingLayer = dishSpriteComponent.sortingOrder;
	}

	void LateUpdate() {
        int layerNum = Mathf.Clamp((initialSortingLayer + 13) + ((-(int)transform.position.y) * 10), 1, 100);

        lidSpriteComponent.sortingOrder = layerNum + 2;
        if (foodObject) foodSpriteComponent.sortingOrder = layerNum + 1;
        dishSpriteComponent.sortingOrder = layerNum;

	}

	public void SetFood(GameObject food) {
		print ("SetFood");
		foodObject = food;
		myFood = foodObject.GetComponent<Food>();
		foodSpriteComponent = foodObject.GetComponent<SpriteRenderer> ();
	}

	public void Reset() {
		Destroy(myFood.gameObject);
		Cover ();
	}

	public void SpawnLids(bool playAnim) {
		if (playAnim) {
			dishAnim.Play ("Dish_Start");
		} else {
			dishAnim.Play ("Dish_hasLid");
		}
	}

	public void Cover() {
		if(!lid.activeSelf) {
			lid.SetActive(true);
		}
		dishAnim.Play ("Dish_Start");
	}

	public void Shake(bool hideLid) {
		dishAnim.Play ("Dish_Shake");
		if(hideLid) {
			lid.SetActive(false);
		}
	}

	public void OpenLid() {
		if(!lid.activeSelf) {
			lid.SetActive(true);
		}
		dishAnim.Play ("Dish_OpenLid");
	}

	public void CloseLid() {
		if(!lid.activeSelf) {
			lid.SetActive(true);
		}
		dishAnim.Play ("Dish_CloseLid");
	}

	public void Correct() {
		SoundManager.GetInstance ().PlayCorrectSFX ();
		if(!lid.activeSelf) {
			lid.SetActive(true);
		}
		dishAnim.Play ("Dish_Correct");
		MemoryMatchGameManager.GetInstance().AddToMatchedList(myFood);
		matched = true;
	}

    /**
     * \brief OnMouseDown is called when the player clicks (or taps) one of the dishes.
     * 
     * Check if this dish's myFood matches the foodToMatch. If it does match, deactivate
     * the top part of the dish permenanatly, otherwise, cover the food again.
     * @return WaitForSeconds for a delay.
     */

    IEnumerator OnMouseDown () {
		MemoryMatchGameManager manager = MemoryMatchGameManager.GetInstance ();
		if(manager.inputAllowed && !isGuessing && (manager.isGameStarted() || manager.isRunningTutorial())) {
			isGuessing = true;

			OpenLid();
			manager.subtitlePanel.Display(myFood.name, myFood.clipOfName);

			if(manager.GetFoodToMatch().name != myFood.name) {
				if (!matched && !manager.isRunningTutorial()) {
					manager.SubtractTime (3.0f);
				}
				yield return new WaitForSeconds (2f);
				CloseLid ();
			}
			else {
				Correct();
				yield return new WaitForSeconds(1.5f);
				manager.ChooseFoodToMatch();
			}

			//The player can now guess again.
			manager.subtitlePanel.Hide ();
			isGuessing = false;
		}
	}

	public bool IsMatched() {
		return matched;
	}

	/* Used in Dish_Correct animation event */
	public void PlayLidWoosh() {
		SoundManager.GetInstance ().PlaySFXClip (lidSfx);
	}
}
