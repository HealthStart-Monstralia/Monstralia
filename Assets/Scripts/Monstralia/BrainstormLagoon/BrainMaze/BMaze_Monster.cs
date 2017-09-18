using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Monster : MonoBehaviour {

	private Animator animComp;
	private DataType.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
	}

	public void PlaySpawn() {
		monster = GameManager.GetInstance().GetMonster();
		switch (monster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BMaze_BlueBabySpawn", -1, 0f);
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("BMaze_GreenBabySpawn", -1, 0f);
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("BMaze_RedBabySpawn", -1, 0f);
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("BMaze_YellowBabySpawn", -1, 0f);
			break;
		}
	}

	public void PlayDance() {
        monster = GameManager.GetInstance ().GetMonster ();
        switch (monster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BMaze_BlueBabyDance");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("BMaze_GreenBabyDance");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("BMaze_RedBabyDance");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("BMaze_YellowBabyDance");
			break;
		}
	}
}
