using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameButton : MonoBehaviour {
    public DataType.Minigame typeOfGame;
    private SwitchScene scene;

    private void Awake () {

        // If SwitchScene is already added, use it, if not add one and use the typeOfGame variable to desired load scene
        if (GetComponent<SwitchScene>()) {
            scene = GetComponent<SwitchScene> ();
        }
        else {
            scene = gameObject.AddComponent<SwitchScene> ();
            scene.sceneToLoadName = typeOfGame.ToString();
        }
    }

    public void LoadGame() {
        if (ReviewManager.GetInstance().needReview) {
            StartReview ();
        }
        else scene.LoadScene ();
    }

    void StartReview () {
        print ("MinigameButton StartReview");
        ReviewManager.OnFinishReview += EndReview;
        ReviewManager.GetInstance ().StartReview (typeOfGame);
    }

    void EndReview () {
        print ("MinigameButton EndReview");
        ReviewManager.OnFinishReview -= EndReview;
        scene.LoadScene ();
    }
}
