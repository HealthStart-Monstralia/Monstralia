using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**
 * \class ChangeSprite
 * \brief Script that changes the sprite on a GameObject while keeping track
 * of its original sprite
 */

public class ChangeSprite : MonoBehaviour {

	Sprite oldSprite; 	/*!< The original sprite */
	bool changed;		/*!< Flag to keep track of whether or not the sprite has changed from the original sprite */

	/** \cond */
	void Awake() {
		oldSprite = gameObject.GetComponent<Image>().sprite;
		changed = false;
	}
	/** \endcond */

	/**
	 * @brief Changes the sprite of the GameObject to newSprite
	 * @param newSprite: the sprite to change to
	 */
	public void changeSpriteImage(Sprite newSprite) {
		Sprite changeTo;

		if(changed) {
			changeTo = oldSprite;
			changed = false;
		}
		else {
			changeTo = newSprite;
			changed = true;
		}

		gameObject.GetComponent<Image>().sprite = changeTo; 
	}
}
