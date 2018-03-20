using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesItem : MonoBehaviour {
    public DataType.Senses[] validSenses;
    //public DataType.Senses[] negativeSenses;
    public AudioClip voiceOver;

    public DataType.Senses ChooseRandomSense () {
        return validSenses.GetRandomItem ();
    }
}
