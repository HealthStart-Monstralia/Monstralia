using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneManager : SingletonPersistent<MilestoneManager>
{

    // Track milestone unlock status with their name

    public Dictionary<DataType.Milestone, bool> unlockedMilestone;

    private void Start () {
        unlockedMilestone = new Dictionary<DataType.Milestone, bool> ();
    }

    // Add a milestone name to the dictionary and set the boolean value to false.
    public void AddMilestone (DataType.Milestone mileName) {
        unlockedMilestone.Add (mileName, false);
    }

    // Retrieve the boolean value from a string key
    public bool GetUnlockedStatus (DataType.Milestone mileName) {
        if (ContainsMilestone (mileName))
            return false;
        return unlockedMilestone[mileName];
    }

    // Check if a milestone key exists in the dictionary
    public bool ContainsMilestone (DataType.Milestone mileName) {
        return unlockedMilestone.ContainsKey (mileName);
    }

    // Set the boolean value for a string key
    public void SetUnlockedStatus (DataType.Milestone mileName, bool status) {
        if (ContainsMilestone (mileName))
            unlockedMilestone[mileName] = status;
    }

    // Create milestone notification and unlock the corresponding milestone
    // Use this for gameplay rather than SetUnlockedStatus
    public void UnlockMilestone (DataType.Milestone mileName) {
        // Create notification code here
        SetUnlockedStatus(mileName, true);
    }
}