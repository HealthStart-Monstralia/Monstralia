using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainbowTutorialHand : MonoBehaviour {

	/* Used in animation events in DragBananaToStripe */
	public void GrabBanana() {
		BrainbowGameManager.Instance.
            tutorialManager.
            tutorialBanana.
            transform.SetParent (gameObject.transform);
	}

	/* Used in animation events in DragBananaToStripe */
	public void DropBanana() {
        BrainbowGameManager.Instance.
            tutorialManager.
            tutorialBanana.
            GetComponent<BrainbowFoodItem> ().
            InsertItemIntoStripe (BrainbowGameManager.Instance.
                stripes[1]
            );
    }
}
