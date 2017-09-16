using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewEmotionsGame : MonoBehaviour {
    public Text text;
    public bool isReviewRunning, inputAllowed;
    private static ReviewEmotionsGame instance;

    public static ReviewEmotionsGame GetInstance () {
        return instance;
    }

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    void Start () {
        PrepareReview ();
    }

    public void PrepareReview () {
        StartCoroutine (BeginReview ());
    }

    IEnumerator BeginReview () {
        yield return new WaitForSecondsRealtime (1f);
        isReviewRunning = true;
        inputAllowed = true;
    }

    public void EndReview () {
        isReviewRunning = false;
        inputAllowed = false;
        text.text = "Great job!";
        ReviewManager.GetInstance ().EndReview ();
    }
}
