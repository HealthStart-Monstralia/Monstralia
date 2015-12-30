using UnityEngine;
using System.Collections;

public class MemoryMatchFood : Food {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplaySubtitle() {
		gameObject.GetComponent<Subtitle>().Display(gameObject.transform.position);
	}
}
