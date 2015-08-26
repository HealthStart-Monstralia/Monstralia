using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private string monster = "green-monster-1";
	private Text monsterSelected;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		monsterSelected = GameObject.Find ("Text").GetComponent<Text>();
		monsterSelected.text = monster;
	}

	public void setGreenMonster() {
		setMonster("green-monster-1");
	}

	public void setBlueMonster() {
		setMonster("blue-monster");
	}

	void setMonster(string color) {
		this.monster = color;
	}

	string getMonster() {
		return monster;
	}
}
