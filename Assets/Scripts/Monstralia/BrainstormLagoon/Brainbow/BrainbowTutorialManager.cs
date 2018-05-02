using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainbowTutorialManager : MonoBehaviour {
    public GameObject tutorialHand;
    [HideInInspector] public GameObject tutorialBanana;
    public GameObject tutorialApplePrefab;
    public GameObject tutorialBananaPrefab;
    public GameObject tutorialBroccoliPrefab;
    public GameObject tutorialEggplantPrefab;
    public Canvas instructionPopup;
    public GameObject waterNotification;
    public bool isRunningTutorial = false;
    public Coroutine tutorialCoroutine;

    private BrainbowGameManager brainbowManager;

    private void Awake () {
        instructionPopup.gameObject.SetActive (false);
        waterNotification.SetActive (false);
        tutorialHand.SetActive (false);
    }

    public void StartTutorial() {
        if (!BrainbowGameManager.Instance.GetGameStarted()) {
            brainbowManager = BrainbowGameManager.Instance;
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        }
    }

    IEnumerator RunTutorial () {
        isRunningTutorial = true;
        brainbowManager.inputAllowed = false;
        instructionPopup.gameObject.SetActive (true);
        yield return new WaitForSeconds (1.0f);
        brainbowManager.monsterObject.ChangeEmotions (DataType.MonsterEmotions.Happy);
        StartCoroutine (TurnOnRainbows ());
        AudioClip tutorial1 = brainbowManager.voData.FindVO ("1_tutorial_start");
        SoundManager.Instance.PlayVoiceOverClip (tutorial1);
        yield return new WaitForSeconds (tutorial1.length);

        AudioClip tutorial2 = brainbowManager.voData.FindVO ("2_tutorial_goodfood");
        SoundManager.Instance.PlayVoiceOverClip (tutorial2);
        yield return new WaitForSeconds (tutorial2.length - 0.5f);

        brainbowManager.foodPanel.gameObject.SetActive (true);
        brainbowManager.foodPanel.TurnOnNumOfSlots (1);
        CreateBanana ();
        yield return new WaitForSeconds (1f);

        AudioClip tutorial3 = brainbowManager.voData.FindVO ("3_tutorial_likethis");
        SoundManager.Instance.PlayVoiceOverClip (tutorial3);
        yield return new WaitForSeconds (tutorial3.length - 1f);

        tutorialHand.SetActive (true);
        tutorialHand.GetComponent<Animator> ().Play ("DragBananaToStripe");
        yield return new WaitForSeconds (5f);

        tutorialHand.SetActive (false);
        tutorialBanana.SetActive (false);
        brainbowManager.foodPanel.GetComponent<Outline> ().enabled = false;

        if (isRunningTutorial) {
            brainbowManager.ShowRainbowStripe (1, false);
            yield return new WaitForSeconds (0.5f);
            brainbowManager.ShowRainbowStripe (1, true);
            yield return new WaitForSeconds (0.5f);
            SubtitlePanel.Instance.Display ("Now You Try!", brainbowManager.voData.FindVO ("4_tutorial_tryit"));
            brainbowManager.inputAllowed = true;
            CreateBanana ();
        }
    }

    public IEnumerator TutorialEnd () {
        StopCoroutine (tutorialCoroutine);
        brainbowManager.inputAllowed = false;
        AudioClip letsPlay = brainbowManager.voData.FindVO ("letsplay");
        SubtitlePanel.Instance.Display ("Perfect! Let's play!");
        SoundManager.Instance.PlayVoiceOverClip (letsPlay);
        brainbowManager.foodPanel.Deactivate ();

        yield return new WaitForSeconds (0.5f);
        brainbowManager.monsterObject.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        brainbowManager.activeFoods.Clear ();
        yield return new WaitForSeconds (letsPlay.length + 0.5f);

        SubtitlePanel.Instance.Hide ();
        waterNotification.SetActive (true);
        AudioClip water = brainbowManager.voData.FindVO ("water");
        SoundManager.Instance.PlayVoiceOverClip (water);

        yield return new WaitForSeconds (water.length + 1f);
        waterNotification.GetComponent<Animator> ().SetBool ("Active", true);
        StartCoroutine (TutorialTearDown ());
    }

    IEnumerator TutorialTearDown () {
        StopCoroutine (tutorialCoroutine);
        yield return new WaitForSeconds (1f);
        StartCoroutine (TurnOffRainbows ());
        GameManager.Instance.CompleteTutorial (DataType.Minigame.Brainbow);
        brainbowManager.inputAllowed = false;
        isRunningTutorial = false;
        brainbowManager.foodPanel.GetComponent<Outline> ().enabled = false;
        brainbowManager.foodPanel.Deactivate ();
        SubtitlePanel.Instance.Hide ();
        brainbowManager.monsterObject.ChangeEmotions (DataType.MonsterEmotions.Happy);
        yield return new WaitForSeconds (1.25f);

        instructionPopup.gameObject.SetActive (false);
        BrainbowGameManager.Instance.StartGame ();
    }

    public void SkipTutorialButton (GameObject button) {
        SkipTutorial ();
        Destroy (button);
    }

    public void SkipTutorial () {
        if (tutorialBanana)
            Destroy (tutorialBanana);
        StopCoroutine (tutorialCoroutine);
        StartCoroutine (TutorialTearDown ());
    }

    IEnumerator TurnOnRainbows () {
        brainbowManager.ShowRainbowStripe (0, true);
        yield return new WaitForSeconds (0.25f);
        CreateFoodItem (tutorialApplePrefab, brainbowManager.stripes[0]);
        brainbowManager.ShowRainbowStripe (1, true);
        yield return new WaitForSeconds (0.25f);
        brainbowManager.ShowRainbowStripe (2, true);
        yield return new WaitForSeconds (0.25f);
        CreateFoodItem (tutorialBroccoliPrefab, brainbowManager.stripes[2]);
        brainbowManager.ShowRainbowStripe (3, true);
        yield return new WaitForSeconds (0.25f);
        CreateFoodItem (tutorialEggplantPrefab, brainbowManager.stripes[3]);
    }

    IEnumerator TurnOffRainbows () {
        brainbowManager.ShowRainbowStripe (3, false);
        yield return new WaitForSeconds (0.25f);
        brainbowManager.ShowRainbowStripe (2, false);
        yield return new WaitForSeconds (0.25f);
        brainbowManager.ShowRainbowStripe (1, false);
        yield return new WaitForSeconds (0.25f);
        brainbowManager.ShowRainbowStripe (0, false);
        yield return new WaitForSeconds (0.25f);
    }

    void CreateFoodItem(GameObject prefab, BrainbowStripe stripe) {
        BrainbowFoodItem newFood = Instantiate (prefab).AddComponent<BrainbowFoodItem>();
        newFood.InsertItemIntoStripe (stripe);
        newFood.gameObject.transform.localScale = Vector3.one * brainbowManager.foodScale;
        newFood.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 3;
    }

    void CreateBanana() {
        tutorialBanana = brainbowManager.foodPanel.CreateItemAtSlot (tutorialBananaPrefab, brainbowManager.foodPanel.slots[0]);
        tutorialBanana.name = "Banana";
        tutorialBanana.AddComponent<BrainbowFoodItem> ();
        tutorialBanana.GetComponent<Food> ().Spawn (brainbowManager.foodPanel.slots[0], brainbowManager.foodPanel.slots[0], brainbowManager.foodScale);
    }

}