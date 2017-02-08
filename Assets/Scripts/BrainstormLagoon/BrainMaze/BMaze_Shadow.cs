using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Shadow : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameObject objectToFollow;

	void Start () {
		transform.position = objectToFollow.transform.position;
	}
	
	void Update () {
		transform.position = objectToFollow.transform.position;
	}
}
