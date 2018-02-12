using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    public GameObject panelPrefab;
    public Transform panel;
    private MinigameData[] minigameAssetData;

    private void Awake () {
        minigameAssetData = Resources.LoadAll<MinigameData> ("Data/Minigames");
        foreach (MinigameData data in minigameAssetData) {
            DebugEntry entry = Instantiate (panelPrefab, panel).GetComponent<DebugEntry> ();
            entry.gameName = data.typeOfGame;
        }
    }
}
