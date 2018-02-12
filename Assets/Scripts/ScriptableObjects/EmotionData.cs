using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "EmotionsData_.asset", menuName = "Data/Emotions Data")]
public class EmotionData : ScriptableObject {
    public DataType.MonsterType typeOfMonster;

    [System.Serializable]
    public struct EmotionStruct {
        public Sprite sprite;
        public DataType.MonsterEmotions emotion;
        public AudioClip clipOfEmotion;
        public Color emotionColor;
    }

    public EmotionStruct afraid, disgusted, happy, joyous, mad, sad, thoughtful, worried;
}
