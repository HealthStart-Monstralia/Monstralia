using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;

    public void SetMonsterChosen() {
		GameManager.GetInstance().SetMonster(typeOfMonster);
    }
}
