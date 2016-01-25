using UnityEngine;
using System.Collections;

public class SaveData : MonoBehaviour {

	// Use this for initialization
	public void setMonster(string color) {
		GameManager.GetInstance().setMonster(color);
	}
}
