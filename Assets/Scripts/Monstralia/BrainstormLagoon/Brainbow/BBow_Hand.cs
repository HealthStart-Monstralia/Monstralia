using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBow_Hand : MonoBehaviour {

	/* Used in animation events in DragBananaToStripe */
	public void GrabBanana() {
		BrainbowGameManager.GetInstance ().tutorialBanana.transform.SetParent (gameObject.transform);
	}

	/* Used in animation events in DragBananaToStripe */
	public void DropBanana() {
		BrainbowGameManager.GetInstance ().tutorialBanana.transform.position = 
			BrainbowGameManager.GetInstance ().tutorialSpot.transform.position;
		BrainbowGameManager.GetInstance ().tutorialBanana.transform.SetParent (
			BrainbowGameManager.GetInstance().tutorialSpot.transform);
	}
}
