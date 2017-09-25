﻿using System.Collections;
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

    // Enum name should match scene name so MinigameButton can load the game.
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

    // Emotions referenced by Monster.cs, EmotionsGenerator.cs, EmotionsCardHand.cs
    public enum MonsterEmotions {
        Happy = 0,
        Afraid = 1,
        Disgusted = 2,
        Joyous = 3,
        Mad = 4,
        Sad = 5,
        Thoughtful = 6,
        Worried = 7
    };
}
