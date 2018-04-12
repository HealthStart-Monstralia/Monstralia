using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem {
    public static GameSave savedGame;

    private static string filePath = Path.Combine (Application.persistentDataPath, "MonstraliaGame.save");

    public static void Save (GameSave save) {
        savedGame = save;

        BinaryFormatter bf = new BinaryFormatter ();
        FileStream file = File.Create (filePath);
        bf.Serialize (file, savedGame);
        file.Close ();
        Debug.Log ("Game Saved to: " + filePath);
    }

    public static void Load () {
        if (File.Exists (filePath)) {
            BinaryFormatter bf = new BinaryFormatter ();
            FileStream file = File.Open (filePath, FileMode.Open);
            savedGame = (GameSave)bf.Deserialize (file);
            file.Close ();
            
            Debug.Log ("Game Loaded from: " + filePath);
        }
    }

    public static void DeleteSave () {
        if (File.Exists (filePath)) {
            try {
                File.Delete (filePath);
                if (!File.Exists (filePath)) {
                    Debug.Log ("Game Deleted at: " + filePath);
                }
            } catch (Exception ex) {
                Debug.LogException (ex);
            }
        }
    }
}
