using UnityEngine;
using System.Collections;

public class BMaze_Pickup : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public string TypeOfPickup;

	private BMaze_PickupManager pickupMan;

	void Start () {
		pickupMan = transform.parent.GetComponent<BMaze_PickupManager> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<BMaze_MonsterMovement>())
			col.GetComponent<BMaze_MonsterMovement>().Pickup (TypeOfPickup);
		pickupMan.pickupList.Remove (gameObject);
		Destroy(gameObject);
	}
}
