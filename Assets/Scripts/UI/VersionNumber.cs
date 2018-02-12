using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionNumber : MonoBehaviour {
    private Text textField;

    private void Awake () {
        textField = GetComponent<Text> ();
        if (textField)
            textField.text = "v" + Application.version;
    }

    public void OnClick() {
        GameManager.Instance.ToggleFPSDisplay ();
    }
}
