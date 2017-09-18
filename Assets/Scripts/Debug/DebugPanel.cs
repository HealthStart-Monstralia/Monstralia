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
        starCount = GameManager.GetInstance ().GetNumStars (gameName);
        starsText.text = "Stars: " + starCount;
        titleText.text = gameName.ToString ();
        if (!GameManager.GetInstance ().GetPendingTutorial (gameName)) {
            tutorialButton.SetActive (false);
        }

        if (GameManager.GetInstance ().GetStickerUnlock (gameName)) {
            stickerButton.SetActive (false);
        }
    }
	
	void Update () {
        starCount = GameManager.GetInstance ().GetNumStars (gameName);
        starsText.text = "Stars: " + starCount;
    }

    public void LevelComplete () {
        GameManager.GetInstance ().LevelUp (gameName);
    }

    public void TutorialComplete () {
        GameManager.GetInstance ().CompleteTutorial(gameName);
        if (!GameManager.GetInstance ().GetPendingTutorial (gameName)) {
            tutorialButton.SetActive (false);
        }
    }

    public void CreateReview() {
        ReviewManager.GetInstance ().CreateReview (gameName);
    }

    public void ActivateSticker () {
        GameManager.GetInstance ().ActivateSticker (gameName);
        if (GameManager.GetInstance ().GetStickerUnlock (gameName)) {
            stickerButton.SetActive (false);
        }
    }

}
