using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentPage : PopupPage {
	public Button[] buttonsToEnable;
	public int currentPage = 0;
	public GameObject[] pageList;
	public Canvas[] pageTabList;
	public int selectedPosition = 470;
	public int unselectedPosition = 450;

    [SerializeField] private AudioClip introductionVO;
    [SerializeField] private AudioClip introductionVO2;

    new void Start() {
        base.Start ();
		SetCurrentPage (0);
	}

    private new void OnEnable () {
        base.OnEnable ();
        if (!GameManager.Instance.GetHasPlayerDone (DataType.GamePersistentEvents.ParentPage)) {
            GameManager.Instance.SetPlayerDone (DataType.GamePersistentEvents.ParentPage);
            IntroducePlayerToPage ();
        }
    }

    private void OnDisable () {
        SoundManager.Instance.StopVOQueue ();
        SoundManager.Instance.StopPlayingVoiceOver ();
    }

    void IntroducePlayerToPage () {
        SoundManager.Instance.AddToVOQueue (introductionVO);
        SoundManager.Instance.AddToVOQueue (introductionVO2);
    }

	public void SetCurrentPage(int page) {
		currentPage = page;
        int sortingBaseNumber = 3;

        for (int i = 0; i < pageTabList.Length; i++) {
            if (i == currentPage) {
                pageList[i].SetActive (true);
                pageTabList[i].sortingOrder = i + sortingBaseNumber;
                pageTabList[i].transform.localPosition = new Vector2 (
                    pageTabList[i].transform.localPosition.x, selectedPosition);
            }
            else if (pageTabList[i].gameObject.activeSelf) {
                pageList[i].SetActive (false);
                pageTabList[i].sortingOrder = sortingBaseNumber - i;
                pageTabList[i].transform.localPosition = new Vector2 (
                    pageTabList[i].transform.localPosition.x, unselectedPosition);
            }

        }


        /*
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
        */
    }
}
