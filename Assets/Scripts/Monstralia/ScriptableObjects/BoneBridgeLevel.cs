using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "BoneBridgeLevel_.asset", menuName = "Data/BoneBridge Level")]
public class BoneBridgeLevel : ScriptableObject {
    public DataType.MonsterType[] savedMonsters;
    public GameObject start, goal, chest;
    public Transform[] friendSpawns;
}
