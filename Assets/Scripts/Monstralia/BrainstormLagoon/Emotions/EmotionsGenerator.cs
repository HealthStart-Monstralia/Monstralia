using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsGenerator : MonoBehaviour {
    public EmotionsCardHand cardHand;
    public GameObject cardPrefab;
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

    private List<GameObject> cardList = new List<GameObject> ();
    [SerializeField] private Transform cardSpawn;
    private List<EmotionData.EmotionStruct> primaryEmotions = new List<EmotionData.EmotionStruct> ();
    private List<EmotionData.EmotionStruct> secondaryEmotions = new List<EmotionData.EmotionStruct> ();
    private List<EmotionData.EmotionStruct> activeEmotions = new List<EmotionData.EmotionStruct> ();
    [SerializeField] private List<EmotionData.EmotionStruct> poolEmotions = new List<EmotionData.EmotionStruct> ();
    private DataType.MonsterType typeOfMonster;
    private int numOfSlots = 2;

    private void Start () {
        if (!cardSpawn) {
            cardSpawn = cardHand.transform;
        }

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

        poolEmotions.Clear ();
        poolEmotions.AddRange (primaryEmotions);
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
        this.numOfSlots = numOfSlots;
        cardHand.SetSlots (numOfSlots);
    }

    public IEnumerator CreateNextEmotions (float duration, CallBack EmotionsManagerCallback) {
        isDrawingCards = true;
        activeEmotions.Clear ();
        poolEmotions.Clear ();
        poolEmotions.AddRange (primaryEmotions);
        if (!currentTargetEmotionStruct.Equals(null)) {
            poolEmotions.Remove (currentTargetEmotionStruct);   // Prevent the same emotion being selected more than once in a row
        }
        yield return new WaitForSeconds (duration);

        RemoveCards ();
        ChooseActiveEmotion ();
        ChooseEmotions ();
        yield return new WaitForSecondsRealtime (0.4f);

        // Create and draw cards
        for (int i = 0; i < numOfSlots; i++) {
            int random = Random.Range (0, activeEmotions.Count);
            EmotionData.EmotionStruct emoData = activeEmotions[random];
            activeEmotions.RemoveAt (random);
            CreateCard (emoData);
            yield return new WaitForSecondsRealtime (0.4f);
        }
        yield return new WaitForSecondsRealtime (0.5f);

        isDrawingCards = false;
        EmotionsManagerCallback ();
    }

    private void ChooseActiveEmotion () {
        EmotionData.EmotionStruct selectedEmoStruct = poolEmotions.RemoveRandom (); // Randomly select and remove an emotion from pool
        poolEmotions.Add (currentTargetEmotionStruct);      // Put last emotion back into deck
        currentTargetEmotionStruct = selectedEmoStruct;     // Select emotion chosen by random
        activeEmotions.Add (selectedEmoStruct);             // Add to active emotions deck
    }

    private void ChooseEmotions () {
        if (allowOtherMonsterCards)
            poolEmotions.AddRange (secondaryEmotions);

        for (int i = 0; i < numOfSlots - 1; i++) {
            EmotionData.EmotionStruct emoStruct = poolEmotions.RemoveRandom ();
            activeEmotions.Add (emoStruct);
        }
    }

    public IEnumerator CreateTutorialCards () {
        poolEmotions.AddRange (primaryEmotions);

        currentTargetEmotionStruct = poolEmotions[0];
        activeEmotions.Add (poolEmotions[0]);
        activeEmotions.Add (poolEmotions[1]);
        poolEmotions.RemoveAt (0);

        for (int i = 0; i < numOfSlots; i++) {
            EmotionData.EmotionStruct emoData = activeEmotions[0];
            activeEmotions.RemoveAt (0);

            CreateCard (emoData);
            yield return new WaitForSecondsRealtime (0.4f);
        }
    }

    public EmotionCard CreateCard (EmotionData.EmotionStruct emoData) {
        EmotionCard card = Instantiate (
            cardPrefab,
            cardSpawn.position,
            Quaternion.identity,
        cardSpawn.parent).GetComponent<EmotionCard> ();

        card.monsterSprite = emoData.sprite;
        card.emotion = emoData.emotion;
        card.clipOfName = emoData.clipOfEmotion;
        card.colorOfCard = emoData.emotionColor;

        cardList.Add (card.gameObject);
        if (cardList.Count > 0)
            cardHand.PutCardInSlot (card, cardList.Count - 1);
        return card;
    }

    public void RemoveCards () {
        foreach (GameObject cardSlot in cardHand.cardLocations) {
            for (int slot = 0; slot < numOfSlots; slot++) {
                EmotionCard card = cardHand.GetCardInSlot (slot);
                if (card) {
                    cardList.Remove (card.gameObject);
                    card.StartCoroutine (card.MoveToAndRemove (cardSpawn));
                }
            }
        }
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
