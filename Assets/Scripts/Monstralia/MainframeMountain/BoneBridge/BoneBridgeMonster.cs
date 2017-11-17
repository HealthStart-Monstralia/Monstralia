using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeMonster : MonoBehaviour {
    [HideInInspector] public Rigidbody2D rigBody;

    private BoxCollider2D col;
    private Vector3 pointerOffset;
    private Vector3 cursorPos;
    private Monster monster;

    private void OnEnable () {
        BoneBridgeManager.PhaseChange += OnPhaseChange;
    }

    private void OnDisable () {
        BoneBridgeManager.PhaseChange -= OnPhaseChange;
    }

    private void Awake () {
        transform.SetParent (transform.root.parent);
        monster = GetComponentInChildren<Monster> ();
    }

    public void OnMouseDown () {
        if (BoneBridgeManager.GetInstance ().inputAllowed) {
            if (BoneBridgeManager.GetInstance ().bridgePhase != BoneBridgeManager.BridgePhase.Crossing) {
                StopAllCoroutines ();
                BoneBridgeManager.GetInstance ().ChangePhase (BoneBridgeManager.BridgePhase.Crossing);
            }
        }
    }

    void OnPhaseChange(BoneBridgeManager.BridgePhase phase) {
        switch (phase) {
            case BoneBridgeManager.BridgePhase.Start:
                
                break;
            case BoneBridgeManager.BridgePhase.Building:
                StopAllCoroutines ();
                break;
            case BoneBridgeManager.BridgePhase.Crossing:
                BoneBridgeManager.GetInstance ().CameraSwitch (gameObject);
                StartCoroutine (Move ());
                break;
            case BoneBridgeManager.BridgePhase.Falling:
                StopAllCoroutines ();
                monster.ChangeEmotions (DataType.MonsterEmotions.Afraid);
                break;
            case BoneBridgeManager.BridgePhase.Finish:
                StopAllCoroutines ();
                break;
        }
    }

    public IEnumerator Move () {
        Vector2 dir = Vector2.right;
        while (true) {
            MoveTowards (dir);
            yield return null;
        }
    }

    public void MoveTowards (Vector2 pos) {
        rigBody.AddForce (pos * 0.25f, ForceMode2D.Impulse);
    }
}
