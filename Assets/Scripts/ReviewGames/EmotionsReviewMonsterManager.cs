using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsReviewMonsterManager : MonoBehaviour {


    bool haveAMonsterToChoose;
    public List<Sprite> monsterSprites;
	void Start () {
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
	}
}
