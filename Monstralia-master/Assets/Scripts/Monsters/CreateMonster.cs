using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {
    public DataType.MonsterType typeToSpawn;
    public bool spawnMonsterOnStart = false;
    public bool selectPlayerMonsterToSpawn = false;
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
            if (selectPlayerMonsterToSpawn) {
                SpawnPlayerMonster ();
            } else {
                SpawnMonster (typeToSpawn);
            }
            print ("Spawning monster on start from: " + gameObject);
        };
    }

    // Spawn Monster overloads
    // Uses typeToSpawn to decide which monster to spawn
    public Monster SpawnMonster () {
        return SpawnMonster (GameManager.Instance.GetMonsterObject (typeToSpawn), spawnPosition);
    }

    public Monster SpawnMonster (DataType.MonsterType monsterType) {
        return SpawnMonster (GameManager.Instance.GetMonsterObject (monsterType), spawnPosition);
    }

    public Monster SpawnMonster (GameObject monsterObject) {
        return SpawnMonster (monsterObject, spawnPosition);
    }

    public Monster SpawnMonster (DataType.MonsterType monsterType, Transform spot) {
        spawnPosition = spot;
        return SpawnMonster (GameManager.Instance.GetMonsterObject (monsterType), spawnPosition);
    }

    public Monster SpawnMonster(GameObject monsterObject, Transform spot) {
        spawnPosition = spot;
        if (!spawnPosition)
            spawnPosition = transform;
        return Spawn (monsterObject);
    }

    // Spawn Player Monster overloads
    public Monster SpawnPlayerMonster () {
        return SpawnPlayerMonster (spawnPosition);
    }

    public Monster SpawnPlayerMonster (Transform spot) {
        spawnPosition = spot;
        if (!spawnPosition)
            spawnPosition = transform;
        return Spawn (GameManager.Instance.GetPlayerMonsterObject ());
    }

    private Monster Spawn(GameObject monsterObject) {
        Monster monster = Instantiate (monsterObject, spawnPosition.parent).GetComponent<Monster> ();
        monster.transform.position = spawnPosition.position;
        monster.AllowMonsterTickle = allowMonsterTickle;
        monster.IdleAnimationOn = idleAnimationOn;
        monster.transform.localScale = scale;

        if (replaceBoxCollider) {
            monster.colliderComponent.enabled = false;
            CapsuleCollider2D capsule = monster.gameObject.AddComponent<CapsuleCollider2D> ();
            capsule.size = new Vector2 (4f, 5f);
        }

        if (playSpawnAnimation) monster.spawnAnimation = true;

        if (addRigidbody) {
            Rigidbody2D rigBody = monster.transform.gameObject.AddComponent<Rigidbody2D> ();
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
