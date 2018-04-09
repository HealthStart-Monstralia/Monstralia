using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsGenerator : MonoBehaviour {
    public EmotionsCardHand cardHand;
    public GameObject cardPrefab;
    public EmotionData.EmotionStruct currentTargetEmotionStruct;
    public DataType.MonsterEmotions currentTargetEmotion;

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
    [SerializeField] private List<EmotionData> poolOfEmotionsData = new List<EmotionData> ();

    [SerializeField] private List<DataType.MonsterEmotions> emotions = new List<DataType.MonsterEmotions> ();
    [SerializeField] private List<DataType.MonsterEmotions> poolOfEmotions = new List<DataType.MonsterEmotions> ();

    private DataType.MonsterType typeOfMonster;
    private int numOfSlots = 2;

    private void Start () {
        if (!cardSpawn) {
            cardSpawn = cardHand.transform;
        }

        typeOfMonster = GameManager.Instance.GetPlayerMonsterType ();
        ResetEmotionsList ();

        poolOfEmotionsData.Add (blueEmotionsData);
        poolOfEmotionsData.Add (greenEmotionsData);
        poolOfEmotionsData.Add (redEmotionsData);
        poolOfEmotionsData.Add (yellowEmotionsData);
    }

    void ResetEmotionsList () {
        emotions.Clear ();
        // Grab each type of emotion and store inside a list.
        foreach (DataType.MonsterEmotions emotion in System.Enum.GetValues (typeof (DataType.MonsterEmotions))) {
            emotions.Add (emotion);
        }
    }

    public void SetSlots (int numOfSlots) {
        this.numOfSlots = numOfSlots;
        cardHand.SetSlots (numOfSlots);
    }

    public IEnumerator CreateNextEmotions (float duration, CallBack EmotionsManagerCallback) {
        isDrawingCards = true;
        ResetEmotionsList ();
        poolOfEmotions.Clear ();

        yield return new WaitForSeconds (duration);

        RemoveCards ();
        ChooseActiveEmotion ();
        ChooseEmotions ();
        yield return new WaitForSecondsRealtime (0.4f);

        EmotionData.EmotionStruct emoData;

        // Create and draw cards
        for (int i = 0; i < numOfSlots; i++) {
            if (allowOtherMonsterCards) {
                EmotionData data = poolOfEmotionsData.GetRandomItem();
                emoData = GetEmotionStruct (poolOfEmotions.RemoveRandom (), data);
            }
            else {
                emoData = GetEmotionStruct (poolOfEmotions.RemoveRandom (), GetPlayerMonsterData ());
            }

            CreateCard (emoData);
            yield return new WaitForSecondsRealtime (0.4f);
        }
        yield return new WaitForSecondsRealtime (0.5f);

        isDrawingCards = false;
        EmotionsManagerCallback ();
    }

    private void RemoveDuplicateEmotion () {
        poolOfEmotions.Remove (currentTargetEmotion);   // Prevent the same emotion being selected more than once in a row
    }

    private void ChooseActiveEmotion () {

        // Remove last emotion selected if applicable
        if (!currentTargetEmotion.Equals (null)) {

            // Define a variable that holds the last emotion selected
            DataType.MonsterEmotions holdEmotion = currentTargetEmotion;
            emotions.Remove (holdEmotion);

            currentTargetEmotion = emotions.RemoveRandom ();
            poolOfEmotions.Add (currentTargetEmotion);

            emotions.Add (holdEmotion);
        }
        else {
            currentTargetEmotion = emotions.RemoveRandom ();
            poolOfEmotions.Add (currentTargetEmotion);
        }
    }

    private void ChooseEmotions () {
        // Choose emotions from list
        for (int i = 0; i < numOfSlots - 1; i++) {
            DataType.MonsterEmotions emotion = emotions.RemoveRandom ();
            poolOfEmotions.Add (emotion);
        }
    }

    public void SelectTutorialEmotions () {
        currentTargetEmotion = DataType.MonsterEmotions.Afraid;
        poolOfEmotions.Add (currentTargetEmotion);
        poolOfEmotions.Add (DataType.MonsterEmotions.Thoughtful);
    }

    public IEnumerator CreateTutorialCards () {
        for (int i = 0; i < numOfSlots; i++) {
            EmotionData.EmotionStruct emoData = GetEmotionStruct (poolOfEmotions[i], GetPlayerMonsterData ());

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
        return currentTargetEmotion;
    }

    public EmotionData.EmotionStruct GetEmotionStruct (DataType.MonsterEmotions emotion, EmotionData data) {
        switch (emotion) {
            case DataType.MonsterEmotions.Afraid:
                return data.afraid;
            case DataType.MonsterEmotions.Disgusted:
                return data.disgusted;
            case DataType.MonsterEmotions.Happy:
                return data.happy;
            case DataType.MonsterEmotions.Joyous:
                return data.joyous;
            case DataType.MonsterEmotions.Mad:
                return data.mad;
            case DataType.MonsterEmotions.Sad:
                return data.sad;
            case DataType.MonsterEmotions.Thoughtful:
                return data.thoughtful;
            case DataType.MonsterEmotions.Worried:
                return data.worried;
            default:
                return data.happy;
        }
    }

    public EmotionData GetPlayerMonsterData () {
        switch (typeOfMonster) {
            case DataType.MonsterType.Blue: return blueEmotionsData;
            case DataType.MonsterType.Green: return greenEmotionsData;
            case DataType.MonsterType.Red: return redEmotionsData;
            case DataType.MonsterType.Yellow: return yellowEmotionsData;
            default: return blueEmotionsData;
        }
    }

}