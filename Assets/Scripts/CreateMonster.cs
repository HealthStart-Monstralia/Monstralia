using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {
    public bool allowMonsterTickle = true;
    public bool idleAnimationOn = true;
    public Vector3 scale = new Vector3 (1f, 1f, 1f);
    public Transform spawnPosition;

    public GameObject SpawnMonster() {

        Transform spot;
        if (spawnPosition)
            spot = spawnPosition;
        else
            spot = transform;

        Monster monster = Instantiate (GameManager.GetInstance ().GetMonster (), spot.position, Quaternion.identity, spot.parent).GetComponentInChildren<Monster> ();
        monster.allowMonsterTickle = allowMonsterTickle;
        monster.idleAnimationOn = idleAnimationOn;
        monster.gameObject.transform.localScale = scale;
        return monster.gameObject;
    }
}
