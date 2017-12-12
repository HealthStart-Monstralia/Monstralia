using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewBrainMazeCanvas : MonoBehaviour {
	public ReviewBrainMazeMonster monster;
	public bool isReviewRunning = false;
    public Text reviewText;
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
		switch (GameManager.GetInstance().GetMonsterType ()) {
		case DataType.MonsterType.Blue:
			monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)DataType.MonsterType.Blue];
				break;
		case DataType.MonsterType.Green:
		monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)DataType.MonsterType.Green];
			break;
		case DataType.MonsterType.Red:
		monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)DataType.MonsterType.Red];
			break;
		case DataType.MonsterType.Yellow:
		monster.GetComponentInChildren<SpriteRenderer>().sprite = monster.spriteList [(int)DataType.MonsterType.Yellow];
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
        reviewText.text = "Great Job!";
		isReviewRunning = false;
		monster.allowMovement = false;
        ReviewManager.GetInstance ().EndReview ();
	}
}
