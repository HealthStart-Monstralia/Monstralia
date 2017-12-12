using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BMaze_PickupManager : MonoBehaviour {
    /* CREATED BY: Colby Tang.
	 * GAME: Brain Maze
	 */
	public List<GameObject> pickupList = new List<GameObject> ();

	private bool achieved = false;
    private int score = 0;

    public void ResetScore () {
        score = 0;
    }

    public void AddToList (GameObject obj) {
        pickupList.Add (obj);
        ChangeProgress ();
    }

    public void PickupScored (GameObject obj) {
        obj.SetActive (false);
        score++;
        ChangeProgress ();
        if (score >= pickupList.Count)
            GoalAchieved ();
    }

    public void ReactivatePickup(GameObject obj) {
        obj.SetActive (true);
        ChangeProgress ();
    }

	void GoalAchieved () {
		BMaze_Manager.GetInstance().UnlockDoor ();
	}

    void ChangeProgress() {
        BMaze_Manager.GetInstance ().ChangeProgressBar ((float)score / pickupList.Count);
    }
}
