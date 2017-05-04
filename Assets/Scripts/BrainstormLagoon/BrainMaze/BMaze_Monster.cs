using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Monster : MonoBehaviour {

	private Animator animComp;
	private BMaze_Manager.MonsterType monster;

	void Start() {
		animComp = GetComponentInChildren<Animator> ();
	}

	public void PlayDance() {
		monster = BMaze_Manager.GetInstance ().typeOfMonster;
		switch (monster) {
		case BMaze_Manager.MonsterType.Blue:
			animComp.Play ("BMaze_BlueBabyDance");
			break;
		case BMaze_Manager.MonsterType.Green:
			animComp.Play ("BMaze_GreenBabyDance");
			break;
		case BMaze_Manager.MonsterType.Red:
			animComp.Play ("BMaze_RedBabyDance");
			break;
		case BMaze_Manager.MonsterType.Yellow:
			animComp.Play ("BMaze_YellowBabyDance");
			break;
		}
	}
}
