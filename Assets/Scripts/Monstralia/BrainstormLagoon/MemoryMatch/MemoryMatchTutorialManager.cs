using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMatchTutorialManager : MonoBehaviour {
    private Canvas tutorialCanvas;
    private Coroutine tutorialCoroutine;
    [SerializeField] private DishObject[] tutorialDishes;
    [SerializeField] private GameObject tutorialBanana;
    [SerializeField] private GameObject tutorialMatchBanana;
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

        /*
		tutFood1.transform.localPosition = new Vector3 (0, 1.25f, 0);
		tutFood2.transform.localPosition = new Vector3 (0, 1.25f, 0);
		tutFood3.transform.localPosition = new Vector3 (0, 1.25f, 0);
        */

        tutFood1.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);
        tutFood2.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);
        tutFood3.transform.localScale = new Vector3 (0.65f, 0.65f, 1f);

        yield return new WaitForSeconds (1f);

        AudioClip tutorial1 = voData.FindVO ("1_tutorial_welcome");
        AudioClip tutorial2 = voData.FindVO ("2_tutorial_platters");
        SoundManager.Instance.PlayVoiceOverClip (tutorial1);

        yield return new WaitForSeconds (tutorial1.length);

        SoundManager.Instance.PlayVoiceOverClip (tutorial2);

        yield return new WaitForSeconds (tutorial2.length);

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

        AudioClip tutorial3 = voData.FindVO ("3_tutorial_rememberfood");
        AudioClip tutorial4 = voData.FindVO ("4_tutorial_letmeshow");
        SoundManager.Instance.PlayVoiceOverClip (tutorial3);

        yield return new WaitForSeconds (tutorial3.length - 1.5f);

        tutorialMatchBanana.gameObject.SetActive (true);

        yield return new WaitForSeconds (1.5f);

        // Dish close.
        foreach (DishObject dish in tutorialDishes) {
            dish.CloseLid ();
        }

        SoundManager.Instance.PlayVoiceOverClip (tutorial4);
        yield return new WaitForSeconds (tutorial4.length - 1f);

        Animator handAnim = tutorialCanvas.gameObject.transform.Find ("TutorialAnimation").gameObject.transform.Find ("Hand").gameObject.GetComponent<Animator> ();
        handAnim.Play ("mmhand_5_12");
        yield return new WaitForSeconds (4f);

        // Hand taps on the left dish cover
        SoundManager.Instance.PlayCorrectSFX ();
        tutorialDishes[0].OpenLid ();

        yield return new WaitForSeconds (2f);
        tutorialDishes[0].CloseLid ();

        handAnim.gameObject.SetActive (false);
        MemoryMatchGameManager.Instance.inputAllowed = true;
        AudioClip tutorial5 = voData.FindVO ("5_tutorial_nowtry");
        MemoryMatchGameManager.Instance.subtitlePanel.Display ("Now you try!", tutorial5);


        for (int i = 0; i < tutorialDishes.Length; ++i) {
            tutorialDishes[i].GetComponent<Collider2D> ().enabled = true;
        }
    }

    public void SkipTutorialButton (GameObject button) {
        SkipTutorial ();
        Destroy (button);
    }

    public void SkipTutorial () {
        StopCoroutine (tutorialCoroutine);
        FinishTutorial ();
    }

    void FinishTutorial() {
        MemoryMatchGameManager.Instance.inputAllowed = false;
        MemoryMatchGameManager.Instance.OnTutorialFinish ();
    }
}
