using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created with online tutorial at http://catlikecoding.com/unity/tutorials/maze/ */
public enum MazeDirection {
    North,
    West,
    South,
    East
}

public static class MazeDirections {
    public const int Count = 4;

    private static MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.East,
        MazeDirection.North,
        MazeDirection.West
    };

    public static MazeDirection RandomValue {
        get {
            return (MazeDirection)Random.Range (0, Count);
        }
    }

    // Extension method
    public static IntVector2 ToIntVector2 (this MazeDirection direction) {
        return vectors[(int)direction];
    }

    private static IntVector2[] vectors = {
        new IntVector2(0, 1),
        new IntVector2(-1, 0),
        new IntVector2(0, -1),
        new IntVector2(1, 0)
    };

    public static MazeDirection GetOpposite (this MazeDirection direction) {
        return opposites[(int)direction];
    }

    public static Quaternion ToRotation (this MazeDirection direction) {
        return rotations[(int)direction];
    }

    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(0f, 0f, 180f),
        Quaternion.Euler(0f, 0f, 270f)
    };
}
