using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType : ScriptableObject {
    public enum StickerType {
        Amygdala = 0,
        Cerebellum = 1,
        Frontal = 2,
        Hippocampus = 3,
        RainbowBrain = 4
    };

    public enum Minigame {
        Brainbow,
        BrainMaze,
        MemoryMatch,
        MonsterEmotions,
        MonsterSenses
    };

    public enum IslandSection {
        Monstralia,
        BrainstormLagoon,
        MainframeMountain
    };

    public enum MonsterType {
        Blue = 0,
        Green = 1,
        Red = 2,
        Yellow = 3
    };
}
