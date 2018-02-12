using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsGenerator : MonoBehaviour {
    public EmotionsCardHand cardHand;
    public EmotionData.EmotionStruct currentTargetEmotionStruct;
    public EmotionData blueEmotionsData;
    public EmotionData greenEmotionsData;
    public EmotionData redEmotionsData;
    public EmotionData yellowEmotionsData;
    public Color afraidColor, disgustedColor, happyColor, joyousColor, madColor, sadColor, thoughtfulColor, worriedColor;
    [HideInInspector] public bool allowOtherMonsterCards = false;
    [HideInInspector] public bool isDrawingCards;

    public delegate void CallBack ();
    public delegate void CardDelegate (DataType.MonsterEmotions emotion);
    public CardDelegate CheckEmotion;

    private List<EmotionData.EmotionStruct> primaryEmotions = new List<EmotionData.EmotionStruct> ();
    private List<EmotionData.EmotionStruct> secondaryEmotions = new List<EmotionData.EmotionStruct> ();
    private List<EmotionData.EmotionStruct> activeEmotions = new List<EmotionData.EmotionStruct> ();
    [SerializeField] private List<EmotionData.EmotionStruct> poolEmotions = new List<EmotionData.EmotionStruct> ();
    private DataType.MonsterType typeOfMonster;
    private int slots = 2;

    private void Start () {
        typeOfMonster = GameManager.Instance.GetPlayerMonsterType ();


        // Initialize emotion lists according to monster type
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

    public void SetSlots (int numOfSlots) {
        slots = numOfSlots;
        cardHand.SetSlots (numOfSlots);
    }

    public IEnumerator CreateNextEmotions (float duration, CallBack EmotionsManagerCallback) {
        isDrawingCards = true;
        activeEmotions.Clear ();
        poolEmotions.Clear ();
        poolEmotions.AddRange (primaryEmotions);
        if (!currentTargetEmotionStruct.Equals(null)) {
            poolEmotions.Remove (currentTargetEmotionStruct);   // Prevent the same emotion being selected more than once in a row
            print (string.Format ("currentTargetEmotionStruct.emotion: {0}", currentTargetEmotionStruct.emotion));
        }
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

        isDrawingCards = false;
        EmotionsManagerCallback ();
    }

    private void ChooseActiveEmotion () {
        EmotionData.EmotionStruct selectedEmoStruct = poolEmotions.RemoveRandom (); // Randomly select and remove an emotion from pool
        print (string.Format ("selectedEmoStruct.emotion: {0}", selectedEmoStruct.emotion));
        poolEmotions.Add (currentTargetEmotionStruct);      // Put last emotion back into deck
        currentTargetEmotionStruct = selectedEmoStruct;     // Select emotion chosen by random
        activeEmotions.Add (selectedEmoStruct);             // Add to active emotions deck
    }

    private void ChooseEmotions () {
        if (allowOtherMonsterCards)
            poolEmotions.AddRange (secondaryEmotions);

        for (int i = 0; i < slots - 1; i++) {
            EmotionData.EmotionStruct emoStruct = poolEmotions.RemoveRandom ();
            activeEmotions.Add (emoStruct);
            print (string.Format ("emoStruct.emotion: {0}", emoStruct.emotion));
        }
    }

    public IEnumerator CreateTutorialCards () {
        poolEmotions.AddRange (primaryEmotions);

        currentTargetEmotionStruct = poolEmotions[0];
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

    EmotionCard CreateCard (EmotionData.EmotionStruct emoData, int iteration) {
        EmotionCard card = cardHand.SpawnCard (
            iteration,
            emoData.emotion,
            emoData.sprite,
            GetEmotionColor (emoData.emotion),
            emoData.clipOfEmotion
        );
        return card;
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

    public DataType.MonsterEmotions GetSelectedEmotion () {
        return currentTargetEmotionStruct.emotion;
    }

}
