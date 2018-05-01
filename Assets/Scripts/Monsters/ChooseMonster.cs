using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMonster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;
    public Transform spawnPoint;

    private Vector3 originalScale;
    private int moveTweenID;
    private int scaleTweenID;

    private void OnEnable () {
        StartManager.OnMonsterSelected += OnMonsterSelected;
        StartManager.OnMonsterUnselected += OnMonsterUnselected;
        originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale (gameObject, originalScale, 0.5f).setEaseOutBack ();
    }

    private void OnDisable () {
        StartManager.OnMonsterSelected -= OnMonsterSelected;
        StartManager.OnMonsterUnselected -= OnMonsterUnselected;
    }

    void OnMonsterSelected (DataType.MonsterType monsterType) {
        if (monsterType == typeOfMonster) {
            moveTweenID = LeanTween.move (gameObject, Vector2.zero, 0.35f).setEaseOutBack ().id;
            scaleTweenID = LeanTween.scale (gameObject, originalScale * 1.75f, 0.5f).setEaseOutBack ().id;
        } else {
            scaleTweenID = LeanTween.scale (gameObject, Vector3.zero, 0.3f).id;
        }
        GetComponent<Button> ().enabled = false;
    }

    void OnMonsterUnselected (DataType.MonsterType monsterType) {
        LeanTween.cancel (moveTweenID);
        LeanTween.cancel (scaleTweenID);

        if (monsterType == typeOfMonster) {
        }
        LeanTween.move (gameObject, spawnPoint.transform.position, 0.25f).setEaseOutBack ();
        LeanTween.scale (gameObject, originalScale, 0.3f).setEaseOutBack ();
        GetComponent<Button> ().enabled = true;
    }

    public void SelectMonster() {
        StartManager.Instance.OnSelectedMonster (typeOfMonster);
    }
}
