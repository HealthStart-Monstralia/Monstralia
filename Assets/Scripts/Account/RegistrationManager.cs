using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour {
    [Header ("Email Components")]
    public Text emailValidText;
    public InputField emailInput;
    public string emailIsEmpty = "Email is empty.";
    public string emailNoAt = "Email does not have an @";
    public string emailNoPeriod = "Email does not have a period.";

    public Text reEmailValidText;
    public InputField reemailInput;
    public string emailDoesNotMatch = "Emails do not match.";

    [Header ("Name Components")]
    public Text firstNameValidText;
    public Text lastNameValidText;
    public InputField firstNameInput;
    public InputField lastNameInput;
    public string firstNameIsEmpty = "First Name is empty.";
    public string lastNameIsEmpty = "Last Name is empty.";

    //custom URL for database
    private string CreateUserURL = "https://monstraliatestdb.000webhostapp.com/PHPdefault.php";

	//strings to convert what the user puts into the fields (.txt)
    private string phpUserEmail;
	private string phpUserFirstName;
	private string phpUserLastName;

    private void Awake () {
        emailValidText.gameObject.SetActive (false);
        reEmailValidText.gameObject.SetActive (false);
        firstNameValidText.gameObject.SetActive (false);
        lastNameValidText.gameObject.SetActive (false);
    }

    // Use with a button event
    public void OnSubmitVerifyInformation () {
        if (VerifyInformation()) {
            SubmitInformation ();
        }
    }

    // Use with On End Edit event in input fields
    public void OnEditVerifyEmail () { VerifyEmail (); }
    public void OnEditVerifyReEmail () { VerifyReEmail (); }
    public void OnEditVerifyFirstName () { VerifyFirstName (); }
    public void OnEditVerifyLastName () { VerifyLastName (); }

    // Verify that all information is valid before sending to server.
    private bool VerifyInformation () {
        bool isEmailValid = VerifyEmail () && VerifyReEmail ();
        bool areNamesValid = VerifyFirstName () && VerifyLastName ();
        return isEmailValid && areNamesValid;
    }

    // Check if first name field is empty
    private bool VerifyFirstName () {
        return VerifyName (firstNameInput.text, firstNameIsEmpty, firstNameValidText);
    }

    // Check if last name field is empty
    private bool VerifyLastName () {
        return VerifyName (lastNameInput.text, lastNameIsEmpty, lastNameValidText);
    }

    private bool VerifyName (string textToCheck, string messageToDisplay, Text validTextObject) {
        // Check if name field is empty
        if (IsTextEmpty (textToCheck)) {
            DisplayError (messageToDisplay, validTextObject);

            // If empty return false
            return false;
        }

        // If not empty, then hide it and return true
        HideError (validTextObject);
        return true;
    }

    private bool VerifyEmail () {
        // Check if text is empty
        if (IsTextEmpty (emailInput.text)) {
            DisplayError (emailIsEmpty, emailValidText);
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

        // Email input is valid therefore, hide email error message and return true to VerifyInformation ().
        HideError (emailValidText);

        //grabs the information that was put into the fields and assigned to a string to send to the PHP 'CreateUser' function down below
        phpUserEmail = emailInput.text;
        phpUserFirstName = firstNameInput.text;
		phpUserLastName = lastNameInput.text;
        return true;
    }

    private bool VerifyReEmail () {
        // Check if confirmation matches
        if (!IsTextMatching (emailInput.text, reemailInput.text)) {
            DisplayError (emailDoesNotMatch, reEmailValidText);
            return false;
        }

        // Email input is valid therefore, hide email error message and return true to VerifyInformation ().
        HideError (reEmailValidText);
        return true;
    }

    private void SubmitInformation () {
        /* Send information to web server
         * Send Email - emailInput.text
         * Send Password - passInput.text
         * Send Opt-in - checkmark.isOn
         */

        //Our function being called in order to send the information to our PHP script (written down below)
        WWW www = CreateUser (phpUserEmail, phpUserFirstName, phpUserLastName);

        // Upon completion show success page if no error has occured
        if (string.IsNullOrEmpty (www.error))
            IntroManager.Instance.ShowPage (IntroManager.Instance.signedUpPage);
        else
            Debug.LogError ("WWW returned an error: " + www.error);

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

    //-------------------------------------------------------------------------------------------------
    //creates a WWW form to communicate with the PHP script using strings (name, email, survey questions)
	public WWW CreateUser(string email, string FirstName, string LastName)
    {
        //creating new form to talk to PHP
        WWWForm form = new WWWForm();
        //fields that will be submitted
        //_____POST is a variable we are looking for within our PHP script
        form.AddField("FirstNamePOST", FirstName);
        form.AddField("emailPOST", email);
		form.AddField("LastNamePOST", LastName);

        //This submits our form to the PHP script
        WWW _www = new WWW(CreateUserURL, form);
        return _www;
    }

//final bracket
}
