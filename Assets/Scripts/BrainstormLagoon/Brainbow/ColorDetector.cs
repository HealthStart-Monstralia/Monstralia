using UnityEngine;
using System.Collections;

public class ColorDetector : Colorable {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if(other.GetComponent<Food>() != null) {
			Food foodColor = other.gameObject.GetComponent<Food>();
			if(foodColor.color == this.color && !Input.GetMouseButton(0)) {
				Destroy(other.GetComponent<Collider2D>());
				BrainbowGame.instance.Replace(other.gameObject);
			}
		}
	}
}
