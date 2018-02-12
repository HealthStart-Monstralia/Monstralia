﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInMenu : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * THIS IS FOR THE MONSTER IN THE MENUS, NOT IN THE GAMES
	 */

	public DataType.MonsterType typeOfMonster;
	public Sprite[] monsterSprites = new Sprite[4];
	public bool idleAnimationOn = true;
	public AudioClip monsterSfx;

	private Animator animComp;

	void Awake () {
		typeOfMonster = GameManager.Instance.GetPlayerMonsterType ();
		animComp = GetComponent<Animator> ();

	}

    private void OnEnable () {
        ChangeMonsterSprite (monsterSprites[(int)typeOfMonster]);
        PlayIdle ();
        if (idleAnimationOn)
            StartCoroutine (RandomAnimation ());
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void OnMouseDown() {
		if (!ParentPage.Instance) {
			PlayTouch ();
			SoundManager.Instance.PlaySFXClip(monsterSfx);
		}
	}

	IEnumerator RandomAnimation() {
		while (idleAnimationOn) {
			int selection = Random.Range (1, 4);
			yield return StartCoroutine (PlayAnimation (selection));
		}
	}

	IEnumerator PlayAnimation (int animToPlay) {
		switch (animToPlay) {
		case 1:
			PlayIdle ();
			yield return new WaitForSeconds (3f); 
			break;
		case 2:
			PlayIdle2 ();
			yield return new WaitForSeconds (3f); 
			break;
		case 3:
			PlayIdle3 ();
			yield return new WaitForSeconds (3f); 
			break;
		default:
			PlayIdle ();
			yield return new WaitForSeconds (3f); 
			break;
		}
	}

	public void ChangeMonsterSprite(Sprite spr) {
		GetComponent<SpriteRenderer> ().sprite = spr;
	}
		
	public void PlayIdle() {
		switch (typeOfMonster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BlueBaby_Idle");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("GreenBaby_Idle");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("RedBaby_Idle");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("YellowBaby_Idle");
			break;
		}
	}

	public void PlayIdle2() {
		switch (typeOfMonster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BlueBaby_Idle2");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("GreenBaby_Idle2");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("RedBaby_Idle2");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("YellowBaby_Idle2");
			break;
		}
	}

	public void PlayIdle3() {
		switch (typeOfMonster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BlueBaby_Idle3");
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("GreenBaby_Idle3");
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("RedBaby_Idle3");
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("YellowBaby_Idle3");
			break;
		}
	}

	public void PlayTouch() {
		switch (typeOfMonster) {
		case DataType.MonsterType.Blue:
			animComp.Play ("BlueBaby_Touch", -1, 0f);
			break;
		case DataType.MonsterType.Green:
			animComp.Play ("GreenBaby_Touch", -1, 0f);
			break;
		case DataType.MonsterType.Red:
			animComp.Play ("RedBaby_Touch", -1, 0f);
			break;
		case DataType.MonsterType.Yellow:
			animComp.Play ("YellowBaby_Touch", -1, 0f);
			break;
		}
	}
}
