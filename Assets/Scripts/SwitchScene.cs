using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchScene: MonoBehaviour {

	public string sceneToLoad;
	public GameObject loadingScreen;

	public void loadScene() {
		loadingScreen.SetActive(true);
		SceneManager.LoadScene (sceneToLoad);
		//Application.LoadLevel(sceneToLoad);
	}

	public void loadScene(string name) {
		if(loadingScreen)
			loadingScreen.SetActive(true);
		SceneManager.LoadScene (name);
	}
}
