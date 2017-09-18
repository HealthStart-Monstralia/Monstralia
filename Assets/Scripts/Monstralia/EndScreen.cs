using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {
    public enum EndScreenType {
        EarnedSticker,
        CompletedLevel,
        FailedLevel
    }

    public EndScreenType typeOfScreen;
    public DataType.Minigame game;
    public bool earnedSticker;
    public Text headerText, footerText;
    public GameObject stickerButton, backButton, nextLevelButton, imageLocation, brain;

    private void Awake () {
        GetComponent<Canvas> ().worldCamera = Camera.main;
    }

    public void EarnedSticker() {
        stickerButton.SetActive (true);
        brain.SetActive (false);
        SoundManager.GetInstance ().PlayCorrectSFX ();
        GameObject sticker = Instantiate (GameManager.GetInstance ().GetMinigameData (game).stickerPrefab, imageLocation.transform);
        sticker.transform.localPosition = Vector3.zero;
        Destroy (sticker.GetComponent<StickerBehaviour> ());
        headerText.text = "Congratulations you earned a new sticker!";
        footerText.text = "Tap on the button below to use your new sticker!";
    }

    public void EarnedSticker (string header, string footer) {
        stickerButton.SetActive (true);
        brain.SetActive (false);
        GameObject sticker = Instantiate (GameManager.GetInstance ().GetMinigameData (game).stickerPrefab, imageLocation.transform);
        sticker.transform.localPosition = Vector3.zero;
        Destroy (sticker.GetComponent<StickerBehaviour> ());
        headerText.text = header;
        footerText.text = footer;
    }

    public void CompletedLevel () {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = "Great job you completed " + SceneManager.GetActiveScene().name + "!";
        footerText.text = "Tap on the back button to go back or" +
            " tap on the next button to continue playing";
    }

    public void CompletedLevel (string header, string footer) {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = header;
        footerText.text = footer;
    }

    public void FailedLevel () {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = "Game over!";
        footerText.text = "Tap on the back button to go back or" +
            " tap on the play again button to try again";
    }

    public void FailedLevel (string header, string footer) {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = header;
        footerText.text = footer;
    }
}
