using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrainbowFood : MonoBehaviour {
	private Vector3 offset;
	private Vector3 screenPoint;
	private Transform origin;
	private bool busy = false;
	private bool moving = false;

	void OnMouseDown() {
		if(!busy && !BrainbowGameManager.GetInstance().isGameOver() && GameManager.GetInstance().GetIsInputAllowed()) {
			moving = true;
			BrainbowGameManager.GetInstance().SetActiveFood(this);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

			//Display the food's name
			//move this into game manager with method call
			BrainbowGameManager.GetInstance().ShowSubtitles(gameObject.name, gameObject.GetComponent<Food>().clipOfName);
		}
	}
	
	void FixedUpdate() {
		if(moving && !BrainbowGameManager.GetInstance().isGameOver()) {
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			gameObject.GetComponent<Rigidbody2D>().MovePosition(curPosition);
		}
	}

	void OnMouseUp() {
		if (GameManager.GetInstance ().GetIsInputAllowed ()) {
			if (!busy && moving) {
				busy = true;

				RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, 1.0f, BrainbowGameManager.GetInstance().foodLayerMask);

				if (hit.collider != null && hit.collider.gameObject.GetComponent<ColorDetector> ().color == GetComponent<Food> ().color) {
					BrainbowGameManager.GetInstance ().SetActiveFood (null);

					ColorDetector detector = hit.collider.gameObject.GetComponent<ColorDetector> ();
					SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().correctSound);
					Vector3 oldPos = gameObject.transform.position;
					detector.AddFood (gameObject);

					if (Random.value < 0.3f) {
						int randomClipIndex = Random.Range (0, BrainbowGameManager.GetInstance ().correctMatchClips.Length);
						SoundManager.GetInstance ().PlayVoiceOverClip (BrainbowGameManager.GetInstance ().correctMatchClips [randomClipIndex]);
					}

					gameObject.GetComponent<Collider2D> ().enabled = false;
					BrainbowGameManager.GetInstance ().Replace (gameObject);
				} else {
					MoveBack ();
					int randomClipIndex = Random.Range (0, BrainbowGameManager.GetInstance ().wrongMatchClips.Length);
					SoundManager.GetInstance ().PlayVoiceOverClip (BrainbowGameManager.GetInstance ().wrongMatchClips [randomClipIndex]);
				}
			}
			moving = false;
			busy = false;
			Debug.Log ("About to hide sutitle");
			StartCoroutine (HideSubtitle ());
		}
	}

	IEnumerator HideSubtitle() {
		yield return new WaitForSeconds(0.5f);
		BrainbowGameManager.GetInstance().HideSubtitles();
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
		gameObject.transform.position = GetOrigin().position;
		gameObject.GetComponent<Collider2D>().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (BrainbowGameManager.GetInstance ()) {
			if (BrainbowGameManager.GetInstance ().isGameOver ()) {
				if (other.tag == "Player") {
					gameObject.SetActive (false);
					SoundManager.GetInstance ().PlaySFXClip (BrainbowGameManager.GetInstance ().munchSound);
				}
			}
		}
	}
}
