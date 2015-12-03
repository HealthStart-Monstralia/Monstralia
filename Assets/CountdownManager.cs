using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour {

	private static CountdownManager instance;

	public GameObject[] countdown;
	
	void Awake () {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}
	}

	public static CountdownManager GetInstance() {
		return instance;
	}
	
	// Update is called once per frame
	public void BeginCountdown () {
		for(int i = 0; i < countdown.Length; ++i) {
			print("Translating: " + i);{
			countdown[i].transform.Translate(new Vector3(-8f ,countdown[i].transform.position.y, countdown[i].transform.position.z));
			}
		}
	}
}
