using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionCard : MonoBehaviour {
    public bool isCardFlipped;
    public DataType.MonsterEmotions emotion;
    public AudioClip clipOfName, cardDraw, cardFlip, cardWoosh;
    public Color colorOfCard;

    public delegate void EmotionDelegate (DataType.MonsterEmotions emotion, AudioClip clip);
    public static EmotionDelegate CheckEmotion; // Set by EmotionsGameManager

    private Animator animComp;
    private GameObject monster, cardFront;
    private AudioSource audioSrc;

    private void Awake () {
        animComp = GetComponent<Animator> ();
        audioSrc = GetComponent<AudioSource> ();
        cardFront = transform.GetChild (0).gameObject;
        monster = transform.GetChild (1).gameObject;
    }

    public void ChangeCardColor () {
        cardFront.GetComponent<Image> ().color = colorOfCard;
    }

    public void CardFlip() {
        audioSrc.PlayOneShot (cardFlip);
        animComp.Play ("CardFlip", -1, 0f);
    }

    public void ResetCard () {
        animComp.Play ("CardIdle", -1, 0f);
    }

    public void CardUnflip () {
        animComp.Play ("CardUnflip", -1, 0f);
    }

    public void ChangeEmotion(DataType.MonsterEmotions changeToEmotion, Sprite img, AudioClip audio) {
        emotion = changeToEmotion;
        monster.GetComponent<Image> ().sprite = img;
        clipOfName = audio;
    }

    public IEnumerator MoveToAndFlip (GameObject obj) {
        SoundManager.Instance.PlaySFXClip (cardDraw);
        Vector2 pos = obj.transform.position;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f) {
            transform.position = Vector2.MoveTowards (transform.position, pos, 0.1f);
            yield return null;
        }
        CardFlip ();
        transform.SetParent (obj.transform);
    }

    public IEnumerator MoveToAndRemove (GameObject obj) {
        CardUnflip ();
        SoundManager.Instance.PlaySFXClip (cardWoosh);
        transform.SetParent (obj.transform);
        Vector2 pos = obj.transform.position;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f) {
            transform.position = Vector2.MoveTowards (transform.position, pos, 0.1f);
            yield return null;
        }
        Destroy (gameObject);
    }

    private void OnMouseDown () {
        CheckEmotion (emotion, clipOfName);
    }
}
