using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMatchTutorialManager : MonoBehaviour {
    [SerializeField] private Transform handSpawn;
    [SerializeField] private Transform hand;
    [SerializeField] private DishObject[] tutorialDishes;
    [SerializeField] private GameObject tutorialBanana;
    [SerializeField] private GameObject tutorialMatchBanana;
    private Canvas tutorialCanvas;
    private Coroutine tutorialCoroutine;
    private VoiceOversData voData;          // Pull from MemoryMatchGameManager

    private void Awake () {
        tutorialCanvas = GetComponent<Canvas> ();
        tutorialCanvas.gameObject.SetActive (false);
    }

    public void StartTutorial(ref GameObject banana) {
        banana = tutorialBanana;
        voData = MemoryMatchGameManager.Instance.voData;
        tutorialCanvas.gameObject.SetActive (true);
        tutorialMatchBanana.gameObject.SetActive (false);
        tutorialCoroutine = StartCoroutine (RunTutorial ());
    }

    IEnumerator RunTutorial () {
        print ("RunTutorial");
        MemoryMatchGameManager.Instance.ActivateHUD (false);

        DishObject tutDish1 = tutorialDishes[0];
        DishObject tutDish2 = tutorialDishes[1];
        DishObject tutDish3 = tutorialDishes[2];

        GameObject tutFood1 = tutorialDishes[0].transform.Find ("Banana").gameObject;
        GameObject tutFood2 = tutorialDishes[1].transform.Find ("Raspberry").gameObject;
        GameObject tutFood3 = tutorialDishes[2].transform.Find ("Brocolli").gameObject;

        tutorialDishes[0].SetFood (tutFood1);
        tutorialDishes[1].SetFood (tutFood2);
        tutorialDishes[2].SetFood (tutFood3);

        tutFood1.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);
        tutFood2.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);
        tutFood3.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);
        yield return new WaitForSeconds (1f);

        AudioClip tutorial1 = voData.FindVO ("1_tutorial_welcome");
        AudioClip tutorial2a = voData.FindVO ("2a_tutorial_platters");
        AudioClip tutorial2b = voData.FindVO ("2b_tutorial_platters");

        SubtitlePanel.Instance.Display ("Your memory keeps you safe and healthy.", tutorial1, false, tutorial1.length);
        yield return new WaitForSeconds (tutorial1.length);

        SubtitlePanel.Instance.Display ("I'll show you some foods that are good for my brain-", tutorial2a, false, tutorial2a.length);
        yield return new WaitForSeconds (tutorial2a.length);

        SubtitlePanel.Instance.Display ("-hide them under the platters, and mix them up.", tutorial2b, false, tutorial2b.length);
        yield return new WaitForSeconds (tutorial2b.length);

        tutDish1.SpawnLids (true);
        yield return new WaitForSeconds (0.25f);
        tutDish2.SpawnLids (true);
        yield return new WaitForSeconds (0.25f);
        tutDish3.SpawnLids (true);

        yield return new WaitForSeconds (1.15f);

        // Dish lift.
        foreach (DishObject dish in tutorialDishes) {
            dish.OpenLid ();
        }

        AudioClip tutorial3a = voData.FindVO ("3a_tutorial_rememberfood");
        AudioClip tutorial3b = voData.FindVO ("3b_tutorial_rememberfood");
        AudioClip tutorial4 = voData.FindVO ("4_tutorial_letmeshow");

        SubtitlePanel.Instance.Display ("Remember what is under the platters.", tutorial3a, false, tutorial3a.length);
        yield return new WaitForSeconds (tutorial3a.length);

        SubtitlePanel.Instance.Display ("Pick the platter with the food that matches the food at the bottom.", tutorial3b, false, tutorial3b.length);
        yield return new WaitForSeconds (tutorial3b.length - 1.0f);

        tutorialMatchBanana.gameObject.SetActive (true);
        Vector3 originalScale = tutorialMatchBanana.transform.localScale;
        tutorialMatchBanana.transform.localScale = Vector3.zero;
        LeanTween.scale (tutorialMatchBanana, originalScale, 0.3f).setEaseOutBack ();
        yield return new WaitForSeconds (1.0f);

        // Dish close.
        foreach (DishObject dish in tutorialDishes) {
            dish.CloseLid ();
        }

        SubtitlePanel.Instance.Display ("Here! I'll show you with this banana!", tutorial4, false, tutorial4.length);
        yield return new WaitForSeconds (tutorial4.length);

        hand.gameObject.SetActive (true);
        originalScale = hand.transform.localScale;

        LeanTween.move (hand.gameObject, tutorialDishes[0].transform.position, 1.5f).setEaseInOutCubic ();
        yield return new WaitForSeconds (1.75f);

        LeanTween.scale (hand.gameObject, originalScale * 0.9f, 0.25f);
        yield return new WaitForSeconds (0.25f);

        LeanTween.scale (hand.gameObject, originalScale, 0.25f);
        SoundManager.Instance.PlayCorrectSFX ();
        tutorialDishes[0].OpenLid ();
        yield return new WaitForSeconds (0.5f);

        LeanTween.move (hand.gameObject, handSpawn.transform.position, 1f).setEaseInBack ();
        yield return new WaitForSeconds (1.25f);

        //tutorialHandAnim.Play ("mmhand_5_12");
        //yield return new WaitForSeconds (1.5f);

        // Hand taps on the left dish cover


        //yield return new WaitForSeconds (3f);

        tutorialDishes[0].CloseLid ();
        yield return new WaitForSeconds (0.5f);

        hand.gameObject.SetActive (false);
        MemoryMatchGameManager.Instance.inputAllowed = true;
        AudioClip tutorial5 = voData.FindVO ("5_tutorial_nowtry");
        SubtitlePanel.Instance.Display ("Now you try!", tutorial5);

        for (int i = 0; i < tutorialDishes.Length; ++i) {
            tutorialDishes[i].GetComponent<Collider2D> ().enabled = true;
        }
    }

    public void SkipTutorialButton (GameObject button) {
        StopCoroutine (tutorialCoroutine);
        hand.gameObject.SetActive (false);
        StartCoroutine (MemoryMatchGameManager.Instance.TurnOffTutorial ());
        MemoryMatchGameManager.Instance.inputAllowed = false;
        Destroy (button);
    }

    void FinishTutorial() {
        MemoryMatchGameManager.Instance.inputAllowed = false;
        MemoryMatchGameManager.Instance.OnTutorialFinish ();
    }
}
