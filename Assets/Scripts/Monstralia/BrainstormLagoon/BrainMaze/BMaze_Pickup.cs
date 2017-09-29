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

    private BMaze_PickupManager pickupMan;

    private void Start () {
        pickupMan = BMaze_Manager.GetInstance ().pickupMan;
        pickupMan.AddToList (gameObject);
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<BMaze_MonsterMovement> ()) {
			if (BMaze_Manager.GetInstance ()) {
                SoundManager.GetInstance ().PlayVoiceOverClip (pickupSfx);

                if (pickup == TypeOfPickup.Water)
					GetComponent<BMaze_WaterPickup> ().IncreaseTime ();
                else
                    SoundManager.GetInstance ().PlayCorrectSFX ();

                BMaze_Manager.GetInstance ().ShowSubtitle (pickup.ToString ());
				pickupMan.PickupScored (gameObject);
			}
			gameObject.SetActive (false);
		}
	}

	public void ReActivate() {
        pickupMan.ReactivatePickup (gameObject);
    }
}
