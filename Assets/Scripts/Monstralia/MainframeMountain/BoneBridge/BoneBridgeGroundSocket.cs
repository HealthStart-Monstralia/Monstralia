using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeGroundSocket : MonoBehaviour {
    public Color normalColor, touchColor, snapColor;
    private SpriteRenderer spr;

    private void Awake () {
        spr = GetComponent<SpriteRenderer> ();
        spr.color = normalColor;
    }

    private void OnEnable () {
        BoneBridgeManager.PhaseChange += OnPhaseChange;
    }

    private void OnDisable () {
        BoneBridgeManager.PhaseChange -= OnPhaseChange;
    }

    void OnPhaseChange (BoneBridgeManager.BridgePhase phase) {
        switch (phase) {
            case BoneBridgeManager.BridgePhase.Start:
                spr.color = normalColor;
                break;
            case BoneBridgeManager.BridgePhase.Building:
                spr.color = normalColor;
                break;
            case BoneBridgeManager.BridgePhase.Falling:
                spr.color = snapColor;
                break;
            case BoneBridgeManager.BridgePhase.Crossing:
                spr.color = snapColor;
                break;
            case BoneBridgeManager.BridgePhase.Finish:
                spr.color = snapColor;
                break;
        }
    }
}
