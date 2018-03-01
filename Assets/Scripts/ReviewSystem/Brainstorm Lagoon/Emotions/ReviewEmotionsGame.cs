using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewEmotionsGame : Singleton<ReviewEmotionsGame> {
    public Text text;
    public bool isReviewRunning, inputAllowed;
    public float waitDuration = 2.5f;
    public Transform monsterLocation;

    private GameObject monster;
    private EmotionsGenerator generator;

    void Start () {
        PrepareReview ();
    }

    public void PrepareReview () {
        generator = GetComponent<EmotionsGenerator> ();
        StartCoroutine (BeginReview ());
    }

    IEnumerator BeginReview () {
        generator.SetSlots (3);
        yield return new WaitForSecondsRealtime (1f);
        CreateMonster ();
        monster.GetComponent<Monster> ().spriteRenderer.sortingLayerName = "UI";
        monster.GetComponent<Monster> ().spriteRenderer.sortingOrder = 6;
        isReviewRunning = true;
        EmotionCard.CheckEmotion = CheckEmotion;
        StartCoroutine (generator.CreateNextEmotions (0.5f, ContinueGame));
    }

    public void ContinueGame () {
        SubtitlePanel.Instance.Display ("Match the emotion shown below!");
        ChangeMonsterEmotion (generator.GetSelectedEmotion());
        inputAllowed = true;
    }

    public void CheckEmotion (DataType.MonsterEmotions emotion, AudioClip clip) {
        if (inputAllowed) {
            inputAllowed = false;
            SubtitlePanel.Instance.Display (emotion.ToString (), clip);
            if (emotion == generator.GetSelectedEmotion()) {
                SoundManager.Instance.PlayCorrectSFX ();
                Invoke ("EndReview", 2f);
            } else {
                StartCoroutine (WrongAnswerWait (waitDuration));
            }
        }
    }

    public IEnumerator WrongAnswerWait (float duration) {
        yield return new WaitForSeconds (duration);
        inputAllowed = true;
    }

    public void EndReview () {
        isReviewRunning = false;
        inputAllowed = false;
        //Destroy (monster.gameObject, 1f);
        SubtitlePanel.Instance.Display ("Great job!");
        ChangeMonsterEmotion (DataType.MonsterEmotions.Joyous);
        ReviewManager.Instance.EndReview ();
    }

    public void CreateMonster () {
        Vector2 pos = monsterLocation.position;
        monster = Instantiate (GameManager.Instance.GetPlayerMonsterObject (), pos, Quaternion.identity);
        monster.transform.position = pos;
        monster.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        monster.transform.SetParent (monsterLocation);
        monster.gameObject.AddComponent<Animator> ();
        monster.GetComponent<Monster> ().IdleAnimationOn = false;
        monster.GetComponent<Monster> ().AllowMonsterTickle = false;
    }

    public void ChangeMonsterEmotion (DataType.MonsterEmotions emo) {
        monster.GetComponent<Monster> ().ChangeEmotions (emo);
    }
}
