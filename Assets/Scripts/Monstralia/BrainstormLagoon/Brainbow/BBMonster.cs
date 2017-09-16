using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBMonster : MonoBehaviour {
	
	private Animator animComp;

	public GameManager.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
		monster = GameManager.GetMonsterType();
	}

	public void PlaySpawn() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("BB_BlueBabySpawn");
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("BB_GreenBabySpawn");
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("BB_RedBabySpawn");
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("BB_YellowBabySpawn");
			break;
		}
	}

	/*
	public void PlayDance() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("BB_BlueBabyDance");
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("BB_GreenBabyDance");
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("BB_RedBabyDance");
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("BB_YellowBabyDance");
			break;
		}
	}
	*/

	public void PlayEat() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("BB_BlueBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("BB_GreenBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("BB_RedBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("BB_YellowBabyEat", -1, 0f);
			break;
		}
	}
}
