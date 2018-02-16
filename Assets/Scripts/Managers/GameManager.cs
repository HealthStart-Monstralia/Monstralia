using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class GameManager : SingletonPersistent<GameManager> {
    private Dictionary<DataType.Minigame, MinigameData> minigameDictionary = new Dictionary<DataType.Minigame, MinigameData> ();
    private Dictionary<DataType.StickerType, GameObject> stickerDictionary = new Dictionary<DataType.StickerType, GameObject> ();
    private MinigameData[] minigameAssetData;   // Loaded from InitializeDictionaryEntries()
    private DataType.IslandSection currentSection;
    private bool activateReview = false; // Alternate activating review when game is lvl 3
    private bool isSaveAllowed = false;

    // Can be loaded from a save file
    private Dictionary<DataType.Minigame, MinigameStats> gameStats = new Dictionary<DataType.Minigame, MinigameStats>();
    private Dictionary<DataType.StickerType, StickerStats> stickerStats = new Dictionary<DataType.StickerType, StickerStats>();
    private Dictionary<DataType.IslandSection, bool> visitedAreas = new Dictionary<DataType.IslandSection, bool>();
    private DataType.Minigame lastGamePlayed;
    private DataType.MonsterType playerMonsterType;
    private bool isMonsterSelected = false;
    private int numOfGamesCompleted = 0;

    [Serializable]
    public struct MinigameStats {
        public int level;
        public bool isTutorialPending;
        public int stars;
    }

    [Serializable]
    public struct StickerStats {
        public bool isStickerUnlocked;
        public bool isStickerPlaced;
    }

    [HideInInspector] public bool isIntroShown = false;
    public GameObject fpsCounter;
    public GameObject notificationPrefab;
    public GameObject loadingScreenPrefab;
    public GameObject endingScreenPrefab;
    public GameObject countdownPrefab;
    public GameObject blueMonster, greenMonster, redMonster, yellowMonster;

    new void Awake() {
        //base.Awake ();
        // If not on Android or Windows Unity Editor, remove exit handler
        if (GetComponent<ExitHandler> () && Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor) {
            Destroy (GetComponent<ExitHandler> ());
        }
    }

    private void Start () {
        InitializeDictionaryEntries ();
    }

    void InitializeDictionaryEntries () {
        print ("Initializing dictionary entries");
        // Load all minigame asset data in the Minigames folder
        minigameAssetData = Resources.LoadAll<MinigameData> ("Data/Minigames");

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

            // Check if MinigameData entry has a sticker prefab
            if (entry.stickerPrefab) {
                // Create a reference to the sticker object from type of sticker
                stickerDictionary.Add (GetAssignedSticker (entry.typeOfGame), entry.stickerPrefab);

                // Create and initialize stats for corresponding sticker and add it to stickerStats Dictionary
                StickerStats newSticker;
                newSticker.isStickerPlaced = false;
                newSticker.isStickerUnlocked = false;
                stickerStats.Add (GetAssignedSticker (entry.typeOfGame), newSticker);
            }
        }
    }

    void InitializeFromLoad (GameSave save) {
        print ("Initializing entries from save file");
        gameStats = save.gameStats;
        stickerStats = save.stickerStats;
        visitedAreas = save.visitedAreas;
        lastGamePlayed = save.lastGamePlayed;
        isMonsterSelected = save.isMonsterSelected;
        playerMonsterType = save.playerMonsterType;
        numOfGamesCompleted = save.numOfGamesCompleted;
        isIntroShown = save.isIntroShown;
        FoodList.SetFoodDictionary (save.foodEatenDictionary);
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

        // Save changes to save data
        SaveGame ();
    }

    public void CompleteTutorial(DataType.Minigame gameName) {
        // Copy current struct to a new one
        MinigameStats newStats = gameStats[gameName];

        // Modify desired variable
        newStats.isTutorialPending = false;

        // Save changes to new struct
        gameStats[gameName] = newStats;

        // Save changes to save data
        SaveGame ();
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

        // Save changes to save data
        SaveGame ();
    }

    /// <summary>
    /// Unlocks a sticker for the corresponding game.
    /// </summary>
    /// <param name="game">Type of game to unlock sticker for.</param>

    public void ActivateSticker (DataType.Minigame game) {
        // Copy current struct to a new one
        DataType.StickerType gameSticker = GetAssignedSticker(game);
        if (gameSticker != DataType.StickerType.None) {
            ActivateSticker (gameSticker);
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

            // Save changes to save data
            SaveGame ();
        }
    }

    public void SetPlayerMonsterType (DataType.MonsterType monster) {
        playerMonsterType = monster;
        isMonsterSelected = true;

        // Save changes to save data
        SaveGame ();
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

    /// <summary>
    /// Retrieves the current level of a game
    /// </summary>
    /// <param name="gameName">Type of game to retrieve current level for.</param>
    /// <returns>Returns the current level of game, first level has a value of 1.</returns>
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

    public bool GetIsStickerUnlocked (DataType.Minigame game) {
        if (stickerStats.ContainsKey(GetAssignedSticker (game)))
            return stickerStats[GetAssignedSticker (game)].isStickerUnlocked;
        else
            return false;
    }

    public GameObject GetStickerObject (DataType.StickerType typeOfSticker) {
        if (stickerDictionary.ContainsKey(typeOfSticker)) {
            return stickerDictionary[typeOfSticker];
        }

        return null;
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

        // Save changes to save data
        SaveGame ();
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

    public void SetVisitedArea (DataType.IslandSection island, bool isVisited) {
        visitedAreas[island] = isVisited;

        // Save changes to save data
        SaveGame ();
    }

    public int GetNumOfGamesCompleted() {
        return numOfGamesCompleted;
    }

    public bool GetIsMonsterSelected () {
        return isMonsterSelected;
    }

    public void CreateNotification (string text) {
        Instantiate (notificationPrefab).GetComponent<Notification> ().DisplayNotification (text);
    }

    public bool IsFPSDisplayActive () {
        return fpsCounter.activeSelf;
    }

    public void ToggleFPSDisplay () {
        fpsCounter.SetActive (!fpsCounter.activeSelf);
    }

    public void TurnOnFPSDisplay () {
        fpsCounter.SetActive (true);
    }

    public void TurnOffFPSDisplay () {
        fpsCounter.SetActive (false);
    }

    // Only save game if Start Button is pressed in the Start scene
    public void AllowSave () {
        isSaveAllowed = true;
    }

    public void SaveGame () {
        if (isSaveAllowed) {
            Dictionary<string, int> foodDictionary = FoodList.GetFoodDictionary ();

            GameSave save = new GameSave {
                gameStats = gameStats,
                stickerStats = stickerStats,
                visitedAreas = visitedAreas,
                lastGamePlayed = lastGamePlayed,
                isMonsterSelected = isMonsterSelected,
                playerMonsterType = playerMonsterType,
                numOfGamesCompleted = numOfGamesCompleted,
                isIntroShown = isIntroShown,
                foodEatenDictionary = foodDictionary
            };

            SaveSystem.Save (save);
        }
    }

    public void LoadGame () {
        SaveSystem.Load ();
        if (SaveSystem.savedGame != null) {
            InitializeFromLoad (SaveSystem.savedGame);
            CreateNotification ("Game Loaded!");
        }
    }

    public void DeleteSave () {
        SaveSystem.DeleteSave ();
        CreateNotification ("Game Deleted!");
    }
}
