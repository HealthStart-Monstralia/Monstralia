using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public enum MonsterType {
		Blue = 0, 
		Green = 1, 
		Red = 2, 
		Yellow = 3
	};

	private Dictionary<MinigameData.Minigame, int> gameLevels;
    private Dictionary<MinigameData.Minigame, bool> gameTutorials;
    private Dictionary<MinigameData.Minigame, int> gameStars;
    private Dictionary<MinigameData.Minigame, StickerData.StickerType> gameStickers;
    private Dictionary<StickerData.StickerType, bool> stickers;
    private Dictionary<StickerData.StickerType, bool> stickersPlaced;

	private static GameManager instance = null;
	private List<string> LagoonReviewGames;
	private string reviewGamePath = "ReviewGames";
	private bool allowInput = true;

	public bool[] LagoonTutorial = new bool[5];
	public bool LagoonFirstSticker = true;
	public bool LagoonReview = false;
	public bool PlayLagoonVoiceOver = true; // Prevents voiceover clip from playing when returning from a game inside Brainstorm Lagoon - CT
	public bool PlayIntro = true;
	public Canvas introObject;
	public static MonsterType monsterType;

	string monster;

	void Awake() {
		for(int i = 0; i < 5; ++i) {
			LagoonTutorial[i] = true;
		}

		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	}

	public static GameManager GetInstance() {
		return instance;
	}

	void Start() {
        InitializeDictionaryEntries ();
        LagoonReviewGames = new List<string>();

		if (PlayIntro) {
			Instantiate (introObject);
		}
	}

	public void setMonster(string color) {
		this.monster = color;
		DetermineMonster ();
	}
	
	public string getMonster() {
		return monster;
	}

	public int GetLevel(MinigameData.Minigame gameName) {
		return gameLevels[gameName];
	}

	public bool LevelUp(MinigameData.Minigame gameName) {
		if(gameStars[gameName] < 3) {
			gameStars[gameName] += 1;
            if (gameStars[gameName] == 1 || gameStars[gameName] == 3) {
                switch (gameName) {

                    case MinigameData.Minigame.Brainbow:
                        ReviewManager.GetInstance ().levelToReview = "BrainbowReviewGame";
                        break;
                    case MinigameData.Minigame.BrainMaze:
                        ReviewManager.GetInstance ().levelToReview = "BrainmazeReviewGame";
                        break;
                    case MinigameData.Minigame.MemoryMatch:
                        ReviewManager.GetInstance ().levelToReview = "MemoryMatchReviewGame";
                        break;
                    case MinigameData.Minigame.MonsterEmotions:
                        ReviewManager.GetInstance ().levelToReview = "EmotionsReviewGame";
                        break;
                    case MinigameData.Minigame.MonsterSenses:
                        ReviewManager.GetInstance ().levelToReview = "SensesReviewGame";
                        break;

                }
            }

            if (gameLevels[gameName] < 3){
				gameLevels[gameName] += 1;
			}
			return true;
		}
		return false;
	}

    public void CompleteTutorial(MinigameData.Minigame gameName) {
        gameTutorials[gameName] = false;
    }

    public bool GetPendingTutorial (MinigameData.Minigame gameName) {
        return gameTutorials[gameName];
    }

    public int GetNumStars(MinigameData.Minigame gameName) {
		return gameStars[gameName];
	}

	public void ActivateBrainstormLagoonReview() {
		Debug.Log ("Activating Brainstorm Lagoon review");
		if(!LagoonReview) {
			Debug.Log ("LagoonReview: " + LagoonReview);
			LagoonReview = true;
			Debug.Log ("LagoonReview: " + LagoonReview);
		}
	}

	public void AddLagoonReviewGame(string gameName) {
		Debug.Log ("Adding lagoon review game " + gameName);
		LagoonReviewGames.Add (gameName);
		Debug.Log ("COUNT OF REVIEW GAMES ACTIVE: " + LagoonReviewGames.Count);
	}

	public Canvas ChooseLagoonReviewGame() {
		Debug.Log("Review Game: " + LagoonReviewGames[Random.Range (0, LagoonReviewGames.Count)]);
		Canvas reviewGame = (Canvas)Instantiate(Resources.Load("MemoryMatchReviewGame"));//"ReviewGames/LagoonReveiwGames/" + LagoonReviewGames[Random.Range (0, LagoonReviewGames.Count)]));
		Debug.Log("ReivewGameName: " + reviewGame.name);
		return reviewGame;//Resources.Load(reviewGamePath  + "/" + LagoonReviewGames[Random.Range (0, LagoonReviewGames.Count)]) as Canvas;
	}

	// Determines what kind of monster is chosen
	void DetermineMonster() {
		if (GameManager.GetInstance ()) {
			if (GameManager.GetInstance ().getMonster ().Contains ("Blue")) {
				monsterType = MonsterType.Blue;
			} else {
				if (GameManager.GetInstance ().getMonster ().Contains ("Green")) {
					monsterType = MonsterType.Green;
				} else {
					if (GameManager.GetInstance ().getMonster ().Contains ("Red")) {
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

	public void Countdown() {
		GetComponent<CreateCountdown>().SpawnCountdown();
	}

    public StickerData.StickerType GetAssignedSticker(MinigameData.Minigame game) {
        return gameStickers[game];
    }

    public bool GetStickerStatus(MinigameData.Minigame game) {
        return stickers[gameStickers[game]];
    }

    public Dictionary<StickerData.StickerType, bool> GetAllStickers () {
        return stickers;
    }

    public Dictionary<StickerData.StickerType, bool> GetAllPlacedStickers () {
        return stickersPlaced;
    }

    public void OnStickerPlaced(StickerData.StickerType typeOfSticker) {
		stickersPlaced [typeOfSticker] = true;
	}

	public void DebugStickers() {
        foreach (StickerData.StickerType sticker in System.Enum.GetValues (typeof (StickerData.StickerType))) {
            stickers[sticker] = true;
        }
    }

    public void ActivateSticker (MinigameData.Minigame game) {
        stickers[gameStickers[game]] = true;
    }

    public bool GetIsInputAllowed() {
		return allowInput;
	}

	public void SetIsInputAllowed(bool boolean) {
		allowInput = boolean;
		print ("Input Allowed: " + boolean);
	}

    void InitializeDictionaryEntries() {
        // Initialize dictionaries
        gameLevels = new Dictionary<MinigameData.Minigame, int> ();
        gameStars = new Dictionary<MinigameData.Minigame, int> ();
        gameTutorials = new Dictionary<MinigameData.Minigame, bool> ();

        // Loop through each minigame enum to initialize the dictionary values to avoid typing all that damn stuff out
        foreach (MinigameData.Minigame game in System.Enum.GetValues (typeof (MinigameData.Minigame))) {
            gameLevels.Add (game, 1);
            gameStars.Add (game, 0);
            gameTutorials.Add (game, true);
        }

        // Associate each sticker with a certain minigame
        gameStickers = new Dictionary<MinigameData.Minigame, StickerData.StickerType> ();
        gameStickers.Add (MinigameData.Minigame.Brainbow, StickerData.StickerType.RainbowBrain);
        gameStickers.Add (MinigameData.Minigame.BrainMaze, StickerData.StickerType.Frontal);
        gameStickers.Add (MinigameData.Minigame.MemoryMatch, StickerData.StickerType.Hippocampus);
        gameStickers.Add (MinigameData.Minigame.MonsterEmotions, StickerData.StickerType.Amygdala);
        gameStickers.Add (MinigameData.Minigame.MonsterSenses, StickerData.StickerType.Cerebellum);

        // Initialize dictionaries
        stickers = new Dictionary<StickerData.StickerType, bool> ();
        stickersPlaced = new Dictionary<StickerData.StickerType, bool> ();

        // Loop through each sticker enum to initialize the dictionary values to avoid typing more damn stuff
        foreach (StickerData.StickerType sticker in System.Enum.GetValues (typeof (StickerData.StickerType))) {
            stickers.Add (sticker, false);
            stickersPlaced.Add (sticker, false);
        }
    }

}
