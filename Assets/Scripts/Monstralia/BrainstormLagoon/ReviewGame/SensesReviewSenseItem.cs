using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesReviewSenseItem : MonoBehaviour {

    // Borrowed from InteractableObject_LJ.cs
    public enum Senses {
        //enumerates to elements 0, 1, 2, 3, 4
        see, hear, smell, taste, touch
    }
    public Senses[] assignSenses;

}
