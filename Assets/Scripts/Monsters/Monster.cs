using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    // WARNING: POORLY WRITTEN CODE

    public DataType.MonsterType typeOfMonster;
    public DataType.MonsterEmotions selectedEmotion;
    public bool spawnAnimation;
    public SpriteRenderer spriteRenderer;
    public Collider2D colliderComponent;
    public MonsterAnimations monsterAnimator;
    public bool AllowMonsterTickle {
        get {
            return monsterAnimator.allowMonsterTickle;
        }
        set {
            monsterAnimator.allowMonsterTickle = value;
        }
    }

    public bool IdleAnimationOn {
        get {
            return monsterAnimator.IdleAnimationOn;
        }
        set {
            monsterAnimator.IdleAnimationOn = value;
        }
    }

    void Awake () {
        if (spawnAnimation) monsterAnimator.PlaySpawnAnimation ();
        monsterAnimator.allowMonsterTickle = AllowMonsterTickle;
    }

    public void ChangeEmotions (DataType.MonsterEmotions emotionToChangeTo) {
        monsterAnimator.ChangeEmotions (emotionToChangeTo);
    }

}
