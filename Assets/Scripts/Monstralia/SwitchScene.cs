using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchScene: MonoBehaviour {
	public string sceneToLoadName;
    public GameObject loadingScreen;

    public void LoadScene() {
        if (SoundManager.Instance)
            SoundManager.Instance.StopAmbientSound ();

        if (loadingScreen) {
            Instantiate (loadingScreen, transform.root);
        }
        if (GameManager.Instance) {
            loadingScreen = GameManager.Instance.loadingScreenPrefab;
            Instantiate (loadingScreen, transform.root);
        }

        if (sceneToLoadName != "")
            SceneManager.LoadScene (sceneToLoadName);
        else {
            Debug.LogError ("CUSTOM ERROR: No scene defined in " + gameObject);
            SceneManager.LoadScene ("MainMap");
        }
    }

	public void LoadScene(string name) {
        if (loadingScreen)
            Instantiate (loadingScreen, transform.root);
		SceneManager.LoadScene (name);
	}

    public void LoadScene (Scene scene) {
        if (loadingScreen)
            Instantiate (loadingScreen, transform.root);
        SceneManager.LoadScene (scene.name);
    }

    public void LoadSceneNoScreen (string name) {
        SceneManager.LoadScene (name);
    }

    public void LoadSceneNoScreen (Scene scene) {
        SceneManager.LoadScene (scene.name);
    }

    public void LoadIslandSection () {
        if (GameManager.Instance) {
            DataType.IslandSection island = GameManager.Instance.GetIslandSection ();
            switch (island) {
                case DataType.IslandSection.Monstralia:
                    LoadScene ("MainMap");
                    break;
                case DataType.IslandSection.BrainstormLagoon:
                    LoadScene ("BrainstormLagoon");
                    break;
                case DataType.IslandSection.MainframeMountain:
                    LoadScene ("MainframeMountain");
                    break;
                default:
                    LoadScene (island.ToString ());
                    break;
            }
            
        }
    }

    public void LoadIslandSection (DataType.IslandSection island) {
        switch (island) {
            case DataType.IslandSection.Monstralia:
                LoadScene ("MainMap");
                break;
            case DataType.IslandSection.BrainstormLagoon:
                LoadScene ("BrainstormLagoon");
                break;
            case DataType.IslandSection.MainframeMountain:
                LoadScene ("MainframeMountain");
                break;
            default:
                LoadScene (island.ToString ());
                break;
        }
    }

    public void ReloadScene () {
        if (loadingScreen)
            Instantiate (loadingScreen, transform.root);
        SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }
}
