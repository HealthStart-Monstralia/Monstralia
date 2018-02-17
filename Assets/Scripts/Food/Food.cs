using UnityEngine;
using System.Collections;

/**
 * \class Food
 * \brief Abstract class that all Foods inherit from; inherits from Colorable.
 * 
 * The abstract Food class handles spawning the food into the scene.
 * Everything else is handled by the individual Food scripts.
 */
public class Food : MonoBehaviour {
    public enum TypeOfFood { Fruit, Vegetable, Dairy, Meat, Misc } /*!< Food property for sorting into lists*/
    public DataType.Color typeOfColor;
    public TypeOfFood foodType;     /*!< Food property variable to set manually in inspector */
    public string foodName;			/*!< Food's name to be used with subtitles */
	public AudioClip clipOfName;	/*!< Audio clip of the food's name to be used with subtitles */
    public AudioClip eatSound;

	/**
	 * \brief Spawn the food into the scene at the specified location
	 * @param spawnPos: The posistion to spawn the food at.
	 * @param parent: The parent GameObject of the food, defaults to null
	 * @param scale: The scale the food is spawned at, defaults to 1.0f
	 */
	public virtual void Spawn(Transform spawnPos, Transform parent = null, float scale = 1.0f) {
		if (parent != null) {
			gameObject.transform.SetParent (parent.transform);
		}

        //gameObject.transform.localPosition = spawnPos.localPosition;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = new Vector3(scale, scale, 1);
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
	}

    public void EatFood () {
        FoodList.IncreaseFoodCount (foodName);
        Destroy (gameObject);
        SoundManager.Instance.PlaySFXClip (eatSound);
    }
}
