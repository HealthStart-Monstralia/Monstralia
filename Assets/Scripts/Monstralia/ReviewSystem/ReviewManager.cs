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
    private GameObject reviewGameBase;      // Relies on child structure to find

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
        reviewGameBase = transform.GetChild (0).gameObject;
        reviewGameBase.GetComponent<Animator> ().SetBool ("ReviewEnd", false);
        reviewGameBase.SetActive (false);
        reviewGamesDict = new Dictionary<GameObject, DataType.Minigame> (); // <review prefab object> <review prefab type>
    }

    public void AddReviewGameToList(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab; // Retrieve corresponding review prefab from MinigameData
        
        // If review prefab exists
        if (!reviewGame) {
            bool foundObject = reviewGamesList.Contains (reviewGame);
            if (!foundObject) {
                reviewGamesDict.Add (reviewGame, minigame);
                reviewGamesList.Add (reviewGame);
            }
        }
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
                SpawnReview (selectedReview);
            } else
                TerminateReview ();
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
