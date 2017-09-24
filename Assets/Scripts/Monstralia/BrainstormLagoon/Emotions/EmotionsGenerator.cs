using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionsGenerator : MonoBehaviour {
    public EmotionsCardHand cardHand;
    public EmotionsGameManager.MonsterEmotions currentTargetEmotion;
    public List<GameObject> blueEmotions;
    public List<GameObject> greenEmotions;
    public List<GameObject> redEmotions;
    public List<GameObject> yellowEmotions;
    public Color afraidColor, disgustedColor, happyColor, joyousColor, madColor, sadColor, thoughtfulColor, worriedColor;
    public GameObject monster;

    private EmotionsGameManager.MonsterEmotions lastTargetEmotion;
    private DataType.MonsterType typeOfMonster;
    private List<GameObject> primaryEmotions = new List<GameObject>();
    private List<GameObject> secondaryEmotions = new List<GameObject>();
    private List<GameObject> activeEmotions = new List<GameObject>();
    [SerializeField] private List<GameObject> poolEmotions = new List<GameObject> ();
    private float scale = 40f;
    private float toMatchScale = 70f;
    private int difficultyLevel;
    private int slots;

    private void Awake () {
        typeOfMonster = GameManager.GetInstance ().GetMonsterType ();
        difficultyLevel = GameManager.GetInstance ().GetLevel (DataType.Minigame.MonsterEmotions);
        slots = difficultyLevel + 1;
    }

    private void Start () {
        // Initialize emotion lists according to monster type
        switch (typeOfMonster) {
            case DataType.MonsterType.Blue:
                primaryEmotions = blueEmotions;
                secondaryEmotions.AddRange (greenEmotions);
                secondaryEmotions.AddRange (redEmotions);
                secondaryEmotions.AddRange (yellowEmotions);
                break;
            case DataType.MonsterType.Green:
                primaryEmotions = greenEmotions;
                secondaryEmotions.AddRange (blueEmotions);
                secondaryEmotions.AddRange (redEmotions);
                secondaryEmotions.AddRange (yellowEmotions);
                break;
            case DataType.MonsterType.Red:
                primaryEmotions = redEmotions;
                secondaryEmotions.AddRange (greenEmotions);
                secondaryEmotions.AddRange (blueEmotions);
                secondaryEmotions.AddRange (yellowEmotions);
                break;
            case DataType.MonsterType.Yellow:
                primaryEmotions = yellowEmotions;
                secondaryEmotions.AddRange (greenEmotions);
                secondaryEmotions.AddRange (redEmotions);
                secondaryEmotions.AddRange (blueEmotions);
                break;
        }
    }

    public IEnumerator CreateNextEmotions (float duration) {
        print ("Creating emotions");
        activeEmotions.Clear ();
        poolEmotions.Clear ();
        poolEmotions.AddRange (primaryEmotions);
        yield return new WaitForSeconds (duration);

        RemoveCards ();
        ChooseActiveEmotion ();
        ChooseEmotions ();
        for (int i = 0; i < slots; i++) {
            int random = Random.Range (0, activeEmotions.Count);
            GameObject emotionObject = activeEmotions[random];
            activeEmotions.RemoveAt (random);

            EmotionBehavior emo = emotionObject.GetComponent<EmotionBehavior> ();
            SpriteRenderer spriteCard = emotionObject.GetComponent<SpriteRenderer> ();
            cardHand.SpawnCard (
                i,
                emo.emotions,
                spriteCard.sprite,
                emo.clipOfName
                );
            yield return new WaitForSecondsRealtime (0.4f);
        }
        yield return new WaitForSecondsRealtime (0.5f);
        EmotionsGameManager.GetInstance().isDrawingCards = false;
        EmotionsGameManager.GetInstance ().ContinueGame ();
    }

    public void RemoveCards() {
        cardHand.RemoveCards ();
    }

    private void ChooseActiveEmotion () {
        int random = Random.Range (0, poolEmotions.Count);

        currentTargetEmotion = poolEmotions[random].GetComponent<EmotionBehavior>().emotions;
        activeEmotions.Add (poolEmotions[random]);
        poolEmotions.RemoveAt (random);
    }

    private void ChooseEmotions () {
        bool addOnce = true;
        int random;

        for (int i = 0; i < slots - 1; i++) {
            if (difficultyLevel >= 3 && i > 0) {

                if (addOnce) {
                    poolEmotions.AddRange (secondaryEmotions);
                    addOnce = false;
                }

                random = Random.Range (0, poolEmotions.Count);
                activeEmotions.Add (poolEmotions[random]);
                poolEmotions.RemoveAt (random);
            }

            else {
                random = Random.Range (0, poolEmotions.Count);
                activeEmotions.Add (poolEmotions[random]);
                poolEmotions.RemoveAt (random);
            }
        }
    }

    public Color GetEmotionColor (EmotionsGameManager.MonsterEmotions emo) {
        switch (emo) {
            case EmotionsGameManager.MonsterEmotions.Afraid:
                return afraidColor;
            case EmotionsGameManager.MonsterEmotions.Disgusted:
                return disgustedColor;
            case EmotionsGameManager.MonsterEmotions.Happy:
                return happyColor;
            case EmotionsGameManager.MonsterEmotions.Joyous:
                return joyousColor;
            case EmotionsGameManager.MonsterEmotions.Mad:
                return madColor;
            case EmotionsGameManager.MonsterEmotions.Sad:
                return sadColor;
            case EmotionsGameManager.MonsterEmotions.Thoughtful:
                return thoughtfulColor;
            case EmotionsGameManager.MonsterEmotions.Worried:
                return worriedColor;
            default:
                return Color.white;
        }
    }

    public void CreateMonster() {
        Vector2 pos = EmotionsGameManager.GetInstance ().monsterLocation.position;
        monster = Instantiate(GameManager.GetInstance ().GetMonster (), pos, Quaternion.identity);
        monster.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        monster.AddComponent<EmotionsMonster> ();
        monster.GetComponentInChildren<Monster> ().idleAnimationOn = false;
        monster.GetComponentInChildren<Monster> ().allowMonsterTickle = false;
    }

    public void ChangeMonsterEmotion (EmotionsGameManager.MonsterEmotions emo) {
        monster.GetComponentInChildren<Monster> ().ChangeEmotions (emo);
        /*
        switch (emo) {
            case EmotionsGameManager.MonsterEmotions.Afraid:
                break;
            case EmotionsGameManager.MonsterEmotions.Disgusted:
                break;
            case EmotionsGameManager.MonsterEmotions.Happy:
                break;
            case EmotionsGameManager.MonsterEmotions.Joyous:
                break;
            case EmotionsGameManager.MonsterEmotions.Mad:
                break;
            case EmotionsGameManager.MonsterEmotions.Sad:
                break;
            case EmotionsGameManager.MonsterEmotions.Thoughtful:
                break;
            case EmotionsGameManager.MonsterEmotions.Worried:
                break;
            default:
                break;
        }
        */
    }

}
