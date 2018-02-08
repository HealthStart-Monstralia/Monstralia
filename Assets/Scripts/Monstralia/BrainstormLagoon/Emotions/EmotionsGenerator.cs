﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsGenerator : MonoBehaviour {
    public EmotionsCardHand cardHand;
    public DataType.MonsterEmotions currentTargetEmotion;
    public EmotionData blueEmotionsData;
    public EmotionData greenEmotionsData;
    public EmotionData redEmotionsData;
    public EmotionData yellowEmotionsData;
    public Color afraidColor, disgustedColor, happyColor, joyousColor, madColor, sadColor, thoughtfulColor, worriedColor;
    public GameObject monster;

    [SerializeField] private List<EmotionData.EmotionStruct> primaryEmotions = new List<EmotionData.EmotionStruct> ();
    [SerializeField] private List<EmotionData.EmotionStruct> secondaryEmotions = new List<EmotionData.EmotionStruct> ();
    [SerializeField] private List<EmotionData.EmotionStruct> activeEmotions = new List<EmotionData.EmotionStruct> ();
    [SerializeField] private List<EmotionData.EmotionStruct> poolEmotions = new List<EmotionData.EmotionStruct> ();
    private EmotionData.EmotionStruct lastTargetEmotion;
    private bool firstCard;
    private DataType.MonsterType typeOfMonster;
    private int difficultyLevel;
    private int slots;

    private void Awake () {
        typeOfMonster = GameManager.Instance.GetPlayerMonsterType ();
        difficultyLevel = GameManager.Instance.GetLevel (DataType.Minigame.MonsterEmotions);
        slots = difficultyLevel + 1;

        // Initialize emotion lists according to monster type
        firstCard = true;
        switch (typeOfMonster) {
            case DataType.MonsterType.Blue:
                PopulateList (primaryEmotions, blueEmotionsData);
                PopulateList (secondaryEmotions, greenEmotionsData);
                PopulateList (secondaryEmotions, redEmotionsData);
                PopulateList (secondaryEmotions, yellowEmotionsData);
                break;
            case DataType.MonsterType.Green:
                PopulateList (primaryEmotions, greenEmotionsData);
                PopulateList (secondaryEmotions, blueEmotionsData);
                PopulateList (secondaryEmotions, redEmotionsData);
                PopulateList (secondaryEmotions, yellowEmotionsData);
                break;
            case DataType.MonsterType.Red:
                PopulateList (primaryEmotions, redEmotionsData);
                PopulateList (secondaryEmotions, blueEmotionsData);
                PopulateList (secondaryEmotions, greenEmotionsData);
                PopulateList (secondaryEmotions, yellowEmotionsData);
                break;
            case DataType.MonsterType.Yellow:
                PopulateList (primaryEmotions, yellowEmotionsData);
                PopulateList (secondaryEmotions, blueEmotionsData);
                PopulateList (secondaryEmotions, greenEmotionsData);
                PopulateList (secondaryEmotions, redEmotionsData);
                break;
        }
    }

    void PopulateList(List<EmotionData.EmotionStruct> list, EmotionData emotionData) {
        // Add structs to list of structs from emotionData
        // emotionData contains a struct with MonsterEmotions emotion, Sprite sprite, and AudioClip clipOfEmotion
        list.Add (emotionData.afraid);
        list.Add (emotionData.disgusted);
        list.Add (emotionData.happy);
        list.Add (emotionData.joyous);
        list.Add (emotionData.mad);
        list.Add (emotionData.sad);
        list.Add (emotionData.thoughtful);
        list.Add (emotionData.worried);
    }

    public IEnumerator CreateNextEmotions (float duration) {
        EmotionsGameManager.Instance.isDrawingCards = true;
        activeEmotions.Clear ();
        poolEmotions.Clear ();
        print ("primaryEmotions: " + primaryEmotions.Count);
        poolEmotions.AddRange (primaryEmotions);
        print ("poolEmotions: " + poolEmotions.Count);
        yield return new WaitForSeconds (duration);

        RemoveCards ();
        ChooseActiveEmotion ();
        ChooseEmotions ();
        yield return new WaitForSecondsRealtime (0.4f);

        // Create and draw cards
        for (int i = 0; i < slots; i++) {
            int random = Random.Range (0, activeEmotions.Count);
            EmotionData.EmotionStruct emoData = activeEmotions[random];
            activeEmotions.RemoveAt (random);
            CreateCard (emoData, i);
            yield return new WaitForSecondsRealtime (0.4f);
        }
        yield return new WaitForSecondsRealtime (0.5f);
        EmotionsGameManager.Instance.isDrawingCards = false;
        EmotionsGameManager.Instance.ContinueGame ();
    }

    private void ChooseActiveEmotion () {
        // Prevent the same card being drawn more than once in a row
        if (!firstCard)
            poolEmotions.Remove (lastTargetEmotion);            // Remove last active card from deck

        int random = Random.Range (0, poolEmotions.Count);      // Randomly select number within range
        print ("poolEmotions: " + poolEmotions.Count);

        currentTargetEmotion = poolEmotions[random].emotion;    // Select card chosen by random
        lastTargetEmotion = poolEmotions[random];               // Tag selected card to prevent being drawn more than once in a row
        activeEmotions.Add (poolEmotions[random]);              // Add to active emotions deck
        poolEmotions.RemoveAt (random);                         // Remove selected card from the pool

        if (!firstCard)
            poolEmotions.Add (lastTargetEmotion);               // Add last active card to deck
        if (firstCard)
            firstCard = false;                                  // Turn on duplicate card measures
    }

    private void ChooseEmotions () {
        int random;

        if (difficultyLevel >= 3)
            poolEmotions.AddRange (secondaryEmotions);

        for (int i = 0; i < slots - 1; i++) {
            random = Random.Range (0, poolEmotions.Count);
            activeEmotions.Add (poolEmotions[random]);
            poolEmotions.RemoveAt (random);
        }
    }

    public IEnumerator CreateTutorialCards () {
        poolEmotions.AddRange (primaryEmotions);

        currentTargetEmotion = poolEmotions[0].emotion;
        activeEmotions.Add (poolEmotions[0]);
        activeEmotions.Add (poolEmotions[1]);
        poolEmotions.RemoveAt (0);

        for (int i = 0; i < slots; i++) {
            EmotionData.EmotionStruct emoData = activeEmotions[0];
            activeEmotions.RemoveAt (0);

            CreateCard (emoData, i);
            yield return new WaitForSecondsRealtime (0.4f);
        }
    }

    void CreateCard(EmotionData.EmotionStruct emoData, int iteration) {
        cardHand.SpawnCard (
            iteration,
            emoData.emotion,
            emoData.sprite,
            emoData.clipOfEmotion
        );
    }

    public void RemoveCards () {
        cardHand.RemoveCards ();
    }

    public Color GetEmotionColor (DataType.MonsterEmotions emo) {
        switch (emo) {
            case DataType.MonsterEmotions.Afraid:
                return afraidColor;
            case DataType.MonsterEmotions.Disgusted:
                return disgustedColor;
            case DataType.MonsterEmotions.Happy:
                return happyColor;
            case DataType.MonsterEmotions.Joyous:
                return joyousColor;
            case DataType.MonsterEmotions.Mad:
                return madColor;
            case DataType.MonsterEmotions.Sad:
                return sadColor;
            case DataType.MonsterEmotions.Thoughtful:
                return thoughtfulColor;
            case DataType.MonsterEmotions.Worried:
                return worriedColor;
            default:
                return Color.white;
        }
    }

    public void CreateMonster() {
        Vector2 pos = EmotionsGameManager.Instance.monsterLocation.position;
        monster = Instantiate(GameManager.Instance.GetPlayerMonsterObject (), pos, Quaternion.identity);
        monster.transform.position = pos;
        monster.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        monster.gameObject.AddComponent<Animator> ();
        monster.GetComponent<Monster> ().IdleAnimationOn = false;
        monster.GetComponent<Monster> ().AllowMonsterTickle = false;
    }

    public void ChangeMonsterEmotion (DataType.MonsterEmotions emo) {
        monster.GetComponent<Monster> ().ChangeEmotions (emo);
    }

}
