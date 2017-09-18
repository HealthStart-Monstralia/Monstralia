using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour {

    public GameObject[] ReviewGamePrefabs; //list of review games
    public Dictionary<GameObject, DataType.Minigame> reviewGamesDict;
    public List<GameObject> reviewGamesList = new List<GameObject> ();
    public GameObject currentReview;
    public bool needReview;

    public delegate void ReviewAction ();
    public static event ReviewAction OnFinishReview;

    private int reviewLevelIndex;
    private static ReviewManager instance;

    public static ReviewManager GetInstance() {
        return instance;
    }

    private void Awake() { 
        //to establish as dontdestroyonload/singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        reviewGamesDict = new Dictionary<GameObject, DataType.Minigame> (); // <review prefab object> <review prefab type>
    }

    public void AddReviewGameToList(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab; // Retrieve corresponding review prefab from MinigameData
        bool foundObject = reviewGamesList.Contains (reviewGame);
        if (!foundObject) {
            reviewGamesDict.Add (reviewGame, minigame);
            reviewGamesList.Add (reviewGame);
        }
        //needReview = true;
    }

    public void RemoveReviewGameFromList(DataType.Minigame minigame) {
        reviewGamesList.Remove (currentReview);
    }

    public void StartReview(DataType.Minigame minigame) {
        if (reviewGamesList.Count > 0) {
            int randNum = Random.Range (0, reviewGamesList.Count - 1);
            GameObject selectedReview = reviewGamesList[randNum];
            DataType.Minigame typeOfGame = reviewGamesDict[selectedReview];

            // If the same type of game matches review, choose another
            if (typeOfGame == minigame) {
                if (reviewGamesList.Count <= 1) {
                    GameObject temp = reviewGamesList[randNum];
                    reviewGamesList.RemoveAt (randNum);

                    if (reviewGamesList.Count != 0) {
                        randNum = Random.Range (0, reviewGamesList.Count - 1);
                        selectedReview = reviewGamesList[randNum];

                    } else {
                        selectedReview = null;
                    }

                    reviewGamesList.Add (temp);
                }
            }

            if (selectedReview) {
                SoundManager.GetInstance ().PlayReviewVO ();
                currentReview = Instantiate (selectedReview) as GameObject;
            } else
                TerminateReview ();
        }
        else {
            TerminateReview ();
        }
    }

    public void CreateReview(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab;
        currentReview = Instantiate (reviewGame);
    }

    public void EndReview () {
        // Add delay timer in parameters, add an overload function perhaps
        StartCoroutine (WaitTillReviewEnd (3f));
    }

    IEnumerator WaitTillReviewEnd(float time) {
        yield return new WaitForSecondsRealtime (time);
        TerminateReview ();
    }

    void TerminateReview () {
        needReview = false;
        if (currentReview) {
            Destroy (currentReview);
            currentReview = null;
        } else if (GameObject.FindGameObjectWithTag ("ReviewPrefab"))
            Destroy (GameObject.FindGameObjectWithTag ("ReviewPrefab"));
        print ("ReviewManager OnFinishReview");
        if (OnFinishReview != null)
            OnFinishReview ();
        else
            print ("OnFinishReview returned null");
    }
}   
