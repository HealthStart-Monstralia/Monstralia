using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewBrainMazeCanvas : Singleton<ReviewBrainMazeCanvas> {
	public Transform monsterSpawn;
    public CreateMonster monsterCreator;
	public bool isReviewRunning = false;
    public Text reviewText;

    private Monster monster;
    private Animator monsterAnimator;

    new void Awake() {
		GetComponent<Canvas> ().worldCamera = Camera.main;
	}

	void Start() {
		PrepareReview ();
	}

	public void PrepareReview() {
        monster = monsterCreator.SpawnPlayerMonster (monsterSpawn);
        monster.transform.gameObject.AddComponent<BMazeMonsterMovement> ();
        monsterAnimator = monster.GetComponentInChildren<Animator> ();
        StartCoroutine (BeginReview ());
	}

	IEnumerator BeginReview() {
		yield return new WaitForSecondsRealtime (1f);
		isReviewRunning = true;
		BMazeMonsterMovement.isMonsterMovementAllowed = true;
	}

	public void EndReview() {
        reviewText.text = "Great Job!";
		isReviewRunning = false;
        BMazeMonsterMovement.isMonsterMovementAllowed = false;
        ReviewManager.Instance.EndReview ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
        Invoke ("FadeMonsterOut", 2f);
	}

    public void FadeMonsterOut () {
        monsterAnimator.Play ("MonsterDespawn", -1, 0f);
    }
}
