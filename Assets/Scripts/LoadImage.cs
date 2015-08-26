using UnityEngine;
using System.Collections;

public class LoadImage : MonoBehaviour {

	public TextAsset imageAsset;
	public GameObject monster;

	public void loadImageFromFile () {
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(imageAsset.bytes);
		GetComponent<Renderer>().material.mainTexture = tex;
	}

}
