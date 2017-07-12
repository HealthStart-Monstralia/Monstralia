using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRestrict : MonoBehaviour {

    Vector3 myPos;
    public float upperBound, lowerBound;

	void Update () {
        myPos = transform.position; // get current pos
        myPos.y = Mathf.Clamp(myPos.y, lowerBound, upperBound); // clamp pos
        transform.position = myPos; // assign clamped pos to current pos
        // print(transform.position.y);
    }
}
