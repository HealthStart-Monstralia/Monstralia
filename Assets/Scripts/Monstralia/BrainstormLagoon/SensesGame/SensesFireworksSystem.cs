using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesFireworksSystem : MonoBehaviour {
    public GameObject[] FireWorksPrefab;
    public GameObject[] LargeFireWorkPrefab;
    public Transform[] FireWorkTransform;
    public Transform LargeFireWorkTransform;
    public AudioClip fireworkSfx;

    public void CreateSmallFirework (Transform pos, bool playSound) {
        if (playSound) SoundManager.Instance.PlaySFXClip (fireworkSfx);
        Instantiate (FireWorksPrefab.GetRandomItem(), pos.position, Quaternion.identity);
    }

    public void CreateLargeFirework (Transform pos, bool playSound) {
        if (playSound) SoundManager.Instance.PlaySFXClip (fireworkSfx);
        Instantiate (LargeFireWorkPrefab.GetRandomItem (), pos.position, Quaternion.identity);
    }

    public void ActivateFireworks () {
        CreateSmallFirework (FireWorkTransform[0], false);
        CreateSmallFirework (FireWorkTransform[1], false);
        CreateSmallFirework (FireWorkTransform[2], false);
        CreateSmallFirework (FireWorkTransform[3], false);
        CreateLargeFirework(LargeFireWorkTransform, false);
    }
}
