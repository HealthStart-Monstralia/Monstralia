using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour {
    public Text starsText;
    public Text titleText;
    public GameObject tutorialButton, reviewButton, stickerButton;
    public DataType.Minigame gameName; // Must match same name as in Game Manager

    private int starCount;

	void Awake () {
        starCount = GameManager.Instance.GetNumStars (gameName);
        starsText.text = "Stars: " + starCount;
        titleText.text = gameName.ToString ();
        if (!GameManager.Instance.GetPendingTutorial (gameName)) {
            tutorialButton.SetActive (false);
        }

        if (GameManager.Instance.GetStickerUnlock (gameName)) {
            stickerButton.SetActive (false);
        }
    }
	
	void Update () {
        starCount = GameManager.Instance.GetNumStars (gameName);
        starsText.text = "Stars: " + starCount;
    }

    public void LevelComplete () {
        GameManager.Instance.LevelUp (gameName);
    }

    public void TutorialComplete () {
        GameManager.Instance.CompleteTutorial(gameName);
        if (!GameManager.Instance.GetPendingTutorial (gameName)) {
            tutorialButton.SetActive (false);
        }
    }

    public void CreateReview() {
        ReviewManager.Instance.CreateReviewImmediately (gameName);
    }

    public void ActivateSticker () {
        GameManager.Instance.ActivateSticker (gameName);
        if (GameManager.Instance.GetStickerUnlock (gameName)) {
            stickerButton.SetActive (false);
        }
    }

}
