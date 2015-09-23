using UnityEngine;
using System.Collections;

public abstract class Food : Colorable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Spawn(Transform spawnPos, Transform parent, float scale) {
		if(parent != null) {
			gameObject.transform.SetParent (parent.transform);
		}
		gameObject.transform.localPosition = spawnPos.localPosition;
		gameObject.transform.localScale = new Vector3(scale, scale, 1);
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
	}
}
