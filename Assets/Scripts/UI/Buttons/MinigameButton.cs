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

    private void Start () {
        Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale (gameObject, originalScale, 0.75f).setEaseOutBack ();
    }

    public void LoadGame() {
        print ("Loading: " + typeOfGame);
        if (ReviewManager.Instance) {
            if (ReviewManager.Instance.NeedReview) StartReview ();
            else scene.LoadScene ();
        }
        else scene.LoadScene ();
    }

    void StartReview () {
        print ("MinigameButton StartReview");
        ReviewManager.OnFinishReview += EndReview;
        ReviewManager.Instance.StartReview (typeOfGame);
    }

    void EndReview () {
        print ("MinigameButton EndReview");
        ReviewManager.OnFinishReview -= EndReview;
        scene.LoadScene ();
    }
}
