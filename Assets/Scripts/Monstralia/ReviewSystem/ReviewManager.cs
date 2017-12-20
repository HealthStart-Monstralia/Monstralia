using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour {
    /* Review games are only added after completing the first level of a game.
     * A review game appears as a popup when a player selects a new game.
     * After completing the review game, the player will proceed to the originally intended game.
     * If the selected review game matches the one selected by player, it will be replaced by another from the pool
     * As a temporary measure to avoid aggrevating the player, review games will not appear every time.
     */
      
    public Dictionary<GameObject, DataType.Minigame> reviewGamesDict; // <review prefab object> <review prefab type>
    public List<GameObject> reviewGamesList = new List<GameObject> ();  // Pool of review games to pull from
    public GameObject currentReview;
    public bool needReview;

    public delegate void ReviewAction ();
    public static event ReviewAction OnFinishReview;

    private int reviewLevelIndex;
    private static ReviewManager instance;
    private GameObject reviewGameBase;      // Relies on child structure to find

    public static ReviewManager GetInstance() {
        return instance;
    }

    private void Awake() {
        // Establish as DontDestroyOnLoad/singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        reviewGameBase = transform.GetChild (0).gameObject;
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", false);
        reviewGameBase.SetActive (false);
        reviewGamesDict = new Dictionary<GameObject, DataType.Minigame> (); 
    }

    public void AddReviewGameToList(DataType.Minigame minigame) {
        print ("Adding review game for " + minigame);
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab; // Retrieve corresponding review prefab from MinigameData
        
        // If a review prefab exists
        if (reviewGame) {
            bool foundObject = reviewGamesList.Contains (reviewGame);

            // If selected review is not already loaded
            if (!foundObject) {

                // Check if review game is not already in dictionary
                if (!reviewGamesDict.ContainsKey (reviewGame)) {
                    reviewGamesDict.Add (reviewGame, minigame);
                }

                // Add the review game to the pool
                reviewGamesList.Add (reviewGame);
                print ("+ADDED review game for " + minigame);
            }
        }
    }

    public void RemoveReviewGameFromList(DataType.Minigame minigame) {
        reviewGamesList.Remove (currentReview);
    }

    public void StartReview(DataType.Minigame minigame) {
        // Check if there is at least 1 review game in the pool, otherwise terminate review
        print ("*STARTING REVIEW*");
        if (reviewGamesList.Count > 0) {
            print ("reviewGamesList.Count > 0");
            // Check if review game is assigned to a value in dictionary
            if (reviewGamesDict.ContainsKey(GameManager.GetInstance().GetMinigameData(minigame).reviewPrefab)) {
                print ("Checking if review game is assigned to a value in dictionary");
                int randNum = Random.Range (0, reviewGamesList.Count - 1);
                GameObject selectedReview = reviewGamesList[randNum];
                DataType.Minigame typeOfGame = reviewGamesDict[selectedReview];

                // If the same type of game matches review, choose another
                if (typeOfGame == minigame) {
                    print ("If type == minigame");
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
                    SpawnReview (selectedReview);
                } else
                    TerminateReview ();
            }

            else {
                TerminateReview ();
            }

        }
        else {
            TerminateReview ();
        }
    }

    // For review manager use
    void SpawnReview(GameObject objToSpawn) {
        SoundManager.GetInstance ().PlayReviewVO ();
        currentReview = Instantiate (objToSpawn, reviewGameBase.transform) as GameObject;
        reviewGameBase.SetActive (true);
    }

    // For creating a review without any conditions
    public void CreateReviewImmediately(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab;
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
        reviewGameBase.SetActive (true);
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", false);
        reviewGameBase.SetActive (false);
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
