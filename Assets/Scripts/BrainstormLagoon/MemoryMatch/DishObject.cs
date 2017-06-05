using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
		lidSpriteComponent.sortingOrder = (initialSortingLayer + 15) + ((-(int)transform.position.y) * 10);
		dishSpriteComponent.sortingOrder = (initialSortingLayer + 13) + ((-(int)transform.position.y) * 10);
		if (foodObject)
			foodSpriteComponent.sortingOrder = (initialSortingLayer + 14) + ((-(int)transform.position.y) * 10);
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
			lid.SetActive (true);
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
