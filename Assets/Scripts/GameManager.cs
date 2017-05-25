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

	private Dictionary<string, int> gameLevels;
	private Dictionary<string, int> gameStars;
	private Dictionary<StickerManager.StickerType, bool> stickers;
	private Dictionary<StickerManager.StickerType, bool> stickersPlaced;

	private static GameManager instance = null;
	private List<string> LagoonReviewGames;
	private string reviewGamePath = "ReviewGames";

	public bool[] LagoonTutorial = new bool[5];
	public bool LagoonFirstSticker = true;
	public bool LagoonReview = false;
	public bool PlayLagoonVoiceOver = true; // Prevents voiceover clip from playing when returning from a game inside Brainstorm Lagoon - CT
	public static MonsterType monsterType;

//	public List<Canvas> LagoonReviewCanvases;
//	public List<Canvas> LagoonReviewGames;

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
		gameLevels = new Dictionary<string, int>();
		gameLevels.Add("Brainbow", 1);
		gameLevels.Add("MemoryMatch", 1);
		gameLevels.Add("BrainMaze", 1);
		gameLevels.Add("MonsterEmotions", 1);
		gameLevels.Add("MonsterSenses", 1);

		gameStars = new Dictionary<string, int>();
		gameStars.Add ("Brainbow", 0);
		gameStars.Add("MemoryMatch", 0);
		gameStars.Add("BrainMaze", 0);
		gameStars.Add("MonsterEmotions", 0);
		gameStars.Add("MonsterSenses", 0);

		stickers = new Dictionary<StickerManager.StickerType, bool> ();
		stickers.Add (StickerManager.StickerType.Amygdala, false);
		stickers.Add (StickerManager.StickerType.Cerebellum, false);
		stickers.Add (StickerManager.StickerType.Frontal, false);
		stickers.Add (StickerManager.StickerType.Hippocampus, false);
		stickers.Add (StickerManager.StickerType.RainbowBrain, false);

		stickersPlaced = new Dictionary<StickerManager.StickerType, bool> ();
		stickersPlaced.Add (StickerManager.StickerType.Amygdala, false);
		stickersPlaced.Add (StickerManager.StickerType.Cerebellum, false);
		stickersPlaced.Add (StickerManager.StickerType.Frontal, false);
		stickersPlaced.Add (StickerManager.StickerType.Hippocampus, false);
		stickersPlaced.Add (StickerManager.StickerType.RainbowBrain, false);

		LagoonReviewGames = new List<string>();
	}

	public void setMonster(string color) {
		this.monster = color;
		DetermineMonster ();
	}
	
	public string getMonster() {
		return monster;
	}

	public int GetLevel(string gameName) {
		return gameLevels[gameName];
	}

	public bool LevelUp(string gameName) {
		if(gameStars[gameName] <= 3) {
			gameStars[gameName] += 1;
			if(gameLevels[gameName] != 5){
				gameLevels[gameName] += 1;
			}
			return true;
		}
		return false;
	}

	public int GetNumStars(string gameName) {
		return gameStars[gameName];
	}

	//temporary solution
	public void ActivateSticker(StickerManager.StickerType typeOfSticker) {
		stickers [typeOfSticker] = true;
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

	// Determines what kind of monster is chosen\
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

	public void FetchStickers() {
		foreach (StickerManager.StickerType sticker in stickers.Keys) {
			if (stickers [sticker])
				StickerManager.GetInstance ().SpawnSticker (sticker, stickersPlaced[sticker]);
		}
	}

	public void OnStickerPlaced(StickerManager.StickerType typeOfSticker) {
		stickersPlaced [typeOfSticker] = true;
	}

	public void DebugStickers() {
		stickers [StickerManager.StickerType.Amygdala] = true;
		stickers [StickerManager.StickerType.Cerebellum] = true;
		stickers [StickerManager.StickerType.Frontal] = true;
		stickers [StickerManager.StickerType.Hippocampus] = true;
		stickers [StickerManager.StickerType.RainbowBrain] = true;
	}

}
