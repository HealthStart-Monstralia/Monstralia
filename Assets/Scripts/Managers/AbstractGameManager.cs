using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractGameManager<T> : MonoBehaviour where T : Component {
    // Singleton property
    private static T instance = null;

    public static T Instance {
        get {
            if (!instance) {
                instance = (T)FindObjectOfType (typeof (T));
                print ("Instance FOUND: " + instance + instance.GetInstanceID ());
            }

            return instance;
        }
    }

    [Header ("AbstractGameManager Fields")]
    public DataType.Minigame typeOfGame;
    public abstract void PregameSetup (); // Force PregameSetup() to be implemented in child classes
    public DataType.MonsterType typeOfMonster;
    public bool randomizeMusic = true;
    public AudioClip[] backgroundMusicArray;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameManager.Instance.SetLastGamePlayed (typeOfGame);
    }

    public virtual void Awake () {
        if (!instance) {
            instance = this as T;
        } else if (instance != this)  {
            Destroy (gameObject);
        }
    }

    void OnEnable () {
        typeOfMonster = GameManager.Instance.GetMonsterType ();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable () {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected void Start() {
        if (backgroundMusicArray.Length > 0) {
            if (randomizeMusic) {
                SoundManager.Instance.ChangeAndPlayMusic (backgroundMusicArray.GetRandomItem());
            } else {
                SoundManager.Instance.ChangeAndPlayMusic (backgroundMusicArray[0]);
            }
        }

        PregameSetup ();
    }

    public virtual void GameOver (DataType.GameEnd typeOfEnd) {
        switch (typeOfEnd) {
            case DataType.GameEnd.EarnedSticker:
                SoundManager.Instance.PlaySFXClip (SoundManager.Instance.correctSfx2);
                SoundManager.Instance.PlayCorrectSFX ();
                GameManager.Instance.LevelUp (typeOfGame);
                GameManager.Instance.ActivateSticker (typeOfGame);
                break;
            case DataType.GameEnd.CompletedLevel:
                SoundManager.Instance.PlayCorrectSFX ();
                GameManager.Instance.LevelUp (typeOfGame);
                break;
            case DataType.GameEnd.FailedLevel:
                break;
        }

        GameManager.Instance.CreateEndScreen (typeOfGame, typeOfEnd);
    }

    public void StartCountdown (Countdown.CountdownCallback callback) {
        Instantiate (GameManager.Instance.countdownPrefab).GetComponent<Countdown>().StartCountDown (callback);
    }

    public void SetTimeLimit (float timeLimit) {
        TimerClock.Instance.SetTimeLimit (timeLimit);
    }

    public void AddTime (float time) {
        TimerClock.Instance.AddTime (time);
    }
}
