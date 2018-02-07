using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewEmotionsGame : Singleton<ReviewEmotionsGame> {
    public Text text;
    public bool isReviewRunning, inputAllowed;

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
        ReviewManager.Instance.EndReview ();
    }
}
