using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public enum MonsterType {
		Blue = 0, 
		Green = 1, 
		Red = 2, 
		Yellow = 3
	};

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

    private Dictionary<DataType.Minigame, MinigameStats> gameStats;
    private Dictionary<DataType.StickerType, StickerStats> stickerStats;

    private Dictionary<DataType.Minigame, int> gameLevels;
    private Dictionary<DataType.Minigame, MinigameData> minigameDictionary;
    private Dictionary<DataType.Minigame, bool> gameTutorials;
    private Dictionary<DataType.Minigame, int> gameStars;
    //private Dictionary<DataType.Minigame, DataType.StickerType> gameStickers;
    //private Dictionary<DataType.StickerType, bool> stickers;
    //private Dictionary<DataType.StickerType, bool> stickersPlaced;

	private static GameManager instance = null;
	private bool allowInput = true;
    private string monster;
    [SerializeField] private MinigameData[] minigameAssetData;
    [SerializeField] private GameObject[] stickerObjects;

    public bool firstTimeInMainMap, firstTimeInBrainstormLagoon;
	public bool lagoonFirstSticker = true;
	public bool playLagoonVoiceOver = true; // Prevents voiceover clip from playing when returning from a game inside Brainstorm Lagoon - CT
	public bool playIntro = true;
	public Canvas introObject;
	public static MonsterType monsterType;

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

        /*
        gameLevels = new Dictionary<DataType.Minigame, int> ();
        gameStars = new Dictionary<DataType.Minigame, int> ();
        gameTutorials = new Dictionary<DataType.Minigame, bool> ();
        */

        /*
        // Loop through each minigame enum to initialize the dictionary values to avoid typing all that damn stuff out
        foreach (DataType.Minigame game in System.Enum.GetValues (typeof (DataType.Minigame))) {
            gameLevels.Add (game, 1);
            gameStars.Add (game, 0);
            gameTutorials.Add (game, true);
        }

        // Associate each sticker with a certain minigame
        gameStickers = new Dictionary<DataType.Minigame, DataType.StickerType> ();
        gameStickers.Add (DataType.Minigame.Brainbow, DataType.StickerType.RainbowBrain);
        gameStickers.Add (DataType.Minigame.BrainMaze, DataType.StickerType.Frontal);
        gameStickers.Add (DataType.Minigame.MemoryMatch, DataType.StickerType.Hippocampus);
        gameStickers.Add (DataType.Minigame.MonsterEmotions, DataType.StickerType.Amygdala);
        gameStickers.Add (DataType.Minigame.MonsterSenses, DataType.StickerType.Cerebellum);

        // Initialize sticker dictionaries
        stickers = new Dictionary<DataType.StickerType, bool> ();
        stickersPlaced = new Dictionary<DataType.StickerType, bool> ();

        // Loop through each sticker enum to initialize the dictionary values to avoid typing more damn stuff
        foreach (DataType.StickerType sticker in System.Enum.GetValues (typeof (DataType.StickerType))) {
            stickers.Add (sticker, false);
            stickersPlaced.Add (sticker, false);
        }
        */
    }

    public void SetMonster(string color) {
		monster = color;
		DetermineMonster ();
	}
	
	public string GetMonster() {
		return monster;
	}

    /*
	public bool LevelUp(DataType.Minigame gameName) {
		if(gameStars[gameName] < 3) {
            gameStars[gameName] += 1;

            if (gameStars[gameName] == 1 || gameStars[gameName] == 3) {
                ReviewManager.GetInstance ().AddReviewGameToList (gameName);
            }

            if (gameLevels[gameName] < 3){
                gameLevels[gameName] += 1;
			}

			return true;
		}
		return false;
	}
    */

    public void LevelUp (DataType.Minigame gameName) {
        MinigameStats newStats = gameStats[gameName]; // Copy current struct to a new one

        if (newStats.stars < 3) {
            newStats.stars += 1;
        }

        if (newStats.stars == 1 || newStats.stars == 3) {
            ReviewManager.GetInstance ().AddReviewGameToList (gameName);
        }

        if (newStats.level < 3) {
            newStats.level += 1;
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

    /*
    public Dictionary<DataType.StickerType, bool> GetAllPlacedStickers () {
        return stickersPlaced;
    }

    public Dictionary<DataType.StickerType, bool> GetAllStickers () {
        return stickers;
    }

    public Dictionary<DataType.StickerType, bool> GetAllPlacedStickers () {
        return stickersPlaced;
    }
    */

    public static MonsterType GetMonsterType () {
        return monsterType;
    }

    /// <summary>
    /// Determines what kind of monster is chosen
    /// </summary>

    void DetermineMonster () {
		if (instance) {
			if (instance.GetMonster ().Contains ("Blue")) {
				monsterType = MonsterType.Blue;
			} else {
				if (instance.GetMonster ().Contains ("Green")) {
					monsterType = MonsterType.Green;
				} else {
					if (instance.GetMonster ().Contains ("Red")) {
						monsterType = MonsterType.Red;
					} else {
						monsterType = MonsterType.Yellow;
					}
				}
			}
		} else {
			monsterType = MonsterType.Green;
		}
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
}
