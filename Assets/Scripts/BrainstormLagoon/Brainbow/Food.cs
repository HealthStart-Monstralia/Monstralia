﻿using UnityEngine;
using System;

public class Food : Colorable {

	private Rigidbody2D rb;

	private Vector3 offset;
	private Vector3 screenPoint;

	private Transform origin;

	private bool busy;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		busy = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		//gameObject.transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameObject.transform.localPosition.z);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
		//		Vector3 mousePos = Input.mousePosition;
//		mousePos.z = gameObject.transform.position.z;
//		transform.position = Camera.main.ScreenToWorldPoint(mousePos);
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
}
