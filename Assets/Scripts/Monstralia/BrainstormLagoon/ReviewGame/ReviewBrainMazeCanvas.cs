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
        GetComponentInParent<Animator> ().Play ("ReviewBrainMazeGameFadeIn");
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
        reviewText.text = "Great Job!";
		isReviewRunning = false;
		monster.allowMovement = false;
        Invoke ("FadeOut", 2.5f);
        ReviewManager.GetInstance ().EndReview ();
	}

    public void FadeOut() {
        GetComponentInParent<Animator> ().Play ("ReviewBrainMazeGameFadeOut");
        monster.FadeOut ();
    }
}
