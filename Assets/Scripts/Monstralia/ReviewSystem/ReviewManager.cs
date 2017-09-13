using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour {

    public GameObject[] ReviewGamePrefabs; //list of review games
    public string levelToReview; // bad code smell, open to other options
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

    private void Awake() { //to establish as dontdestroyonload/singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        reviewGamesDict = new Dictionary<GameObject, DataType.Minigame> (); // <review prefab object> <review prefab type>
        /*
        reviewgamesAndGamesDict.Add ("BrainbowReviewGame", DataType.Minigame.Brainbow);
        reviewgamesAndGamesDict.Add ("MemoryMatchReviewGame", DataType.Minigame.MemoryMatch);
        reviewgamesAndGamesDict.Add ("SensesReviewGame", DataType.Minigame.MonsterSenses);
        reviewgamesAndGamesDict.Add ("EmotionsReviewGame", DataType.Minigame.MonsterEmotions);
        reviewgamesAndGamesDict.Add ("BrainmazeReviewGame", DataType.Minigame.BrainMaze);
        */
    }

    public void AddReviewGameToList(DataType.Minigame minigame) {
        GameObject reviewGame = GameManager.GetInstance ().GetMinigameData (minigame).reviewPrefab; // Retrieve corresponding review prefab from MinigameData
        reviewGamesDict.Add (reviewGame, minigame);
        reviewGamesList.Add (reviewGame);
        needReview = true;
    }

    public void RemoveReviewGameFromList(DataType.Minigame minigame) {
        reviewGamesList.Remove (currentReview);
    }

    public void StartReview(DataType.Minigame minigame) {
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

        //currentReview = Instantiate (reviewGamesList[Random.Range(0, reviewGamesList.Count - 1)]) as GameObject;
    }

    public void EndReview () {
        StartCoroutine (WaitTillReviewEnd (3f));
    }

    IEnumerator WaitTillReviewEnd(float time) {
        yield return new WaitForSecondsRealtime (time);
        TerminateReview ();
    }

    void TerminateReview () {
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

    /*
    void OnEnable () {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        print("scene loaded");
        for (int i = 0; i < ReviewGamePrefabs.Length; i++) { // traverse array
            if (ReviewGamePrefabs[i].name == levelToReview) { // check for match
                reviewLevelIndex = i; // set review game
            }
        }
            // if scene is game level and need review == true
        if (levelToReview != "") {
            if (GameManager.GetInstance ().GetNumStars (reviewgamesAndGamesDict [levelToReview]) != 2 && SceneManager.GetActiveScene ().buildIndex > 4) { 
                GameObject activeReviewGame = Instantiate (ReviewGamePrefabs [reviewLevelIndex]) as GameObject; // GameObject.Find ("Review").transform
            }
        }
    }
    */
}   
