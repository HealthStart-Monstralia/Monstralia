using UnityEngine;
using System;

public class Food : Colorable {

	private Vector3 offset;
	private Vector3 screenPoint;
	private Transform origin;
	private bool busy;
	private bool moving;
	
	// Use this for initialization
	void Start () {
		busy = false;
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {
		if(!busy) {
			moving = true;
			BrainbowGameManager.GetInstance().SetActiveFood(this);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}

	void OnMouseUp() {
		if(!busy && moving) {
			gameObject.transform.position = GetOrigin().position;
			SoundManager.GetInstance().PlayClip(BrainbowGameManager.GetInstance().wrongSound); 
		}
	}

	public void Spawn(Transform spawnPos, Transform parent, float scale) {
		if(parent != null) {
			gameObject.transform.SetParent (parent.transform);
		}
		gameObject.transform.localPosition = spawnPos.localPosition;
		gameObject.transform.localScale = new Vector3(scale, scale, 1);
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
	}

	public void SetOrigin(Transform origin) {
		this.origin = origin;
	}

	public Transform GetOrigin() {
		return origin;
	}

	public void SetBusy(bool busy) {
		this.busy = busy;
	}

	public bool IsBusy() {
		return busy;
	}

	public void StopMoving() {
		busy = true;
		gameObject.transform.position = GetOrigin().position;
	}
}
