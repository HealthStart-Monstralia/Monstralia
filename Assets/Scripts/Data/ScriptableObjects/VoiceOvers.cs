using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Voiceover_.asset", menuName = "Data/Voiceover Data")]
public class VoiceOvers : ScriptableObject {
    [System.Serializable]
    public struct VoiceOverDictionary {
        public string name;
        public AudioClip clip;
    }

    public VoiceOverDictionary[] VO;
}
