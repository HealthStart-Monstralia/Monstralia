using UnityEngine;
using System.Collections;

public class BMazePickup : MonoBehaviour {
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
		if (col.GetComponent<BMazeMonsterMovement> ()) {
			if (BMazeManager.Instance) {
                SoundManager.Instance.AddToVOQueue (pickupSfx);
                SoundManager.Instance.PlaySFXClip (SoundManager.Instance.correctSfx2);

                BMazeManager.Instance.ShowSubtitle (pickup.ToString ());
                BMazeManager.Instance.OnScore (this);
			}
			gameObject.SetActive (false);
		}
	}
}
