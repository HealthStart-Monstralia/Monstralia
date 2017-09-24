using UnityEngine;
using System.Collections;

public class SaveSystem : MonoBehaviour {
	public void SetMonster (DataType.MonsterType monster) {
		GameManager.GetInstance().SetMonsterType(monster);
	}
}
