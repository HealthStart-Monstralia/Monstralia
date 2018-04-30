using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchToxinsHand : MonoBehaviour {
    public GameObject objectToGrab;

    private Transform previousParent;

    public void AttachToHand() {
        previousParent = objectToGrab.transform.parent;
        objectToGrab.transform.SetParent (gameObject.transform);
    }

    public void DetachFromHand () {
        objectToGrab.transform.SetParent (previousParent);
    }
}
