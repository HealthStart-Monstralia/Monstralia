using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    // WARNING: POORLY WRITTEN CODE

    public DataType.MonsterType typeOfMonster;
    public DataType.MonsterEmotions selectedEmotion;
    public bool allowMonsterTickle, spawnAnimation;
    public SpriteRenderer spriteRenderer;
    public Collider2D colliderComponent;
    public MonsterAnimations monsterAnimator;
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
    }

    public void ChangeEmotions (DataType.MonsterEmotions emotionToChangeTo) {
        monsterAnimator.ChangeEmotions (emotionToChangeTo);
    }

    void OnMouseDown () {
        if (allowMonsterTickle) {
            if (!ParentPage.Instance) {
                monsterAnimator.animator.Play ("Giggle", -1, 0f);
            }
        }
    }

}
