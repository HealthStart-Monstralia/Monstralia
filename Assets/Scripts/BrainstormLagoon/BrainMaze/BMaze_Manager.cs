using UnityEngine;
using System.Collections;

public class BMaze_Manager : MonoBehaviour {
	public AudioClip backgroundMusic;

	// Use this for initialization
	void Start () {
		SoundManager.GetInstance().ChangeBackgroundMusic(backgroundMusic);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
