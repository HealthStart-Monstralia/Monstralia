using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : MonoBehaviour {

    // Track milestone unlock status with their name
    private Dictionary<string, bool> unlockedMilestone;

    private static MilestoneManager instance = null;

    void Awake () {
        // Singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (this);
    }

    private void Start () {
        // Initialize dictionary
        unlockedMilestone = new Dictionary<string, bool> ();

    }

    public static MilestoneManager GetInstance () {
        return instance;
    }

    // Add a milestone name to the dictionary and set the boolean value to false.
    public void AddMilestone(string mileName) {
        unlockedMilestone.Add (mileName, false);
    }

    // Retrieve the boolean value from a string key
    public bool GetUnlockedStatus(string mileName) {
        if (ContainsMilestone (mileName))
            return false;
        return unlockedMilestone[mileName];
    }

    // Check if a milestone key exists in the dictionary
    public bool ContainsMilestone(string mileName) {
        return unlockedMilestone.ContainsKey (mileName);
    }

    // Set the boolean value for a string key
    public void SetUnlockedStatus (string mileName, bool status) {
        if (ContainsMilestone (mileName))
            unlockedMilestone[mileName] = status;
    }

    // Create milestone notification and unlock the corresponding milestone
    // Use this for gameplay rather than SetUnlockedStatus
    public void UnlockMilestone(string mileName) {
        // Create notification code here
        SetUnlockedStatus (mileName, true);
    }
}