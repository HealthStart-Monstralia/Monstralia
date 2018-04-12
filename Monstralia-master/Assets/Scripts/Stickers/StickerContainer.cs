using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerContainer : MonoBehaviour {
    [HideInInspector] public GameObject selectedSticker;
    public float shrinkSize = 200f;                // For rescaling the stickers

    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;
    private List<GameObject> stickerList = new List<GameObject> ();
    private int index = 0;
    private float originalWidth, originalHeight;    // For rescaling the stickers


    private void Start () {
        ActivateButtons (false);
    }

    public void AddSticker (GameObject stickerToAdd) {
        CreateSticker (stickerToAdd);
        if (stickerList.Count > 1) {
            ActivateButtons (true);
        }
    }

    void CreateSticker(GameObject stickerToCreate) {
        GameObject newSticker = Instantiate (stickerToCreate, transform.position, Quaternion.identity, transform);
        stickerList.Add (newSticker);
        newSticker.SetActive (false);
    }

    public void NextSticker () {
        if (index < stickerList.Count - 1) {
            stickerList[index].SetActive (false);
            index++;
            ChooseSticker ();
        }
        else if (stickerList.Count > 0) {
            stickerList[index].SetActive (false);
            index = 0;
            ChooseSticker ();
        }
    }

    public void PreviousSticker () {
        if (index > 0) {
            stickerList[index].SetActive (false);
            index--;
            ChooseSticker ();
        } else if (stickerList.Count > 0) {
            stickerList[index].SetActive (false);
            index = stickerList.Count - 1;
            ChooseSticker ();
        }
    }

    public void ChooseSticker () {
        if (stickerList.Count > 0) {
            selectedSticker = stickerList[index];
            selectedSticker.SetActive (true);
            selectedSticker.GetComponent<StickerBehaviour>().ShrinkSize (shrinkSize);
        }
        else {
            DisableStickerPanel ();
        }
    }

    public void RemoveSticker() {
        stickerList.RemoveAt (index);
        if (index > 0)
            index--;
        ChooseSticker ();

        if (stickerList.Count < 2) {
            ActivateButtons (false);
        }
    }

    void ActivateButtons (bool activate) {
        buttonLeft.interactable = activate;
        buttonRight.interactable = activate;
    }

    void DisableStickerPanel () {
        transform.parent.gameObject.GetComponent<Button> ().interactable = false;
    }
}
