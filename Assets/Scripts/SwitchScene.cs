using UnityEngine;
using System.Collections;

public class SwitchScene: MonoBehaviour {

	public string sceneToLoad;
	public GameObject loadingScreen;

	public void loadScene() {
		loadingScreen.SetActive(true);
		Application.LoadLevel(sceneToLoad);
	}

	public void loadScene(string name) {
		loadingScreen.SetActive(true);
		Application.LoadLevel(name);
	}
}
