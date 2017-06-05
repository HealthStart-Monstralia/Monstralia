using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMMonster : MonoBehaviour {

	private Animator animComp;

	public GameManager.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
		monster = GameManager.GetMonsterType();
	}

	public void PlaySpawn() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("MM_BlueBabySpawn");
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("MM_GreenBabySpawn");
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("MM_RedBabySpawn");
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabySpawn");
			break;
		}
	}

	public void PlayDance() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("MM_BlueBabyDance");
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("MM_GreenBabyDance");
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("MM_RedBabyDance");
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabyDance");
			break;
		}
	}

	public void PlayEat() {
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("MM_BlueBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("MM_GreenBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("MM_RedBabyEat", -1, 0f);
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabyEat", -1, 0f);
			break;
		}
	}
}
