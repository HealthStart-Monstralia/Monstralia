using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPage : PopupPage {
    public Text monsterNameText;
    public Text gamesPlayedText;
    public CreateMonster monsterCreator;
    public Image monsterBackground;
    public Color blueMonsterColor;
    public Color greenMonsterColor;
    public Color redMonsterColor;
    public Color yellowMonsterColor;

    private Monster monster;

    private new void Start () {
        base.Start ();
        monsterNameText.text = GameManager.Instance.GetMonsterName (GameManager.Instance.GetPlayerMonsterType());
        gamesPlayedText.text = "Games Played: " + GameManager.Instance.GetNumOfGamesCompleted ();
        CreateMonster ();
        DetermineColor ();
    }

    void DetermineColor () {
        switch (GameManager.Instance.GetPlayerMonsterType ()) {
            case DataType.MonsterType.Blue:
                monsterBackground.color = blueMonsterColor;
                break;
            case DataType.MonsterType.Green:
                monsterBackground.color = greenMonsterColor;
                break;
            case DataType.MonsterType.Red:
                monsterBackground.color = redMonsterColor;
                break;
            case DataType.MonsterType.Yellow:
                monsterBackground.color = yellowMonsterColor;
                break;
        }
    }

    void CreateMonster () {
        monster = monsterCreator.SpawnPlayerMonster ();
        monster.transform.SetParent (monsterCreator.transform);
        monster.spriteRenderer.sortingLayerName = "UI";
        monster.spriteRenderer.sortingOrder = 4;
        monster.transform.localScale = Vector3.one * 30f;
        monster.AllowMonsterTickle = true;
        monster.IdleAnimationOn = true;
        monster.spawnAnimation = true;
        monster.transform.localPosition = Vector3.zero;
    }

    new void OnButtonClose () {
        base.OnButtonClose ();
        monster.PlayDespawnAnimation ();
    }
}
