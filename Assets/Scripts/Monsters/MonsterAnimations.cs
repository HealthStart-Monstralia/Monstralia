using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimations : MonoBehaviour {
    public Animator animator;
    public EmotionData emotionData;
    public bool IdleAnimationOn {
        get { return _idleAnimationOn; }
        set {
            _idleAnimationOn = value;
            if (_idleAnimationOn) {
                StartCoroutine (PlayIdleAnimation ());
            }
        }
    }

    private bool _idleAnimationOn;
    [SerializeField] private Monster monster;
    [SerializeField] private AudioClip monsterGiggle;
    private Dictionary<DataType.MonsterEmotions, EmotionData.EmotionStruct> emoDict;
    private DataType.MonsterEmotions[] selectableEmotions;
    private SpriteRenderer spriteRenderer;

    private void Awake () {
        animator = GetComponent<Animator> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        emoDict = new Dictionary<DataType.MonsterEmotions, EmotionData.EmotionStruct> ();

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

        ChangeEmotions (monster.selectedEmotion);
        if (IdleAnimationOn) StartCoroutine (PlayIdleAnimation ());
    }

    public void PlaySpawnAnimation () {
        animator.Play ("MonsterSpawn", -1, 0f);
    }

    public void PlayGiggle () {
        SoundManager.Instance.PlaySFXClip (monsterGiggle);
    }

    void SetupEmotionSprites (EmotionData.EmotionStruct emoStruct) {
        emoDict.Add (emoStruct.emotion, emoStruct);
    }

    public void ChangeEmotions (DataType.MonsterEmotions emotionToChangeTo) {
        EmotionData.EmotionStruct emoStruct = emoDict[emotionToChangeTo];

        spriteRenderer.sprite = emoStruct.sprite;
        monster.selectedEmotion = emoStruct.emotion;
    }

    public IEnumerator PlayIdleAnimation () {
        while (_idleAnimationOn) {
            ChangeEmotions (
                selectableEmotions[Random.Range (0, selectableEmotions.Length)]
                );
            yield return new WaitForSeconds (Random.Range (2f, 6f));
        }
    }
}
