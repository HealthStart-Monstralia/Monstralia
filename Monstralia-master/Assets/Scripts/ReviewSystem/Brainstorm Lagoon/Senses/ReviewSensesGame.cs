using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReviewSensesGame : Singleton<ReviewSensesGame> {
    public Factory senseFactory;
    public Text senseText;
    public bool isGuessing;
    public DataType.Senses selectedSense;

    private static ReviewSensesGame instance;
    [SerializeField] private AudioClip wrongSfx;
    [SerializeField] private GameObject sensePanel;

    new void Awake () {
        base.Awake ();
    }

    private void Start () {
        CreateSenseItem ();
    }

    // Instantiate sense item and replace placeholder
    void CreateSenseItem () {
        GameObject selectedObject = senseFactory.ManufactureRandom ();
        selectedSense = SelectRandomSense (
            selectedObject.GetComponent<SensesItem> ()
        );
        selectedObject.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
        selectedObject.GetComponent<SpriteRenderer> ().sortingOrder = 10;
        print ("Selected Sense: " + selectedSense);
        senseText.text = string.Format ("What do I use to {0} the {1}?", selectedSense.ToString ().ToLower (), selectedObject.name);
    }

    private DataType.Senses SelectRandomSense (SensesItem item) {
        DataType.Senses[] senseItemList = item.validSenses;
        return senseItemList.GetRandomItem ();
    }

    // Loop through each assigned sense in senseItem and see if it matches the button's sense.
    public void CheckSense (DataType.Senses senseButton) {
        bool isCorrect = IsSenseCorrect (senseButton);
        StartCoroutine (OnGuess (isCorrect));
    }

    public bool IsSenseCorrect (DataType.Senses sense) {
        if (selectedSense != DataType.Senses.NONE) {
            if (sense == selectedSense) {
                OnCorrect ();
                return true;
            }

            OnIncorrect ();
        }

        return false;
    }

    void OnCorrect () {
        SubtitlePanel.Instance.Display ("That's correct!");
        SoundManager.Instance.PlayCorrectSFX ();
    }

    void OnIncorrect () {
        SubtitlePanel.Instance.Display ("Not quite, try another one.");
        SoundManager.Instance.PlaySFXClip (wrongSfx);
    }

    IEnumerator OnGuess (bool isCorrect) {
        ActivateSenseButtons (false);
        if (isCorrect) {
            EndReview ();
        }
        else {
            yield return new WaitForSeconds (2f);
            ActivateSenseButtons (true);
        }
    }

    void ActivateSenseButtons (bool activate) {
        if (sensePanel.activeSelf) {
            Button[] childrenButtons = sensePanel.GetComponentsInChildren<Button> ();
            foreach (Button button in childrenButtons) {
                button.interactable = activate;
            }
        }
    }

    // SensesReviewSenseButton uses this function through Instance to signal the review is over
    public void EndReview() {
        senseText.text = "Great job!";
        ReviewManager.Instance.EndReview ();
    }
}
