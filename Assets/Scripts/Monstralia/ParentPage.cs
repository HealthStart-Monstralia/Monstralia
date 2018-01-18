using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentPage : Singleton<ParentPage> {
	public Button[] buttonsToEnable;
	public int currentPage = 0;
	public GameObject[] pageList;
	public Canvas[] pageTabList;
	public int selectedPosition = 470;
	public int unselectedPosition = 450;

	new void Awake() {
        base.Awake ();
		SetCurrentPage (0);
	}

	public void EnableButtons() {
		for (int i = 0; i < buttonsToEnable.Length; i++) {
            if (buttonsToEnable[i] != null)
                buttonsToEnable [i].interactable = true;
		}
	}

	public void DeleteParentPage() {
		gameObject.SetActive (false);
		Destroy(gameObject);
		print ("Parent Page Deleted");
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
