using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonGraphic : MonoBehaviour {
    public Sprite onSprite, offSprite;
    public bool IsOn {
        set {
            _isOn = value;
            if (_isOn) { GetComponent<Image> ().sprite = onSprite; }
            else { GetComponent<Image> ().sprite = offSprite; }
        }

        get {
            return _isOn;
        }
    }

    private bool _isOn;

    public void ToggleSprite() {
        IsOn = !IsOn;
    }
}
