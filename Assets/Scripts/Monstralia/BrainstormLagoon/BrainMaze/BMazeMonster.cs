using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMazeMonster : MonoBehaviour {

	private Animator animComp;
	private DataType.MonsterType monster;

	void Awake() {
		animComp = GetComponentInChildren<Animator> ();
        monster = GameManager.Instance.GetPlayerMonsterType ();
    }

	public void PlaySpawn() {
        animComp.StopPlayback ();
        animComp.Play ("BMaze_Spawn", -1, 0f);
	}

	public void PlayDance() {
        animComp.StopPlayback ();
        animComp.Play ("BMaze_Dance", -1, 0f);
    }
}
