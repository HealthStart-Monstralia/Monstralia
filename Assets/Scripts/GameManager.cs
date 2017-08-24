using UnityEngine;
using UnityEditor;
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

	private Dictionary<DataType.Minigame, int> gameLevels;
    private Dictionary<DataType.Minigame, MinigameData> minigameDictionary;
    private Dictionary<DataType.Minigame, bool> gameTutorials;
    private Dictionary<DataType.Minigame, int> gameStars;
    private Dictionary<DataType.Minigame, DataType.StickerType> gameStickers;
    private Dictionary<DataType.StickerType, bool> stickers;
    private Dictionary<DataType.StickerType, bool> stickersPlaced;

	private static GameManager instance = null;
	private List<string> LagoonReviewGames;
	private string reviewGamePath = "ReviewGames";
	private bool allowInput = true;

	public bool LagoonFirstSticker = true;
	public bool LagoonReview = false;
	public bool PlayLagoonVoiceOver = true; // Prevents voiceover clip from playing when returning from a game inside Brainstorm Lagoon - CT
	public bool PlayIntro = true;
	public Canvas introObject;
	public static MonsterType monsterType;

	string monster;

    public static GameManager GetInstance () {
        return instance;
    }

    void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	}

    void Start () {
        InitializeDictionaryEntries ();
        AddMinigamesToDictionary ();
        LagoonReviewGames = new List<string> ();

        if (PlayIntro) {
            Instantiate (introObject);
        }
    }

    void InitializeDictionaryEntries () {

        // Initialize game dictionaries
        minigameDictionary = new Dictionary<DataType.Minigame, MinigameData> ();
        gameLevels = new Dictionary<DataType.Minigame, int> ();
        gameStars = new Dictionary<DataType.Minigame, int> ();
        gameTutorials = new Dictionary<DataType.Minigame, bool> ();

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
    }

    void AddMinigamesToDictionary() {
        print ("Minigames");
        DirectoryInfo dir = new DirectoryInfo ("Assets/Scripts/Data/Minigames/BrainstormLagoon");
        FileInfo[] info = dir.GetFiles ("*.asset");
        foreach (FileInfo f in info) {
            string fullPath = f.FullName.Replace (@"\", "/");
            string assetPath = "Assets" + fullPath.Replace (Application.dataPath, "");
            MinigameData data = AssetDatabase.LoadAssetAtPath (assetPath, typeof (MinigameData)) as MinigameData;
            minigameDictionary.Add (data.typeOfGame, data);
        }

        foreach (KeyValuePair<DataType.Minigame, MinigameData> entry in minigameDictionary) {
            print (string.Format("Key: {0} Value: {1} Sticker: {2}", entry.Key, entry.Value, entry.Value.sticker));
        }

    }

	public void setMonster(string color) {
		monster = color;
		DetermineMonster ();
	}
	
	public string getMonster() {
		return monster;
	}

	public int GetLevel(DataType.Minigame gameName) {
		return gameLevels[gameName];
	}

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

    public void CompleteTutorial(DataType.Minigame gameName) {
        gameTutorials[gameName] = false;
    }

    public bool GetPendingTutorial (DataType.Minigame gameName) {
        return gameTutorials[gameName];
    }

    public int GetNumStars(DataType.Minigame gameName) {
		return gameStars[gameName];
	}

    public MinigameData GetMinigameData(DataType.Minigame gameName) {
        return minigameDictionary[gameName];
    }

    /*
	public MinigameData.Minigame ChooseReviewGame() {

	}
    */

	// Determines what kind of monster is chosen
	void DetermineMonster() {
		if (instance) {
			if (instance.getMonster ().Contains ("Blue")) {
				monsterType = MonsterType.Blue;
			} else {
				if (instance.getMonster ().Contains ("Green")) {
					monsterType = MonsterType.Green;
				} else {
					if (instance.getMonster ().Contains ("Red")) {
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

	public static MonsterType GetMonsterType() {
		return monsterType;
	}

    /// <summary>
    /// Creates a countdown with a voiceover.
    /// </summary>

    public void Countdown() {
		GetComponent<CreateCountdown>().SpawnCountdown();
	}

    /// <summary>
    /// Retrieves the type of sticker assigned to the game
    /// </summary>
    /// <param name="game">Type of game to unlock sticker for.</param>
    /// <returns>Returns a StickerType from DataType</returns>

    public DataType.StickerType GetAssignedSticker(DataType.Minigame game) {
        return gameStickers[game];
    }

    public bool GetStickerStatus(DataType.Minigame game) {
        return stickers[gameStickers[game]];
    }

    public Dictionary<DataType.StickerType, bool> GetAllStickers () {
        return stickers;
    }

    public Dictionary<DataType.StickerType, bool> GetAllPlacedStickers () {
        return stickersPlaced;
    }

    public void OnStickerPlaced(DataType.StickerType typeOfSticker) {
		stickersPlaced [typeOfSticker] = true;
	}

	public void DebugStickers() {
        foreach (DataType.StickerType sticker in System.Enum.GetValues (typeof (DataType.StickerType))) {
            stickers[sticker] = true;
        }
    }

    /// <summary>
    /// Unlocks a sticker for the corresponding game
    /// </summary>
    /// <param name="game">Type of game to unlock sticker for.</param>

    public void ActivateSticker (DataType.Minigame game) {
        stickers[gameStickers[game]] = true;
    }

    public bool GetIsInputAllowed() {
		return allowInput;
	}

	public void SetIsInputAllowed(bool boolean) {
		allowInput = boolean;
		print ("Input Allowed: " + boolean);
	}



}
