using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLButton : MonoBehaviour {
    public string URLToOpen;

    public void OnClick() {
        Application.OpenURL (URLToOpen);
    }
}
