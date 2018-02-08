using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsReviewMonsterManager : MonoBehaviour {

    public List<Sprite> monsterSprites;

    void Awake () {
        Transform[] childList = new Transform[transform.childCount];

        // Retrieve all children
        for (int i = 0; i < transform.childCount; i++) {
            childList[i] = transform.GetChild(i);
        }

        GameObject chosenMonster = childList[Random.Range (0, transform.childCount)].gameObject;
        chosenMonster.GetComponent<EmotionsReviewMonster> ().isMonsterToChoose = true;
    }
}
