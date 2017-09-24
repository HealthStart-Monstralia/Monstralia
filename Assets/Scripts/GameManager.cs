using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private Dictionary<DataType.Minigame, MinigameStats> gameStats;
    private Dictionary<DataType.StickerType, StickerStats> stickerStats;
    private Dictionary<DataType.Minigame, MinigameData> minigameDictionary;
    private Dictionary<DataType.IslandSection, bool> visitedAreas;

    private DataType.MonsterType monsterType;
    private static GameManager instance = null;
	private bool allowInput = true;
    [SerializeField] private MinigameData[] minigameAssetData;
    //[SerializeField] private GameObject[] stickerObjects;
    private DataType.Minigame lastGamePlayed;
    private DataType.IslandSection currentSection;
    private bool activateReview = false; // Alternate activating review when game is lvl 3

    public struct MinigameStats {
        public int level;
        public bool isTutorialPending;
        public int stars;
    }

    public struct StickerStats {
        public GameObject stickerObject;
        public bool isStickerUnlocked;
        public bool isStickerPlaced;
    }

    public int numOfGamesPlayed = 0;
    public bool lagoonFirstSticker = true;
    //public bool playLagoonVoiceOver = true;
    public bool playIntro = true;
	public Canvas introObject;
    public GameObject loadingScreen;
    public GameObject endingScreen;
    public GameObject blueMonster, greenMonster, redMonster, yellowMonster;

    public static GameManager GetInstance () {
        return instance;
    }

    void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	}

    void Start () {
        InitializeDictionaryEntries ();
        if (playIntro) {
            Instantiate (introObject);
        }
    }

    void InitializeDictionaryEntries () {
        // Initialize dictionaries
        minigameDictionary = new Dictionary<DataType.Minigame, MinigameData> ();
        gameStats = new Dictionary<DataType.Minigame, MinigameStats> ();
        stickerStats = new Dictionary<DataType.StickerType, StickerStats> ();
        visitedAreas = new Dictionary<DataType.IslandSection, bool> ();

        // Initialize visitedAreas dictionary with false boolean
        foreach (DataType.IslandSection island in System.Enum.GetValues (typeof (DataType.IslandSection))) {
            visitedAreas.Add (island, false);
        }

        // Initialize dictionary with a user assigned array since Unity does not support serialized dictionaries
        foreach (MinigameData entry in minigameAssetData) {
            // Create a reference between the MinigameData enum and the asset data
            minigameDictionary.Add (entry.typeOfGame, entry);

            // Create and initialize stats for minigame and add it to gameStats Dictionary
            MinigameStats newGame;
            newGame.level = 1;
            newGame.stars = 0;
            newGame.isTutorialPending = true;
            gameStats.Add (entry.typeOfGame, newGame);

            // Create and initialize stats for corresponding sticker and add it to stickerStats Dictionary
            StickerStats newSticker;
            newSticker.stickerObject = entry.stickerPrefab;
            newSticker.isStickerPlaced = false;
            newSticker.isStickerUnlocked = false;
            stickerStats.Add (GetAssignedSticker(entry.typeOfGame), newSticker);
        }
    }

    // Called at the end of a minigame
    public void LevelUp (DataType.Minigame gameName) {
        MinigameStats newStats = gameStats[gameName]; // Copy current struct to a new one

        if (newStats.stars < 3) {
            newStats.stars += 1;
        }

        if (newStats.stars == 1 || newStats.stars == 3) {
            ReviewManager.GetInstance ().AddReviewGameToList (gameName);

            // Set needReview to true when lvl 3 is completed every other time, temporary measure to reduce annoying reviews
            if (newStats.stars >= 3) {
                if (activateReview) {
                    ReviewManager.GetInstance ().needReview = true;
                }
                activateReview = !activateReview;
            }
            else {
                ReviewManager.GetInstance ().needReview = true;
            }

        }

        if (newStats.level < 3) {
            newStats.level += 1;
        }

        numOfGamesPlayed++;
        gameStats[gameName] = newStats; // Save changes to new struct
    }

    public void CompleteTutorial(DataType.Minigame gameName) {
        // Copy current struct to a new one
        MinigameStats newStats = gameStats[gameName];

        // Modify desired variable
        newStats.isTutorialPending = false;

        // Save changes to new struct
        gameStats[gameName] = newStats;
    }

    /// <summary>
    /// Sets sticker to placed.
    /// </summary>
    /// <param name="typeOfSticker">Type of sticker to set placed</param>

    public void OnStickerPlaced (DataType.StickerType typeOfSticker) {
        // Copy current struct to a new one
        StickerStats newSticker = stickerStats[typeOfSticker];

        // Modify desired variable
        newSticker.isStickerPlaced = true;

        // Save changes to new struct
        stickerStats[typeOfSticker] = newSticker;
    }

    /// <summary>
    /// Unlocks a sticker for the corresponding game.
    /// </summary>
    /// <param name="game">Type of game to unlock sticker for.</param>

    public void ActivateSticker (DataType.Minigame game) {
        // Copy current struct to a new one
        DataType.StickerType gameSticker = GetAssignedSticker(game);
        StickerStats newSticker = stickerStats[gameSticker];

        // Modify desired variable
        if (!newSticker.isStickerUnlocked) {
            newSticker.isStickerUnlocked = true;

            // Save changes to new struct
            stickerStats[gameSticker] = newSticker;
        }
    }

    /// <summary>
    /// Unlocks a sticker for the corresponding game.
    /// </summary>
    /// <param name="game">Type of sticker to unlock.</param>

    public void ActivateSticker (DataType.StickerType sticker) {
        // Copy current struct to a new one
        StickerStats newSticker = stickerStats[sticker];

        // Modify desired variable
        if (!newSticker.isStickerUnlocked) {
            newSticker.isStickerUnlocked = true;

            // Save changes to new struct
            stickerStats[sticker] = newSticker;
        }
    }

    public void SetMonsterType (DataType.MonsterType monster) {
        monsterType = monster;
    }

    public DataType.MonsterType GetMonsterType () {
        return monsterType;
    }

    public GameObject GetMonster () {
        switch(monsterType) {
            case DataType.MonsterType.Blue:
                return blueMonster;
            case DataType.MonsterType.Green:
                return greenMonster;
            case DataType.MonsterType.Red:
                return redMonster;
            case DataType.MonsterType.Yellow:
                return yellowMonster;
            default:
                return greenMonster;
        }
    }

    public int GetLevel (DataType.Minigame gameName) {
        return gameStats[gameName].level;
    }

    public bool GetPendingTutorial (DataType.Minigame gameName) {
        return gameStats[gameName].isTutorialPending;
    }

    public int GetNumStars(DataType.Minigame gameName) {
		return gameStats[gameName].stars;
	}

    public MinigameData GetMinigameData(DataType.Minigame gameName) {
        return minigameDictionary[gameName];
    }

    /// <summary>
    /// Retrieves the type of sticker assigned to the game
    /// </summary>
    /// <param name="game">Type of game to unlock sticker for.</param>
    /// <returns>Returns a StickerType from DataType</returns>

    public DataType.StickerType GetAssignedSticker (DataType.Minigame game) {
        return minigameDictionary[game].stickerPrefab.GetComponent<StickerBehaviour>().typeOfSticker;
    }

    /// <summary>
    /// Retrieves the unlocked status of sticker assigned to the game
    /// </summary>
    /// <param name="game">Type of game to check status of sticker.</param>
    /// <returns>Returns whether sticker is unlocked or not.</returns>

    public bool GetStickerUnlock (DataType.Minigame game) {
        return stickerStats[GetAssignedSticker(game)].isStickerUnlocked;
    }

    public Dictionary<DataType.StickerType, StickerStats> GetStickerDict () {
        return stickerStats;
    }

    /// <summary>
    /// Creates a countdown with a voiceover.
    /// </summary>

    public void Countdown() {
		GetComponent<CreateCountdown>().SpawnCountdown();
	}

    public void DebugStickers() {
        foreach (DataType.StickerType sticker in System.Enum.GetValues (typeof (DataType.StickerType))) {
            ActivateSticker(sticker);
        }
    }

    public bool GetIsInputAllowed() {
		return allowInput;
	}

	public void SetIsInputAllowed(bool boolean) {
		allowInput = boolean;
		print ("Input Allowed: " + boolean);
	}

    public void SetLastGamePlayed (DataType.Minigame game) {
        lastGamePlayed = game;
    }

    public DataType.Minigame GetLastGamePlayed() {
        return lastGamePlayed;
    }

    public void SetIslandSection (DataType.IslandSection island) {
        currentSection = island;
    }

    public DataType.IslandSection GetIslandSection () {
        return currentSection;
    }

    public bool GetVisitedArea(DataType.IslandSection island) {
        return visitedAreas[island];
    }

    public bool SetVisitedArea (DataType.IslandSection island, bool isVisited) {
        return visitedAreas[island] = isVisited;
    }

    public EndScreen CreateEndScreen(DataType.Minigame game, EndScreen.EndScreenType type) {
        print ("Created End Screen of type: " + type);
        EndScreen screen = Instantiate (endingScreen).GetComponent<EndScreen> ();
        screen.game = game;
        screen.typeOfScreen = type;

        switch (type) {
            case EndScreen.EndScreenType.EarnedSticker:
                screen.EarnedSticker ();
                break;
            case EndScreen.EndScreenType.CompletedLevel:
                screen.CompletedLevel ();
                break;
            case EndScreen.EndScreenType.FailedLevel:
                screen.FailedLevel ();
                break;
            default:
                break;
        }

        return screen;
    }
}
