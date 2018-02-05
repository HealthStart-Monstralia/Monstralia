using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour {
    public Text emailValidText, passValidText;
    public InputField emailInput, reemailInput, passInput, repassInput;
    public Toggle checkmark;

    public string emailDoesNotMatch = "Emails do not match.";
    public string passDoesNotMatch = "Passwords do not match.";
    public string emailIsEmpty = "Email is empty.";
    public string passIsEmpty = "Password is empty.";
    public string emailNoAt = "Email does not have an @";
    public string emailNoPeriod = "Email does not have a period.";

    // Use with a button event
    public void OnSubmitVerifyInformation () {
        if (VerifyInformation()) {
            SubmitInformation ();
        }
    }

    // Use with On End Edit event in input fields
    public void OnEditVerifyEmail () { VerifyEmail (); }
    public void OnEditVerifyPassword () { VerifyPassword (); }

    private bool VerifyInformation () {
        bool isEmailValid = VerifyEmail ();
        bool isPasswordValid = VerifyPassword ();
        return isEmailValid && isPasswordValid;
    }

    private bool VerifyPassword () {
        // Check if password is empty
        if (IsTextEmpty (passInput.text)) {
            DisplayError (passIsEmpty, passValidText);
            return false;
        }

        // Check if confirmation matches
        if (!IsTextMatching (passInput.text, repassInput.text)) {
            DisplayError (passDoesNotMatch, passValidText);
            return false;
        }

        HideError (passValidText);
        return true;
    }

    private bool VerifyEmail () {
        // Check if text is empty
        if (IsTextEmpty (emailInput.text)) {
            DisplayError (emailIsEmpty, emailValidText);
            return false;
        }

        // Check if confirmation matches
        if (!IsTextMatching(emailInput.text, reemailInput.text)) {
            DisplayError (emailDoesNotMatch, emailValidText);
            return false;
        }

        // Check if there is an @
        if (!emailInput.text.Contains ("@")) {
            DisplayError (emailNoAt, emailValidText);
            return false;
        }
        
        // Check if there is a period
        if (!emailInput.text.Contains (".")) {
            DisplayError (emailNoPeriod, emailValidText);
            return false;
        }

        HideError (emailValidText);
        return true;
    }

    private void SubmitInformation () {
        /* Send information to web server
         * Send Email - emailInput.text
         * Send Password - passInput.text
         * Send Opt-in - checkmark.isOn
         */

        // Verify if operation was successful
        // Encode password for security

        // Upon completion show success page
        IntroManager.Instance.ShowPage (IntroManager.Instance.signedUpPage);

        // If not successful show not successful page, (NOT IMPLEMENTED YET)
    }

    private bool IsTextEmpty (string textToCheck) {
        return textToCheck == "" ? true : false;
    }

    private bool IsTextMatching (string textToCheck, string textToCheckOther) {
        return textToCheck == textToCheckOther ? true : false;
    }

    private bool DisplayError (string textToUse, Text messageTextObject) {
        messageTextObject.text = textToUse;
        messageTextObject.gameObject.SetActive (true);
        return false;
    }

    private void HideError (Text messageTextObject) {
        messageTextObject.text = "";
        messageTextObject.gameObject.SetActive (false);
    }
}
