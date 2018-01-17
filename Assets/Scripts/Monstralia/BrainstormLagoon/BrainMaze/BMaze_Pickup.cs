using UnityEngine;
using System.Collections;

public class BMaze_Pickup : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public enum TypeOfPickup { 
		Water = 0, 
		Running = 1,
		Swimming = 2,
		Hiking = 3,
		Biking = 4
	};
	public TypeOfPickup pickup;
	public AudioClip pickupSfx;

    private void Start () {
        BMaze_Manager.GetInstance().AddPickupToList (gameObject);
    }

	protected void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<BMaze_MonsterMovement> ()) {
			if (BMaze_Manager.GetInstance ()) {
                SoundManager.GetInstance ().AddToVOQueue (pickupSfx);
                SoundManager.GetInstance ().PlayCorrectSFX ();

                BMaze_Manager.GetInstance ().ShowSubtitle (pickup.ToString ());
                BMaze_Manager.GetInstance ().OnScore (gameObject);
			}
			gameObject.SetActive (false);
		}
	}
}
