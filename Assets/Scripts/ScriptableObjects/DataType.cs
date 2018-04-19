// Define globally used enums in here

public static class DataType {
    public enum GameEnd {
        EarnedSticker,
        CompletedLevel,
        FailedLevel
    };

    public enum StickerType {
        None = -1,
        Amygdala = 0,
        Cerebellum = 1,
        Frontal = 2,
        Hippocampus = 3,
        RainbowBrain = 4
    };

    public enum GamePersistentEvents {
        Stickerbook = 0,
        ParentPage = 1,
        FirstStickerEarned = 2
    }

    // Enum name should match scene name so MinigameButton can load the game.
    public enum Minigame {
        // Brainstorm Lagoon
        Brainbow,
        BrainMaze,
        MemoryMatch,
        MonsterEmotions,
        MonsterSenses,

        // Mainframe Mountain
        BoneBridge,
        MonsterBasket,
        BoneBuilder,
        BalanceGame,
        ListeningGame,

        // Pump Paradise
        CatchTheToxins
    };

    public enum IslandSection {
        Monstralia,
        BrainstormLagoon,
        MainframeMountain,
        PumpParadise,
        Muscle,
        Digestive
    };

    public enum Level {
        LevelOne = 1,
        LevelTwo = 2,
        LevelThree = 3
    }

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

    // Referenced by SensesButton.cs
    //enumerates to elements 0, 1, 2, 3, 4
    public enum Senses {
        See,
        Hear,
        Feel,
        Smell,
        Taste,
        NONE
    }

    // Referenced by Food.cs
    public enum Color {
        Red,
        Yellow,
        Green,
        Purple,
        Blue,
        White,
        Orange,
        Other
    };
}
