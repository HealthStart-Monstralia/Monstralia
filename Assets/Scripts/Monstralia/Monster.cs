using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;
    public bool idleAnimationOn = true;
    public bool allowMonsterTickle;
    public EmotionsGameManager.MonsterEmotions emotions;

    [SerializeField] private AudioClip monsterSfx;
    [SerializeField] private Sprite afraid, disgusted, happy, joyous, mad, sad, thoughtful, worried;
    private Animator animComp;
    private SpriteRenderer sprRenderer;

    void Awake () {
        animComp = GetComponent<Animator> ();
        sprRenderer = GetComponent<SpriteRenderer> ();
        ChangeEmotions (emotions);
    }

    void OnMouseDown () {
        if (allowMonsterTickle) {
            if (!ParentPage.GetInstance ()) {
                animComp.Play ("Giggle", -1, 0f);
            }
        }
    }

    public void PlayGiggle() {
        SoundManager.GetInstance ().PlaySFXClip (monsterSfx);
    }

    public void ChangeEmotions (EmotionsGameManager.MonsterEmotions selectedEmotion) {
        switch (selectedEmotion) {
            case EmotionsGameManager.MonsterEmotions.Afraid:
                sprRenderer.sprite = afraid;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Disgusted:
                sprRenderer.sprite = disgusted;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Happy:
                sprRenderer.sprite = happy;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Joyous:
                sprRenderer.sprite = joyous;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Mad:
                sprRenderer.sprite = mad;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Sad:
                sprRenderer.sprite = sad;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Thoughtful:
                sprRenderer.sprite = thoughtful;
                emotions = selectedEmotion;
                break;
            case EmotionsGameManager.MonsterEmotions.Worried:
                sprRenderer.sprite = worried;
                emotions = selectedEmotion;
                break;
        }
    }
}
