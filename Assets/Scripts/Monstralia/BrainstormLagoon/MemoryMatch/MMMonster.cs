using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMMonster : MonoBehaviour {

	private Animator animComp;

	public DataType.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
		monster = GameManager.GetInstance().GetMonsterType();
	}

	public void PlaySpawn() {
		switch (monster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("MM_BlueBabySpawn");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("MM_GreenBabySpawn");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("MM_RedBabySpawn");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabySpawn");
			break;
		}
	}

	public void PlayDance() {
		switch (monster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("MM_BlueBabyDance");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("MM_GreenBabyDance");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("MM_RedBabyDance");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabyDance");
			break;
		}
	}

	public void PlayEat() {
		switch (monster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("MM_BlueBabyEat", -1, 0f);
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("MM_GreenBabyEat", -1, 0f);
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("MM_RedBabyEat", -1, 0f);
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("MM_YellowBabyEat", -1, 0f);
			break;
		}
	}
}
