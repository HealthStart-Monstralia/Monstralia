using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {
    public bool allowMonsterTickle = true;
    public bool idleAnimationOn = true;
    public bool addRigidbody = false;
    public Vector3 scale = new Vector3 (1f, 1f, 1f);
    public Transform spawnPosition;
    [Range (0.1f, 5f)]
    public float monsterMass = 1.5f;
    [HideInInspector] public Rigidbody2D rigBody;

    private void Update () {
        if (rigBody)
            if (rigBody.mass != monsterMass) rigBody.mass = monsterMass;
    }

    public Monster SpawnMonster() {
        Transform spot;
        if (spawnPosition)
            spot = spawnPosition;
        else
            spot = transform;

        Monster monster = Instantiate (GameManager.GetInstance ().GetMonster (), spot.position, Quaternion.identity, spot.parent).GetComponentInChildren<Monster> ();
        monster.allowMonsterTickle = allowMonsterTickle;
        monster.idleAnimationOn = idleAnimationOn;
        monster.transform.parent.localScale = scale;
        monster.GetComponent<BoxCollider2D> ().enabled = false;
        monster.gameObject.AddComponent<CapsuleCollider2D> ();
        monster.PlaySpawnAnimation ();

        if (addRigidbody) {
            Rigidbody2D rigBody = monster.transform.parent.gameObject.AddComponent<Rigidbody2D> ();
            rigBody.mass = monsterMass;
            rigBody.drag = 0.3f;
            rigBody.freezeRotation = true;
            rigBody.gravityScale = 1.5f;
            rigBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        return monster;
    }
}
