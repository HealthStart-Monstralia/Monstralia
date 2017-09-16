using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsReviewMonsterManager : MonoBehaviour {

    public List<Sprite> monsterSprites;

    bool haveAMonsterToChoose;

    void Awake () {
        Transform[] childList = new Transform[transform.childCount];

        // Retrieve all children
        for (int i = 0; i < transform.childCount; i++) {
            childList[i] = transform.GetChild(i);
        }

        GameObject chosenMonster = childList[Random.Range (0, transform.childCount)].gameObject;
        chosenMonster.GetComponent<EmotionsReviewMonster> ().isMonsterToChoose = true;
        /*
        if (GameManager.GetInstance()) {
            
        }
        while (haveAMonsterToChoose == false) { // pick random monster to be monster to choose
            foreach (Transform monster in transform) { // traverse monsters
                if (Random.Range(0, 10) == 1) { //1 is arbitrary
                    monster.GetComponent<EmotionsReviewMonster>().isMonsterToChoose = true;
                    haveAMonsterToChoose = true;
                    break;
                }
            }
        }
        */
    }
}
