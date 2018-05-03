using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionCard : MonoBehaviour {
    public bool isCardFlipped;
    public DataType.MonsterEmotions emotion;
    public AudioClip clipOfName, cardDraw, cardFlip, cardWoosh;
    public Color colorOfCard;
    public Sprite monsterSprite;

    public delegate void EmotionDelegate (DataType.MonsterEmotions emotion, AudioClip clip);
    public static EmotionDelegate CheckEmotion; // Set by EmotionsGameManager

    private Animator animComp;
    private GameObject monster, cardFront;

    private void Awake () {
        animComp = GetComponent<Animator> ();
        cardFront = transform.GetChild (0).gameObject;
        monster = transform.GetChild (1).gameObject;
    }

    public void ChangeCard () {
        cardFront.GetComponent<Image> ().color = colorOfCard;
        monster.GetComponent<Image> ().sprite = monsterSprite;
    }

    public void CardFlip() {
        SoundManager.Instance.PlaySFXClip (cardFlip);
        animComp.Play ("CardFlip", -1, 0f);
    }

    public void ResetCard () {
        animComp.Play ("CardIdle", -1, 0f);
    }

    public void CardUnflip () {
        animComp.Play ("CardUnflip", -1, 0f);
    }

    public IEnumerator MoveToAndFlip (Transform obj) {
        SoundManager.Instance.PlaySFXClip (cardDraw);
        Vector2 pos = obj.position;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 2f) {
            transform.position = Vector2.Lerp (transform.position, pos, t);
            yield return null;
        }
        CardFlip ();
        transform.SetParent (obj);
    }

    public IEnumerator MoveToAndRemove (Transform obj) {
        CardUnflip ();
        SoundManager.Instance.PlaySFXClip (cardWoosh);
        transform.SetParent (obj.parent);
        Vector2 pos = obj.position;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 0.5f) {
            transform.position = Vector2.Lerp (transform.position, pos, t);
            yield return null;
        }
        Destroy (gameObject, 0.5f);
    }

    private void OnMouseDown () {
        CheckEmotion (emotion, clipOfName);
    }
}
