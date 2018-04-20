using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {
    public DataType.GameEnd typeOfScreen;
    public DataType.Minigame typeOfGame;
    public bool earnedSticker;
    public Text headerText, footerText;
    public GameObject stickerButton, backButton, nextLevelButton, imageLocation, brain;
    public AudioClip unlockSticker;
    [SerializeField] private Button[] buttonsToDisableOnFirstWin;

    private void Awake () {

        GetComponent<Canvas> ().worldCamera = Camera.main;
    }

    public void EarnedSticker() {
        stickerButton.SetActive (true);
        brain.SetActive (false);
        SoundManager.Instance.PlayCorrectSFX ();
        SoundManager.Instance.PlayVoiceOverClip (unlockSticker);
        if (GameManager.Instance.GetMinigameData (typeOfGame).stickerPrefab) {
            GameObject sticker = Instantiate (GameManager.Instance.GetMinigameData (typeOfGame).stickerPrefab, imageLocation.transform);
            sticker.transform.localPosition = Vector3.zero;
            Destroy (sticker.GetComponent<StickerBehaviour> ());
        }

        headerText.text = "Congratulations you earned a new sticker!";

        if (!GameManager.Instance.GetHasPlayerVisitedStickerbook ()) {
            foreach (Button button in buttonsToDisableOnFirstWin) {
                button.interactable = false;
            }
            footerText.text = "Let's go to the stickerbook to show how to use your new sticker!";

        } else {
            footerText.text = "Tap on the button below to use your new sticker!";
        }
    }

    public void CompletedLevel () {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = "Great job you completed " + SceneManager.GetActiveScene().name + "!";
        footerText.text = "You can go back to " + GameManager.Instance.GetIslandSection().ToString()
            + " or play again!";
    }

    public void FailedLevel () {
        stickerButton.SetActive (false);
        brain.SetActive (true);
        headerText.text = "Game over!";
        footerText.text = "Tap on the back button to go back or" +
            " tap on the play again button to try again";
    }

    public void EditHeader(string header) {
        headerText.text = header;
    }

    public void EditFooter (string footer) {
        footerText.text = footer;
    }
}
