using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	string monster = "green-monster-1";

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update () {
	}

	public void setMonster(string color) {
		this.monster = color;
	}
	
	public string getMonster() {
		return monster;
	}
}
