using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Voiceover_.asset", menuName = "Data/Voiceover Data")]
public class VoiceOversData : ScriptableObject {
    [System.Serializable]
    public struct VoiceOverDictionary {
        public string name;
        public AudioClip clip;
    }

    public VoiceOverDictionary[] VO;

    // Use FindVO to search VO and use SoundManager to play the clip if found
    public void PlayVO(string voName) {
        AudioClip foundClip = FindVO (voName);
        if (foundClip)
            SoundManager.Instance.PlayVoiceOverClip (foundClip);
    }

    // Find the voice over in VO and return the clip, return null if not found
    public AudioClip FindVO (string voName) {
        foreach (VoiceOverDictionary voDict in VO) {
            if (voName == voDict.name) {
                return voDict.clip;
            }
        }
        return null;
    }

}
