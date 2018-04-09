using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationManager : MonoBehaviour {
    public Text emailValidText, passValidText;
    public InputField emailInput, reemailInput, passInput, repassInput;
    public Toggle checkmark;
	//
    public string emailDoesNotMatch = "Emails do not match.";
    public string passDoesNotMatch = "Names do not match.";
    public string emailIsEmpty = "Email is empty.";
    public string passIsEmpty = "Name is empty.";
    public string emailNoAt = "Email does not have an @";
    public string emailNoPeriod = "Email does not have a period.";


    //custom URL for database
    string CreateUserURL = "https://monstraliatestdb.000webhostapp.com/PHPdefault.php";

	//strings to convert what the user puts into the fields (.txt)
    private string phpUserEmail;
    private string phpUserPass;
	private string phpUserFirstName;
	private string phpUserLastName;
    

    private void Awake () {
        emailValidText.gameObject.SetActive (false);
        passValidText.gameObject.SetActive (false);

        
    }

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

//        // Check if confirmation matches
//        if (!IsTextMatching (passInput.text, repassInput.text)) {
//            DisplayError (passDoesNotMatch, passValidText);
//            return false;
//        }

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

        // Email input is valid therefore, hide email error message and return true to VerifyInformation ().
        HideError (emailValidText);
        
        //grabs the information that was put into the fields and assigned to a string to send to the PHP 'CreateUser' function down below
        phpUserEmail = emailInput.text;
        phpUserFirstName = passInput.text;
		phpUserLastName = repassInput.text;
        return true;
    }


    private void SubmitInformation () {
        /* Send information to web server
         * Send Email - emailInput.text
         * Send Password - passInput.text
         * Send Opt-in - checkmark.isOn
         */

        //Our function being called in order to send the information to our PHP script (written down below)
		CreateUser(phpUserEmail, phpUserFirstName, phpUserLastName);
        
        
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






    //-------------------------------------------------------------------------------------------------
    //creates a WWW form to communicate with the PHP script using strings (name, email, survey questions)
	public void CreateUser(string email, string FirstName, string LastName)
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
    }


//final bracket
}
