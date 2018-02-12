using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "BoneBridgeLevel_.asset", menuName = "Data/BoneBridge Level")]
public class BoneBridgeData : ScriptableObject {
    public DataType.MonsterType[] savedMonsters;
}
