using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Monster : MonoBehaviour {

	private Animator animComp;
	private GameManager.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
	}

	public void PlaySpawn() {
		monster = GameManager.GetMonsterType();
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("BMaze_BlueBabySpawn", -1, 0f);
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("BMaze_GreenBabySpawn", -1, 0f);
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("BMaze_RedBabySpawn", -1, 0f);
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("BMaze_YellowBabySpawn", -1, 0f);
			break;
		}
	}

	public void PlayDance() {
		monster = GameManager.GetMonsterType();
		switch (monster) {
		case GameManager.MonsterType.Blue:
			animComp.Play ("BMaze_BlueBabyDance");
			break;
		case GameManager.MonsterType.Green:
			animComp.Play ("BMaze_GreenBabyDance");
			break;
		case GameManager.MonsterType.Red:
			animComp.Play ("BMaze_RedBabyDance");
			break;
		case GameManager.MonsterType.Yellow:
			animComp.Play ("BMaze_YellowBabyDance");
			break;
		}
	}
}
