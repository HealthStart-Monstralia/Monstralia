using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMonster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;

    private void OnEnable () {
        Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale (gameObject, originalScale, 0.3f).setEaseOutBack ();
    }

    public void SetMonsterChosen() {
		GameManager.Instance.SetPlayerMonsterType(typeOfMonster);
    }
}
