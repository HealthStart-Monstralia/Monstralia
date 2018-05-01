using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollection : MonoBehaviour 
{

	private string TimeSpent;
	public float curSeconds = 0;
	public float curMinutes = 0;
	public float curHours   = 0;

	public bool isPaused = false;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		curSeconds += Time.deltaTime;

		if (curSeconds >= 60) 
		{
			curSeconds = 0;
			curMinutes += 1;
		}

		if (curMinutes >= 60) 
		{
			curMinutes = 0;
			curHours += 1;
		}
	}


	void OnApplicationPause(bool pauseStatus)
	{
		print (pauseStatus);
	}

	void OnApplicationQuit()
	{
		TimeSpent = "You spent " + curHours.ToString() + " hours " + curMinutes.ToString() + " minutes " + curSeconds.ToString() + " seconds";
		print (TimeSpent);
	}


//final bracket-----------
}
