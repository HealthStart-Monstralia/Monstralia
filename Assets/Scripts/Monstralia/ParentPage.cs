using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentPage : MonoBehaviour {
	public Button[] buttonsToEnable;
	public int currentPage = 0;
	public GameObject[] pageList;
	public Canvas[] pageTabList;
	public int selectedPosition = 470;
	public int unselectedPosition = 450;

	private static ParentPage instance;

	void Awake() {
		/* Ensure singleton status */
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		print ("Parent Page Created");
		SetCurrentPage (0);
	}

	public void EnableButtons() {
		if (buttonsToEnable[0] != null) {
			for (int i = 0; i < buttonsToEnable.Length; i++) {
				buttonsToEnable [i].interactable = true;
			}
		}
	}

	public void DeleteParentPage() {
		gameObject.SetActive (false);
		instance = null;
		Destroy(gameObject);
		print ("Parent Page Deleted");
	}

	public static ParentPage GetInstance() {
		return instance;
	}

	public void SetCurrentPage(int page) {
		currentPage = page;
		switch (currentPage) {
		case 0:
			pageList [0].SetActive (true);
			pageTabList [0].sortingOrder = 5;
			pageTabList [0].transform.localPosition = new Vector2 (
				pageTabList[0].transform.localPosition.x, selectedPosition);

			pageList [1].SetActive (false);
			pageTabList [1].sortingOrder = 3;
			pageTabList [1].transform.localPosition = new Vector2 (
				pageTabList[1].transform.localPosition.x, unselectedPosition);

			pageList [2].SetActive (false);
			pageTabList [2].sortingOrder = 1;
			pageTabList [2].transform.localPosition = new Vector2 (
				pageTabList[2].transform.localPosition.x, unselectedPosition);
			break;
		case 1:
			pageList [0].SetActive (false);
			pageTabList [0].sortingOrder = 3;
			pageTabList [0].transform.localPosition = new Vector2 (
				pageTabList[0].transform.localPosition.x, unselectedPosition);

			pageList [1].SetActive (true);
			pageTabList [1].sortingOrder = 5;
			pageTabList [1].transform.localPosition = new Vector2 (
				pageTabList[1].transform.localPosition.x, selectedPosition);

			pageList [2].SetActive (false);
			pageTabList [2].sortingOrder = 3;
			pageTabList [2].transform.localPosition = new Vector2 (
				pageTabList[2].transform.localPosition.x, unselectedPosition);
			break;
		case 2:
			pageList [0].SetActive (false);
			pageTabList [0].sortingOrder = 1;
			pageTabList [0].transform.localPosition = new Vector2 (
				pageTabList[0].transform.localPosition.x, unselectedPosition);

			pageList [1].SetActive (false);
			pageTabList [1].sortingOrder = 3;
			pageTabList [1].transform.localPosition = new Vector2 (
				pageTabList[1].transform.localPosition.x, unselectedPosition);

			pageList [2].SetActive (true);
			pageTabList [2].sortingOrder = 5;
			pageTabList [2].transform.localPosition = new Vector2 (
				pageTabList[2].transform.localPosition.x, selectedPosition);
			break;
		}
	}
}
