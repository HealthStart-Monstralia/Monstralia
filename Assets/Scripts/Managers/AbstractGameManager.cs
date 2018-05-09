using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class AbstractGameManager<T> : MonoBehaviour where T : Component {
    // Singleton property
    private static T instance = null;

    public static T Instance {
        get {
            if (!instance) {
                instance = (T)FindObjectOfType (typeof (T));
            }
            return instance;
        }
    }

    [Header ("AbstractGameManager Fields")]
    public DataType.Minigame typeOfGame;
    public abstract void PregameSetup (); // Force PregameSetup() to be implemented in child classes
    public bool randomizeMusic = true;
    public AudioClip[] backgroundMusicArray;
    [HideInInspector] public Monster playerMonster;
    [HideInInspector] public CreateMonster monsterCreator;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameManager.Instance.SetLastGamePlayed (typeOfGame);
    }

    public virtual void Awake () {
        if (!instance) {
            instance = this as T;
        } else if (instance != this)  {
            Destroy (gameObject);
        }

        GameObject loadingScreen = GameManager.Instance.CreateLoadingScreen ();
        if (loadingScreen) {
            LeanTween.alphaCanvas (loadingScreen.GetComponent<CanvasGroup> (), 0f, 0.5f).setEaseInCubic ();
            Destroy (loadingScreen, 0.5f);
        }
    }

    void OnEnable () {
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

    public virtual EndScreen GameOver (DataType.GameEnd typeOfEnd) {
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

        return CreateEndScreen (typeOfGame, typeOfEnd);
    }

    /// <summary>
    /// Creates a countdown canvas and begins to countdown after specified wait duration. After countdown finishes, the callback function is called.
    /// </summary>
    /// <param name="callback">The function to call immediately after finishing countdown.</param>
    /// <param name="waitDuration">A pause in secs before starting the countdown.</param>
    public void StartCountdown (Countdown.CountdownCallback callback, float waitDuration = 0.5f) {
        Instantiate (GameManager.Instance.countdownPrefab).GetComponent<Countdown>().StartCountDown (callback, waitDuration);
    }

    public void SetTimeLimit (float timeLimit) {
        TimerClock.Instance.SetTimeLimit (timeLimit);
    }

    public void AddTime (float time) {
        TimerClock.Instance.AddTime (time);
    }

    public void SubtractTime (float time) {
        TimerClock.Instance.SubtractTime (time);
    }

    public void StartTimer () {
        TimerClock.Instance.StartTimer ();
    }

    public void StopTimer() {
        TimerClock.Instance.StopTimer ();
    }

    public float GetTimeRemaining() {
        return TimerClock.Instance.TimeRemaining;
    }

    public Monster CreatePlayerMonster (Transform pos) {
        monsterCreator = gameObject.AddComponent<CreateMonster> ();
        playerMonster = monsterCreator.SpawnPlayerMonster (pos);
        return playerMonster;
    }

    public void CreateMonster (DataType.MonsterType typeOfMonster, Transform pos) {
        monsterCreator = gameObject.AddComponent<CreateMonster> ();
        playerMonster = monsterCreator.SpawnMonster (typeOfMonster, pos);
    }

    public EndScreen CreateEndScreen (DataType.Minigame game, DataType.GameEnd type) {
        print ("Created End Screen of type: " + type);
        EndScreen screen = Instantiate (GameManager.Instance.endingScreenPrefab).GetComponent<EndScreen> ();
        screen.typeOfGame = game;
        screen.typeOfScreen = type;

        switch (type) {
            case DataType.GameEnd.EarnedSticker:
                screen.EarnedSticker ();
                break;
            case DataType.GameEnd.CompletedLevel:
                screen.CompletedLevel ();
                break;
            case DataType.GameEnd.FailedLevel:
                screen.FailedLevel ();
                break;
            default:
                break;
        }

        return screen;
    }
}
