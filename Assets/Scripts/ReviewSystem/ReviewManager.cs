using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : SingletonPersistent<ReviewManager> {
    /* Review games are only added after completing the first level of a game.
     * A review game appears as a popup when a player selects a new game.
     * After completing the review game, the player will proceed to the originally intended game.
     * If the selected review game matches the one selected by player, a review will not be created.
     * As a temporary measure to avoid aggrevating the player, review games will not appear every time.
     */
      
    public List<DataType.Minigame> reviewGamesList = new List<DataType.Minigame> ();  // Pool of review games to pull from
    public GameObject currentReview;
    public bool NeedReview {
        get {
            print ("needReview: " + _needReview);
            return _needReview;
        }
        set {
            print ("SET needReview to: " + value);
            _needReview = value;
        }
    }

    public delegate void ReviewAction ();
    public static event ReviewAction OnFinishReview;

    private bool _needReview = false;
    private int reviewLevelIndex;
    private GameObject reviewGameBase;      // Relies on child structure to find

    private new void Awake() {
        base.Awake ();

        reviewGameBase = transform.GetChild (0).gameObject;
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", false);
        reviewGameBase.SetActive (false);
    }

    public void AddReviewGameToList(DataType.Minigame minigame) {
        print ("Adding review game for " + minigame);
        if (!reviewGamesList.Contains(minigame) && GameManager.Instance.GetMinigameData (minigame).reviewPrefab) {
            reviewGamesList.Add (minigame);
        }
    }

    public void RemoveReviewGameFromList(DataType.Minigame minigame) {
        reviewGamesList.Remove (minigame);
    }

    public void StartReview(DataType.Minigame minigame) {
        // Check if there is at least 1 review game in the pool, otherwise terminate review
        print ("*CHECKING REVIEW CONDITIONS*");

        if (reviewGamesList.Count > 0) {
            DataType.Minigame selectedReview = reviewGamesList.GetRandomItem();

            // If the same type of game matches review, don't review
            if (minigame != selectedReview) {
                SpawnReview (GameManager.Instance.GetMinigameData (selectedReview).reviewPrefab);
            }
        }

        TerminateReview ();
    }

    // For review manager use
    void SpawnReview(GameObject objToSpawn) {
        print ("**STARTING REVIEW**");
        SoundManager.Instance.PlayReviewVO ();
        currentReview = Instantiate (objToSpawn, reviewGameBase.transform) as GameObject;
        reviewGameBase.SetActive (true);
    }

    // For creating a review without any conditions
    public void CreateReviewImmediately(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.Instance.GetMinigameData (minigame).reviewPrefab;
        if (reviewGame)
            SpawnReview (reviewGame);
    }

    public void EndReview () {
        StartCoroutine (WaitTillReviewEnd (3f));
    }

    public void EndReview (float time) {
        StartCoroutine (WaitTillReviewEnd (time));
    }

    IEnumerator WaitTillReviewEnd(float time) {
        yield return new WaitForSecondsRealtime (time - 0.5f);
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", true);
        yield return new WaitForSecondsRealtime (0.5f);
        TerminateReview ();
    }

    void TerminateReview () {
        print ("**TERMINATING REVIEW**");
        reviewGameBase.SetActive (true);
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", false);
        reviewGameBase.SetActive (false);
        NeedReview = false;
        if (currentReview) {
            Destroy (currentReview);
            currentReview = null;
        } else if (GameObject.FindGameObjectWithTag ("ReviewPrefab"))
            Destroy (GameObject.FindGameObjectWithTag ("ReviewPrefab"));
        if (OnFinishReview != null)
            OnFinishReview ();
        else
            print ("OnFinishReview returned null");
    }
}   
