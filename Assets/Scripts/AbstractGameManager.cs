using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {
    public DataType.Minigame typeOfGame;
	public abstract void GameOver(); // Force GameOver() to be implemented in child classes
    public abstract void PregameSetup (); // Force PregameSetup() to be implemented in child classes
    public DataType.MonsterType typeOfMonster;
    public AudioClip[] backgroundMusicArray;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameManager.GetInstance ().SetLastGamePlayed (typeOfGame);
    }

    void OnEnable () {
        typeOfMonster = GameManager.GetInstance ().GetMonster ();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ReviewManager.OnFinishReview -= EndReview;
    }

    // Go to Start scene if no Game Manager is present
    protected void CheckForGameManager () {
        if (!GameManager.GetInstance ()) {
            SwitchScene switchScene = this.gameObject.AddComponent<SwitchScene> ();
            switchScene.LoadScene ("Start", false);
        }
    }

    protected void Start() {
        if (GameManager.GetInstance().GetLevel(typeOfGame) == 1) {
            print ("First level of game, needs review from other games");
        }
        /*
        print ("AbstractGameManager Start running");
        if (ReviewManager.GetInstance ().needReview) {
            StartReview ();
        } else {
            
        }
        */
        PregameSetup ();
    }

    protected void StartReview () {
        print ("AbstractGameManager StartReview");
        ReviewManager.OnFinishReview += EndReview;
        ReviewManager.GetInstance ().StartReview (typeOfGame);
    }

    protected void EndReview () {
        print ("AbstractGameManager EndReview");
        ReviewManager.OnFinishReview -= EndReview;
        PregameSetup ();
    }

    public virtual void UnlockSticker() {
        GameManager.GetInstance ().ActivateSticker (typeOfGame);
        GameManager.GetInstance ().CreateEndScreen (typeOfGame, EndScreen.EndScreenType.EarnedSticker);
    }
}
