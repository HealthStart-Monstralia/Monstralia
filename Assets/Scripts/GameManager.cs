using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private Dictionary<string, int> gameLevels;
	private Dictionary<string, int> gameStars;
	private List<string> brainStickers;
	private static GameManager instance = null;
	private List<string> LagoonReviewGames;
	private string reviewGamePath = "ReviewGames";

	public bool[] LagoonTutorial = new bool[5];
	public bool LagoonFirstSticker = true;
	public bool LagoonReview = false;

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
		gameLevels.Add("MonsterEmotions", 1);
		gameLevels.Add("MonsterSenses", 1);

		gameStars = new Dictionary<string, int>();
		gameStars.Add ("Brainbow", 0);
		gameStars.Add("MemoryMatch", 0);
		gameStars.Add("MonsterEmotions", 0);
		gameStars.Add("MonsterSenses", 0);

		brainStickers = new List<string>();
		LagoonReviewGames = new List<string>();
	}

	public void setMonster(string color) {
		this.monster = color;
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
	public void ActivateSticker(string region, string stickerName) {
		if(region == "BrainstormLagoon") {
			brainStickers.Add(stickerName);
		}
		else {
			print("That region isn't implemented yet");
		}
	}

	public List<string> GetStickers() {
		return brainStickers;
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

}
