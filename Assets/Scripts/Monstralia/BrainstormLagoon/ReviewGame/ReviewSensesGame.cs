using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewSensesGame : MonoBehaviour {
    public SensesReviewSenseItem senseItem;
    public GameObject[] senseItemsSpawnArray;
    public Text senseText;
    public bool isGuessing;
    private static ReviewSensesGame instance;
    private bool isSenseRight = false;

    void Awake () {
        // Enforce singleton property
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
        CreateSenseItem ();
    }

    // Instantiate sense item and replace placeholder
    void CreateSenseItem () {
        GameObject item = Instantiate (senseItemsSpawnArray[Random.Range (0, senseItemsSpawnArray.Length)], senseItem.transform.parent);
        Destroy (senseItem.gameObject); // Remove current sense item as it was a placeholder
        senseItem = item.GetComponent<SensesReviewSenseItem> ();
    }

    // Access ReviewSensesGame instance
    public static ReviewSensesGame GetInstance () {
        return instance;
    }

    // Loop through each assigned sense in senseItem and see if it matches the button's sense.
    public void CheckSense (SensesReviewSenseItem.Senses senseButton) {
        foreach (SensesReviewSenseItem.Senses itemSense in senseItem.assignSenses) {
            if (senseButton == itemSense && !isSenseRight) {
                isSenseRight = true;
                print ("SenseButton: " + senseButton + " | itemSense: " + itemSense);

                if (ReviewManager.GetInstance ()) {
                    ReviewManager.GetInstance ().EndReview ();
                } else {
                    Destroy (gameObject, 2f);
                }

                EndReview ();
                SoundManager.GetInstance ().PlayCorrectSFX ();
            }
        }
        if (!isSenseRight)
            StartCoroutine (ChoiceCooldown ());
    }

    IEnumerator ChoiceCooldown() {
        yield return new WaitForSeconds (1f);
        isGuessing = false;
    }

    // SensesReviewSenseButton uses this function through GetInstance() to signal the review is over
    public void EndReview() {
        senseText.text = "Great job!";
        ReviewManager.GetInstance ().EndReview ();
    }
}
