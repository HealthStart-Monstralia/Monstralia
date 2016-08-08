using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

	public Animator animToPlay;

	// Use this for initialization
	void Start () {
		animToPlay.Play("Countdown2Animation");
	}

}
