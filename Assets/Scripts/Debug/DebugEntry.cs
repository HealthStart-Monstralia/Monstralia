using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugEntry : MonoBehaviour {
    public Text starsText;
    public Text titleText;
    public GameObject levelButton, tutorialButton, reviewButton, stickerButton;
    public DataType.Minigame gameName; // Must match same name as in Game Manager

    private int starCount;

    void Start () {
        CheckStars ();
        titleText.text = gameName.ToString ();

        CheckTutorialButton ();
        CheckStickerButton ();
        CheckReviewButton ();
    }

    public void LevelComplete () {
        GameManager.Instance.LevelUp (gameName);
        CheckStars ();
    }

    public void TutorialComplete () {
        GameManager.Instance.CompleteTutorial(gameName);
        CheckTutorialButton ();
    }

    public void CreateReview() {
        ReviewManager.Instance.CreateReviewImmediately (gameName);
    }

    public void ActivateSticker () {
        GameManager.Instance.ActivateSticker (gameName);
        CheckStickerButton ();
    }

    void CheckStickerButton () {
        if (!GameManager.Instance.GetMinigameData (gameName).stickerPrefab) {
            DisableButton (stickerButton);
        }
        else if (GameManager.Instance.GetIsStickerUnlocked (gameName)) {
            DisableButton (stickerButton);
        }
    }

    void CheckTutorialButton () {
        if (!GameManager.Instance.GetPendingTutorial (gameName)) {
            DisableButton (tutorialButton);
        }
    }

    void CheckReviewButton () {
        if (!GameManager.Instance.GetMinigameData (gameName).reviewPrefab) {
            DisableButton (reviewButton);
        }
    }

    void DisableButton (GameObject buttonToDisable) {
        buttonToDisable.SetActive (false);
    }

    void CheckStars () {
        starCount = GameManager.Instance.GetNumStars (gameName);
        starsText.text = starCount.ToString();
    }

}
