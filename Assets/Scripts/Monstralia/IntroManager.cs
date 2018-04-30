using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroManager : Singleton<IntroManager> {
	public GameObject blockSlide;
	public GameObject[] aboutMonstraliaSlides = new GameObject[4];
	public GameObject signUpPage;
	public GameObject signedUpPage;
    public GameObject didntSignUpPage;

    public delegate void OnStartAction ();
    public delegate void OnEndAction ();
    public static event OnStartAction StartIntro;
    public static event OnEndAction EndIntro;

    private GameObject[] pages = new GameObject[7];

	new void Awake() {
        base.Awake ();

		for (int i = 0; i < aboutMonstraliaSlides.Length; i++) {
			if (aboutMonstraliaSlides [i].activeSelf)
				aboutMonstraliaSlides [i].SetActive (false);
		}
		blockSlide.SetActive (true);
		aboutMonstraliaSlides [0].SetActive (true);

        int count = 0;
        foreach (GameObject slide in aboutMonstraliaSlides) {
            pages[count] = slide;
            count++;
        }

        pages[count] = signUpPage;
        pages[count + 1] = signedUpPage;
        pages[count + 2] = didntSignUpPage;
    }

    private void OnEnable () {
        if (StartIntro != null) {
            StartIntro ();
        }
    }

    private void OnDisable () {
        if (EndIntro != null) {
            EndIntro ();
        }
    }

    public void ShowPage(GameObject selectedPage) {
        foreach (GameObject page in pages) {
            if (page.activeSelf && page != selectedPage) page.SetActive (false);
            else if (page == selectedPage) page.SetActive (true);
        }
    }

    public void ExitIntro(GameObject selectedPage) {
        foreach (GameObject page in pages) {
            if (page.activeSelf && page != selectedPage) page.SetActive (false);
            else if (page == selectedPage) page.SetActive (true);
            GetComponent<Animator> ().Play ("PopupFadeOut");
        }
    }

    // Disable using animation from PopupFade
    public void DisableFromAnim() {
        gameObject.SetActive (false);
    }

}
