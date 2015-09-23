using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private Dictionary<string, int> gameLevels;

	public static GameManager instance = null;

	string monster;

	void Awake() {
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
		print("current level for " + gameName + " is: " + gameLevels[gameName]);
		if(gameLevels[gameName] < 3) {
			print("leveling up: " + gameName + " to level" + (gameLevels[gameName] + 1));
			gameLevels[gameName] += 1;
			return true;
		}
		return false;
	}
}
