/* Created from AmalgamateLabs at http://amalgamatelabs.com/Blog/4/data_persistence */
using System;
using System.Collections.Generic;

[Serializable]
public class GameDetails {
    public List<GameManager.MinigameStats> gameStats;
    public List<GameManager.StickerStats> stickerStats;
    public List<DataType.IslandSection> visitedAreas;
    public DataType.MonsterType monsterType;
    public int numOfGamesPlayed;

    public GameDetails () {
        gameStats = new List<GameManager.MinigameStats> ();
        stickerStats = new List<GameManager.StickerStats> ();
        visitedAreas = new List<DataType.IslandSection> ();
        monsterType = DataType.MonsterType.Blue;
        numOfGamesPlayed = 0;
    }
}
