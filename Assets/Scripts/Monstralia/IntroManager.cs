using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour {
	public GameObject blockSlide;
	public GameObject[] aboutMonstraliaSlides;
	public GameObject signUpPage;
	public GameObject signedUpPage;
	public GameObject emailValid, passValid;
	public InputField emailInput, reemailInput, passInput, repassInput;
	public Toggle checkmark;
	public string emailDoesNotMatch = "Emails do not match.";
	public string passDoesNotMatch = "Passwords do not match.";
	public string emailIsEmpty = "Email is empty.";
	public string passIsEmpty = "Password is empty.";
	public string emailNoAt = "Email does not contain an @";
	public string emailNoPeriod = "Email does not have a period.";

	private static IntroManager instance = null;

	private int slideNum = 0;
	private bool emailOkay = false;
	private bool passOkay = false;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		for (int i = 0; i < aboutMonstraliaSlides.Length; i++) {
			if (aboutMonstraliaSlides [i].activeSelf)
				aboutMonstraliaSlides [i].SetActive (false);
		}
		blockSlide.SetActive (true);
		aboutMonstraliaSlides [0].SetActive (true);
	}

	void SubmitInformation() {
		signUpPage.SetActive (false);
		signedUpPage.SetActive (true);

		/* Send information to web server */
	}

	public void VerifyInformationOnSubmit() {
		VerifyInformation ();
		if (emailOkay && passOkay) {
			if (VerifyCharacters ()) {
				if (emailOkay && passOkay) {
					SubmitInformation ();
				}
			}
		}
	}

	public void VerifyInformationOnEdit() {

	}

	public void VerifyInformation() {
		if (emailInput.text != "") {
			if (emailInput.text != reemailInput.text) {
				emailValid.gameObject.SetActive (true);
				emailValid.GetComponent<Text> ().text = emailDoesNotMatch;
				emailOkay = false;
			} else {
				emailValid.gameObject.SetActive (false);
				emailOkay = true;
			}
		} else {
			emailValid.gameObject.SetActive (true);
			emailOkay = false;
			emailValid.GetComponent<Text> ().text = emailIsEmpty;
		}

		if (passInput.text != "") {
			if (passInput.text != repassInput.text) {
				passValid.gameObject.SetActive (true);
				passValid.GetComponent<Text> ().text = passDoesNotMatch;
				passOkay = false;
			} else {
				passValid.gameObject.SetActive (false);
				passOkay = true;
			}
		} else {
			passValid.gameObject.SetActive (true);
			passValid.GetComponent<Text> ().text = passIsEmpty;
			passOkay = false;
		}
	}

	bool VerifyCharacters() {
		bool passedVerification = true;
		if (emailInput.text.Contains ("@")) {
			if (emailInput.text.Contains (".")) {
				emailValid.gameObject.SetActive (false);
			} else {
				emailValid.gameObject.SetActive (true);
				emailValid.GetComponent<Text> ().text = emailNoPeriod;
				passedVerification = false;
			}
		} else {
			emailValid.GetComponent<Text> ().text = emailNoAt;
			passedVerification = false;
		}

		if (passInput.text == "") {
			passValid.gameObject.SetActive (true);
			passValid.GetComponent<Text> ().text = passIsEmpty;
		} else {
			passValid.gameObject.SetActive (false);
		}

		if (emailInput.text == "") {
			emailValid.gameObject.SetActive (true);
			emailValid.GetComponent<Text> ().text = emailIsEmpty;
		} else {
			emailValid.gameObject.SetActive (false);
		}

		return passedVerification;
	}
}
