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
				print("is food busy? " + food.IsBusy());

				if(!food.IsBusy ()) {
					print("in !food.isbusy()");
					food.SetBusy(true);
					if(food.color == this.color) {
						food.SetBusy(true);
						print("color matches");
						other.gameObject.transform.position = destinations[nextDest++].position;
						Destroy(other.GetComponent<Collider2D>());
						BrainbowGame.instance.Replace(other.gameObject);
					}
					else if(!Input.GetMouseButton(0)) {
						print("color doesn't match");
						food.SetBusy(false);
						other.gameObject.transform.position = other.GetComponent<Food>().GetOrigin().position;
					}
				}
			}

		}
	}
}
