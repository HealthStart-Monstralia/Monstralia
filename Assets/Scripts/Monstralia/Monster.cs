using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public DataType.MonsterType typeOfMonster;
    public DataType.MonsterEmotions selectedEmotion;
    public EmotionData emotionData;
    public bool allowMonsterTickle;

    public bool idleAnimationOn {
        get { return _idleAnimationOn; }
        set {
            _idleAnimationOn = value;
            if (_idleAnimationOn) {
                StartCoroutine (PlayIdleAnimation ());
            }
        }
    }

    [SerializeField] private bool _idleAnimationOn;
    [SerializeField] private AudioClip monsterSfx;
    private Animator animComp;
    private SpriteRenderer sprRenderer;
    private Dictionary<DataType.MonsterEmotions, EmotionData.EmotionStruct> emoDict;
    private DataType.MonsterEmotions[] selectableEmotions;

    void Awake () {
        emoDict = new Dictionary<DataType.MonsterEmotions, EmotionData.EmotionStruct> ();
        animComp = GetComponent<Animator> ();
        sprRenderer = GetComponent<SpriteRenderer> ();

        SetupEmotionSprites (emotionData.afraid);
        SetupEmotionSprites (emotionData.disgusted);
        SetupEmotionSprites (emotionData.happy);
        SetupEmotionSprites (emotionData.joyous);
        SetupEmotionSprites (emotionData.mad);
        SetupEmotionSprites (emotionData.sad);
        SetupEmotionSprites (emotionData.thoughtful);
        SetupEmotionSprites (emotionData.worried);

        selectableEmotions = new DataType.MonsterEmotions[4];
        selectableEmotions[0] = DataType.MonsterEmotions.Happy;
        selectableEmotions[1] = DataType.MonsterEmotions.Thoughtful;
        selectableEmotions[2] = DataType.MonsterEmotions.Afraid;
        selectableEmotions[3] = DataType.MonsterEmotions.Joyous;

        ChangeEmotions (selectedEmotion);
        StartCoroutine (PlayIdleAnimation ());
    }

    void SetupEmotionSprites(EmotionData.EmotionStruct emoStruct) {
        emoDict.Add (emoStruct.emotion, emoStruct);
    }

    public void ChangeEmotions (DataType.MonsterEmotions emotionToChangeTo) {
        EmotionData.EmotionStruct emoStruct = emoDict[emotionToChangeTo];

        sprRenderer.sprite = emoStruct.sprite;
        selectedEmotion = emoStruct.emotion;
    }

    void OnMouseDown () {
        if (allowMonsterTickle) {
            if (!ParentPage.GetInstance ()) {
                animComp.Play ("Giggle", -1, 0f);
            }
        }
    }

    public void PlayGiggle () {
        SoundManager.GetInstance ().PlaySFXClip (monsterSfx);
    }

    IEnumerator PlayIdleAnimation() {
        while (_idleAnimationOn) {
            ChangeEmotions (
                selectableEmotions[Random.Range (0, selectableEmotions.Length)]
                );
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }

}
