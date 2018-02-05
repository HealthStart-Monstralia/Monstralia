using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GameManager : SingletonPersistent<GameManager> {
    private Dictionary<DataType.Minigame, MinigameStats> gameStats;
    private Dictionary<DataType.StickerType, StickerStats> stickerStats;
    private Dictionary<DataType.Minigame, MinigameData> minigameDictionary;
    private Dictionary<DataType.IslandSection, bool> visitedAreas;

    private DataType.MonsterType playerMonsterType;
    private MinigameData[] minigameAssetData;   // Loaded from InitializeDictionaryEntries()
    private DataType.Minigame lastGamePlayed;
    private DataType.IslandSection currentSection;
    private bool activateReview = false; // Alternate activating review when game is lvl 3
    private int numOfGamesCompleted = 0;

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

    public GameObject loadingScreenPrefab;
    public GameObject endingScreenPrefab;
    public GameObject exitPrefab;
    public GameObject countdownPrefab;
    public GameObject blueMonster, greenMonster, redMonster, yellowMonster;

    new void Awake() {
        base.Awake ();
        InitializeDictionaryEntries ();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
            gameObject.AddComponent<ExitHandler> ();
        }
    }

    void InitializeDictionaryEntries () {
        // Load all minigame asset data in the Minigames folder
        minigameAssetData = Resources.LoadAll<MinigameData> ("Data/Minigames");

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
            if (entry.stickerPrefab) {
                StickerStats newSticker;
                newSticker.stickerObject = entry.stickerPrefab;
                newSticker.isStickerPlaced = false;
                newSticker.isStickerUnlocked = false;
                stickerStats.Add (GetAssignedSticker (entry.typeOfGame), newSticker);
            }
        }
    }

    // Called at the end of a minigame
    public void LevelUp (DataType.Minigame gameName) {
        MinigameStats newStats = gameStats[gameName]; // Copy current struct to a new one
        numOfGamesCompleted++;

        if (newStats.stars < 3) newStats.stars += 1;
        if (newStats.level < 3) newStats.level += 1;

        if (newStats.stars == 1 || newStats.stars == 3) {
            if (ReviewManager.Instance)
                ReviewManager.Instance.AddReviewGameToList (gameName);

            // Set needReview to true when lvl 3 is completed every other time, temporary measure to reduce annoying reviews
            if (newStats.stars >= 3) {
                if (activateReview) {
                    if (ReviewManager.Instance)
                        ReviewManager.Instance.NeedReview = true;
                }
                activateReview = !activateReview;
            }
            else {
                if (ReviewManager.Instance)
                    ReviewManager.Instance.NeedReview = true;
            }

        }
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
        if (gameSticker != DataType.StickerType.None) {
            StickerStats newSticker = stickerStats[gameSticker];

            // Modify desired variable
            if (!newSticker.isStickerUnlocked) {
                newSticker.isStickerUnlocked = true;

                // Save changes to new struct
                stickerStats[gameSticker] = newSticker;
            }
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

    public void SetPlayerMonsterType (DataType.MonsterType monster) {
        playerMonsterType = monster;
    }

    public DataType.MonsterType GetPlayerMonsterType () {
        return playerMonsterType;
    }
    
    public GameObject GetPlayerMonsterObject () {
        return GetMonsterObject (playerMonsterType);
    }

    public GameObject GetMonsterObject (DataType.MonsterType typeOfMonster) {
        switch (typeOfMonster) {
            case DataType.MonsterType.Blue:
                return blueMonster;
            case DataType.MonsterType.Green:
                return greenMonster;
            case DataType.MonsterType.Red:
                return redMonster;
            case DataType.MonsterType.Yellow:
                return yellowMonster;
            default:
                return blueMonster;
        }
    }

    public GameObject[] GetMonstersInArray () {
        GameObject[] monsterGroup = new GameObject[Constants.NUM_OF_MONSTERS];
        for (int i = 0; i < Constants.NUM_OF_MONSTERS; i++) {
            monsterGroup[i] = GetMonsterObject ((DataType.MonsterType)i);
        }

        return monsterGroup;
    }

    public List<GameObject> GetMonstersInList () {
        List<GameObject> monsterList = new List<GameObject>(Constants.NUM_OF_MONSTERS);
        for (int i = 0; i < Constants.NUM_OF_MONSTERS; i++) {
            monsterList.Add(GetMonsterObject ( (DataType.MonsterType)i ) );
        }

        return monsterList;
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
        GameObject stickerPrefab = minigameDictionary[game].stickerPrefab;
        if (stickerPrefab)
            return stickerPrefab.GetComponent<StickerBehaviour> ().typeOfSticker;
        else
            return DataType.StickerType.None;
    }

    /// <summary>
    /// Retrieves the unlocked status of sticker assigned to the game
    /// </summary>
    /// <param name="game">Type of game to check status of sticker.</param>
    /// <returns>Returns whether sticker is unlocked or not.</returns>

    public bool GetStickerUnlock (DataType.Minigame game) {
        if (stickerStats.ContainsKey(GetAssignedSticker (game)))
            return stickerStats[GetAssignedSticker (game)].isStickerUnlocked;
        else
            return false;
    }

    public Dictionary<DataType.StickerType, StickerStats> GetStickerDict () {
        return stickerStats;
    }

    public void DebugStickers() {
        foreach (DataType.StickerType sticker in System.Enum.GetValues (typeof (DataType.StickerType))) {
            if (sticker != DataType.StickerType.None) {
                print (sticker);
                ActivateSticker (sticker);
            }
        }
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

    public int GetNumOfGamesCompleted() {
        return numOfGamesCompleted;
    }
}
