using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPage : PopupPage {
    public Text monsterNameText;
    public CreateMonster monsterCreator;
    private Monster monster;

    private void Start () {
        monsterNameText.text = GameManager.Instance.GetMonsterName (GameManager.Instance.GetPlayerMonsterType());
        CreateMonster ();
    }

    void CreateMonster () {
        monster = monsterCreator.SpawnPlayerMonster ();
        monster.transform.SetParent (monsterCreator.transform);
        monster.spriteRenderer.sortingLayerName = "UI";
        monster.spriteRenderer.sortingOrder = 4;
        monster.transform.localScale = Vector3.one * 40f;
        monster.AllowMonsterTickle = true;
        monster.IdleAnimationOn = true;
        monster.spawnAnimation = true;
    }

    new void OnButtonClose () {
        base.OnButtonClose ();
        monster.PlayDespawnAnimation ();
    }
}
