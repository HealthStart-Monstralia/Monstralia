using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneContainer : MonoBehaviour {

    public MilestoneManager milestoneManager;
    public DataType.Milestone milestoneType;
    public GameObject milestoneIcon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnEnable()
    {
        milestoneManager = MilestoneManager.Instance;
        if (milestoneManager.GetUnlockedStatus(milestoneType))
        {
            milestoneIcon.SetActive(true);
        }
    }
}
