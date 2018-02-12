﻿using UnityEngine;

[CreateAssetMenu (fileName = "MinigameData_.asset", menuName = "Data/Minigame Data")]
public class MinigameData : ScriptableObject {
    public DataType.Minigame typeOfGame;
    public DataType.IslandSection sectionOfIsland;
    public GameObject reviewPrefab;
    public GameObject stickerPrefab;
}
