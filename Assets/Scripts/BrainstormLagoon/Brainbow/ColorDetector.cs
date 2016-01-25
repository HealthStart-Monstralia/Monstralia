using UnityEngine;
using System.Collections;

/**
 * \class ColorDetector
 * \brief Holds the foods after color match is made in the Brainbow game; inherits from Colorable
 * 
 * \todo Change the class name to reflect its new purpose since the RayCast2D detects color matches now
 * 
 * The ColorDetector will hold the food in its corresponding color inside
 * of the rainbow in the Brainbow game
 */

public class ColorDetector : Colorable {
	public Transform[] destinations; /*!< The posistions that the food can "snap" to after it is matched */

	private int nextDest = 0; /*!< The index of the next avaliable position in the ColorDetector */

	/**
	 * \brief Adds a food to the ColorDetector
	 * @param food: A GameObject representing the food that was just matched
	 */
	public void AddFood(GameObject food) {
		food.transform.position = destinations[nextDest++].position;
		if(BrainbowGameManager.GetInstance().IsRunningTutorial()) {
			nextDest = 0;
		}
	}
}
