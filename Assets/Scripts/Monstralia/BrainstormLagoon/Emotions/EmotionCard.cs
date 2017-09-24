using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionCard : MonoBehaviour {
    public bool isCardFlipped;
    public EmotionsGameManager.MonsterEmotions emotion;
    public AudioClip clipOfName;

    private Animator animComp;
    private GameObject monster, cardFront;

    private void Awake () {
        animComp = GetComponent<Animator> ();
        cardFront = transform.GetChild (0).gameObject;
        monster = transform.GetChild (1).gameObject;
    }

    public void ChangeCardColor () {
        Color emoColor = EmotionsGameManager.GetInstance ().generator.GetEmotionColor (emotion);
        cardFront.GetComponent<Image> ().color = emoColor;
        print ("Color changed to: " + emoColor);
    }

    public void CardFlip() {
        animComp.Play ("CardFlip", -1, 0f);
    }

    public void ResetCard () {
        animComp.Play ("CardIdle", -1, 0f);
    }

    public void CardUnflip () {
        animComp.Play ("CardUnflip", -1, 0f);
    }

    public void ChangeEmotion(EmotionsGameManager.MonsterEmotions changeToEmotion, Sprite img, AudioClip audio) {
        emotion = changeToEmotion;
        monster.GetComponent<Image> ().sprite = img;
        clipOfName = audio;
    }

    public IEnumerator MoveToAndFlip (GameObject obj) {
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
        transform.SetParent (obj.transform);
        Vector2 pos = obj.transform.position;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 1.0f) {
            transform.position = Vector2.MoveTowards (transform.position, pos, 0.1f);
            yield return null;
        }
        Destroy (gameObject);
    }

    private void OnMouseDown () {
        print ("MouseDown");
        if (EmotionsGameManager.GetInstance ().inputAllowed && (EmotionsGameManager.GetInstance ().isTutorialRunning || EmotionsGameManager.GetInstance ().gameStarted)) {
            EmotionsGameManager.GetInstance ().CheckEmotion (emotion);
            EmotionsGameManager.GetInstance ().subtitlePanel.GetComponent<SubtitlePanel> ().Display (emotion.ToString (), clipOfName);
        }
    }
}
