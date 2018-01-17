using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {
    [Header ("AbstractGameManager Fields")]
    public DataType.Minigame typeOfGame;
    public abstract void PregameSetup (); // Force PregameSetup() to be implemented in child classes
    public DataType.MonsterType typeOfMonster;
    public bool randomizeMusic = true;
    public AudioClip[] backgroundMusicArray;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameManager.GetInstance ().SetLastGamePlayed (typeOfGame);
    }

    void OnEnable () {
        if (GameManager.GetInstance ()) {
            typeOfMonster = GameManager.GetInstance ().GetMonsterType ();
        }
        else {
            typeOfMonster = DataType.MonsterType.Blue;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Go to Start scene if no Game Manager is present
    protected void CheckForGameManager () {
        if (!GameManager.GetInstance ()) {
            SwitchScene switchScene = gameObject.AddComponent<SwitchScene> ();
            switchScene.LoadSceneNoScreen ("Start");
        }
    }

    protected void Start() {
        if (backgroundMusicArray.Length > 0) {
            if (randomizeMusic) {
                SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusicArray[Random.Range (0, backgroundMusicArray.Length)]);
            } else {
                SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusicArray[0]);
            }
        }

        PregameSetup ();
    }

    public virtual void GameOver (DataType.GameEnd typeOfEnd) {
        switch (typeOfEnd) {
            case DataType.GameEnd.EarnedSticker:
                SoundManager.GetInstance ().PlaySFXClip (SoundManager.GetInstance ().correctSfx2);
                SoundManager.GetInstance ().PlayCorrectSFX ();
                GameManager.GetInstance ().LevelUp (typeOfGame);
                GameManager.GetInstance ().ActivateSticker (typeOfGame);
                break;
            case DataType.GameEnd.CompletedLevel:
                SoundManager.GetInstance ().PlayCorrectSFX ();
                GameManager.GetInstance ().LevelUp (typeOfGame);
                break;
            case DataType.GameEnd.FailedLevel:
                break;
        }

        GameManager.GetInstance ().CreateEndScreen (typeOfGame, typeOfEnd);
    }

}
