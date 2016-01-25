using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	
	public GameObject singleton;

	void Awake() { 

		if(GameManager.GetInstance() == null) {
			Instantiate(singleton);
		}

	}
}
