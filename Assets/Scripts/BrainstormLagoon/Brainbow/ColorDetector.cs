using UnityEngine;
using System.Collections;

public class ColorDetector : Colorable {
	public Transform[] destinations;
	private int nextDest = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		if(!Input.GetMouseButton(0)) {
			if(other.GetComponent<Food>() != null) {
				Food food = other.gameObject.GetComponent<Food>();

				if(!food.IsBusy ()) {
					food.SetBusy(true);
					if(food.color == this.color) {
						food.SetBusy(true);
						SoundManager.GetInstance().PlayClip(BrainbowGame.GetInstance().correctSound);
						other.gameObject.transform.position = destinations[nextDest++].position;
						Destroy(other.GetComponent<Collider2D>());
						BrainbowGame.GetInstance().Replace(other.gameObject);
					}
					else if(!Input.GetMouseButton(0)) {
						food.SetBusy(false);
						other.gameObject.transform.position = other.GetComponent<Food>().GetOrigin().position;
					}
				}
			}

		}
	}
}
