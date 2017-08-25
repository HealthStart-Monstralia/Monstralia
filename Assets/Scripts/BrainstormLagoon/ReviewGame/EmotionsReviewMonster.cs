using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionsReviewMonster : MonoBehaviour {

    Image myImage;
    int spriteIndex;
    public Text text;
    EmotionsReviewMonsterManager parent;
    public bool isMonsterToChoose;

    void Start() {
        myImage = GetComponent<Image>();
        parent = FindObjectOfType<EmotionsReviewMonsterManager>();
        spriteIndex = Random.Range(0, parent.monsterSprites.Count);
        myImage.sprite = parent.monsterSprites[spriteIndex];
        parent.monsterSprites.RemoveAt(spriteIndex); // so emotions wont repeat

        if (isMonsterToChoose == true) {
            text.text = "Pick the " + myImage.sprite.name + " monster!";
            print("should be adding button");
            AddButton();
        }
    }
    void AddButton() {
        print("adding button");
        Button mybutton = gameObject.AddComponent<Button>();
        mybutton.onClick.AddListener(Win);
        print(mybutton);
    }
    void Win() {
        ReviewGameWinLose.GetInstance().WinCondition();
        Destroy(FindObjectOfType<ReviewGameWinLose>().gameObject);
    }
}
