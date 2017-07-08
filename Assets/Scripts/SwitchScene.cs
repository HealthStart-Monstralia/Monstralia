using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchScene: MonoBehaviour {

	public string sceneToLoad;
	public GameObject loadingScreen;

	public void loadScene() {
		loadingScreen.SetActive(true);
		SceneManager.LoadScene (sceneToLoad);
	}

	public void loadScene(string name) {
		Instantiate (loadingScreen, transform.root);
		SceneManager.LoadScene (name);
	}
}
