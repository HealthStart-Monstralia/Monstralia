using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMaze_Shadow : MonoBehaviour {
	/* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */

	public GameObject objectToFollow;
	public float offset;

	void Start () {
		transform.position = new Vector3 (
			objectToFollow.transform.position.x, 
			objectToFollow.transform.position.y - offset, 
			objectToFollow.transform.position.z);
	}
	
	void FixedUpdate () {
		transform.position = new Vector3 (
			objectToFollow.transform.position.x, 
			objectToFollow.transform.position.y - offset, 
			objectToFollow.transform.position.z);
	}
}
