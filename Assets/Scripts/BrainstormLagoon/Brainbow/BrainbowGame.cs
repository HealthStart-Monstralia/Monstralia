using UnityEngine;
using System.Collections;

public class BrainbowGame : MonoBehaviour {

	public GameObject instructionsCanvas;

	// Show demonstration.
	void Awake(){
		// print ("in Awake of Brainbow game");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// print ("in Update of Brainbow game");
		if (Time.timeSinceLevelLoad < 3.0f)
			print ("instructionsCanvas.name: " + instructionsCanvas.name);
	}
}
