﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParentsButton : MonoBehaviour {

	public GameObject parentPagePrefab;
	public Button[] buttonsToDisable;

	public void CreateParentPage() {
		GameObject parentPageInstance = Instantiate (parentPagePrefab, transform.parent);
		parentPageInstance.SetActive (true);
		parentPageInstance.GetComponent<ParentPage> ().buttonsToEnable = buttonsToDisable;	// Pass buttons to the parent page prefab
	}

	public void DisableButtons() {
        if (buttonsToDisable.Length > 0) {
            for (int i = 0; i < buttonsToDisable.Length; i++) {
                if (buttonsToDisable[i] != null)
                    buttonsToDisable[i].interactable = false;
            }
        }
	}
}
