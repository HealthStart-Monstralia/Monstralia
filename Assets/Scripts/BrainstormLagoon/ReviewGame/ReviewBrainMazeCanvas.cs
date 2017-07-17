using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewBrainMazeCanvas : MonoBehaviour {
	public ReviewBrainMazeMonster monster;
	public bool isReviewRunning = false;
	private static ReviewBrainMazeCanvas instance;

	void Awake() {
		if(instance == null) {
			instance = this;
		}

		else if(instance != this) {
			Destroy(gameObject);
		}

		GetComponent<Canvas> ().worldCamera = Camera.main;
	}

	void Start() {
		// Change monster sprite depending on player choice
		switch (GameManager.GetMonsterType ()) {
		case GameManager.MonsterType.Blue:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)GameManager.MonsterType.Blue];
				break;
			case GameManager.MonsterType.Green:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)GameManager.MonsterType.Green];
				break;
			case GameManager.MonsterType.Red:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)GameManager.MonsterType.Red];
				break;
			case GameManager.MonsterType.Yellow:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)GameManager.MonsterType.Yellow];
				break;
		}
		PrepareReview ();
	}

	public static ReviewBrainMazeCanvas GetInstance() {
		return instance;
	}

	public void PrepareReview() {
		monster.gameObject.SetActive (true);
		StartCoroutine (BeginReview ());
	}

	IEnumerator BeginReview() {
		yield return new WaitForSecondsRealtime (1f);
		isReviewRunning = true;
		monster.allowMovement = true;
	}

	public void EndReview() {
		isReviewRunning = false;
		monster.allowMovement = false;

		/* Insert function that tells the Review Manager
		 * the review is finished */
	}
}
