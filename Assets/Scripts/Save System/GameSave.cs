/* Created from AmalgamateLabs at http://amalgamatelabs.com/Blog/4/data_persistence */
using System;
using System.Collections.Generic;

[Serializable]
public class GameSave {
    public Dictionary<DataType.Minigame, GameManager.MinigameStats> gameStats;
    public Dictionary<DataType.StickerType, GameManager.StickerStats> stickerStats;
    public Dictionary<DataType.IslandSection, bool> visitedAreas;
    public Dictionary<string, int> foodEatenDictionary = new Dictionary<string, int> ();
    public DataType.Minigame lastGamePlayed;
    public DataType.MonsterType playerMonsterType;
    public bool isMonsterSelected;
    public bool isIntroShown;
    public bool hasPlayerVisitedStickerbook;
    public int numOfGamesCompleted;
}
