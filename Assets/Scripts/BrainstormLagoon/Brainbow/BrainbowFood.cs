using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class BrainbowFood : Food {

	private Vector3 offset;
	private Vector3 screenPoint;
	private Transform origin;
	private bool busy;
	private bool moving;
	private static bool gameOver;

	public LayerMask layerMask;
	
	// Use this for initialization
	void Start () {
		busy = false;
		moving = false;
		gameOver = false;
	}

	void OnMouseDown() {
		if(!busy && !gameOver) {
			moving = true;
			BrainbowGameManager.GetInstance().SetActiveFood(this);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

			//Display the food's name
			StartCoroutine(gameObject.GetComponent<Subtitle>().Display(BrainbowGameManager.GetInstance().subtitlePanel, gameObject));
		}
	}
	
	void FixedUpdate() {
		if(moving && !gameOver) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			gameObject.GetComponent<Rigidbody2D>().MovePosition(curPosition);
		}
	}

	void OnMouseUp() {
		if(!busy && moving) {
			busy = true;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1.0f, layerMask);

			if(hit.collider != null && hit.collider.gameObject.GetComponent<ColorDetector>().color == this.color) {
				ColorDetector detector = hit.collider.gameObject.GetComponent<ColorDetector>();
				SoundManager.GetInstance().PlaySFXClip(BrainbowGameManager.GetInstance().correctSound);
				detector.AddFood(gameObject);

				int randomClipIndex = Random.Range(0, BrainbowGameManager.GetInstance().correctMatchClips.Length);
				SoundManager.GetInstance().PlayVoiceOverClip(BrainbowGameManager.GetInstance().correctMatchClips[randomClipIndex]);

				gameObject.GetComponent<Collider2D>().enabled = false;
				BrainbowGameManager.GetInstance().Replace(gameObject);
			}
			else {
				MoveBack ();
				int randomClipIndex = Random.Range(0, BrainbowGameManager.GetInstance().wrongMatchClips.Length);
				SoundManager.GetInstance().PlayVoiceOverClip(BrainbowGameManager.GetInstance().wrongMatchClips[randomClipIndex]);
			}
		}
		moving = false;
		busy = false;
	}

	public void SetOrigin(Transform origin) {
		this.origin = origin;
	}

	public Transform GetOrigin() {
		return origin;
	}

	void MoveBack () {
		gameObject.transform.position = GetOrigin ().position;
		SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().incorrectSound);
	}

	public void StopMoving() {
		gameOver = true;
		gameObject.transform.position = GetOrigin().position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy(this.gameObject);
		if (other.name == "EndGameAnimation(Clone)")
			//Destroy (this.gameObject);
			gameObject.SetActive (false);
	}
	

}
