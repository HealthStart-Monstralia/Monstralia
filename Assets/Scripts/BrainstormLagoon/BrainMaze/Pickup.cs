using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public string TypeOfPickup;

	private PickupManager pickupMan;

	void Start () {
		pickupMan = transform.parent.GetComponent<PickupManager> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<MonsterMovement>())
			col.GetComponent<MonsterMovement>().Pickup (TypeOfPickup);
		pickupMan.pickupList.Remove (gameObject);
		Destroy(gameObject);
	}
}
