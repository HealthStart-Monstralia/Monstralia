using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {
    public bool spawnMonsterOnStart = false;
    public bool allowMonsterTickle = false;
    public bool idleAnimationOn = false;
    public bool addRigidbody = false;
    public bool replaceBoxCollider = false;
    public bool playSpawnAnimation = true;

    public Vector3 scale = new Vector3 (1f, 1f, 1f);
    public Transform spawnPosition;
    [Range (0.1f, 5f)]
    public float monsterMass = 1.5f;
    [HideInInspector] public Rigidbody2D rigBody;

    private void Start () {
        if (spawnMonsterOnStart) {
            SpawnMonster (GameManager.GetInstance ().GetPlayerMonsterType ());
            print ("Spawning monster on start from: " + gameObject);
        };
    }

    public Monster SpawnPlayerMonster () {
        if (!spawnPosition)
            spawnPosition = transform;
        return Spawn (GameManager.GetInstance().GetPlayerMonsterType());
    }

    public Monster SpawnMonster (GameObject monsterObject) {
        if (!spawnPosition)
            spawnPosition = transform;
        return Spawn (monsterObject);
    }

    public Monster SpawnMonster(GameObject monsterObject, Transform spot) {
        spawnPosition = spot;
        return Spawn (monsterObject);
    }

    private Monster Spawn(GameObject monsterObject) {
        Monster monster = Instantiate (monsterObject, spawnPosition.parent).GetComponentInChildren<Monster> ();
        monster.transform.parent.position = spawnPosition.position;
        monster.allowMonsterTickle = allowMonsterTickle;
        monster.idleAnimationOn = idleAnimationOn;
        monster.transform.parent.localScale = scale;

        if (replaceBoxCollider) {
            monster.GetComponent<BoxCollider2D> ().enabled = false;
            CapsuleCollider2D capsule = monster.gameObject.AddComponent<CapsuleCollider2D> ();
            capsule.size = capsule.size - new Vector2 (0.25f, 0.25f);
        }

        if (playSpawnAnimation) monster.PlaySpawnAnimation ();

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
