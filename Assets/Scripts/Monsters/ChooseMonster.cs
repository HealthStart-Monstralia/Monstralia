using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMonster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;

    public void SetMonsterChosen() {
		GameManager.Instance.SetPlayerMonsterType(typeOfMonster);
    }
}
