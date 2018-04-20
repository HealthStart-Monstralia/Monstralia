using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButtonManager : MonoBehaviour {

    public Button[] buttonsToDisable;

    public GameObject[] itemsToDisable;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            button.interactable = false;
        }

        foreach (GameObject item in itemsToDisable)
        {
            item.SetActive(false);
        }
    }

    public void EnableButtons()
    {
        foreach (Button button in buttonsToDisable)
        {
            button.interactable = true;
        }
        foreach(GameObject item in itemsToDisable)
        {
            item.SetActive(true);
        }
    }
}
