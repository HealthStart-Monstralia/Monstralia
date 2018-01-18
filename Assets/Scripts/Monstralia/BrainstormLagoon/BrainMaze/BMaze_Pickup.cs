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

    protected void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<BMaze_MonsterMovement> ()) {
			if (BMaze_Manager.Instance) {
                SoundManager.Instance.AddToVOQueue (pickupSfx);
                SoundManager.Instance.PlaySFXClip (SoundManager.Instance.correctSfx2);

                BMaze_Manager.Instance.ShowSubtitle (pickup.ToString ());
                BMaze_Manager.Instance.OnScore (gameObject);
			}
			gameObject.SetActive (false);
		}
	}
}
