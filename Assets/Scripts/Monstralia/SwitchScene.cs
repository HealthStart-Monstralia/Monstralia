using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchScene: MonoBehaviour {
	public string sceneToLoadName;

    public void LoadScene() {
        if (SoundManager.Instance) {
            SoundManager.Instance.StopAmbientSound ();
            SoundManager.Instance.StopPlayingVoiceOver ();
        }

        if (GameManager.Instance) {
            GameManager.Instance.CreateLoadingScreen ();
        }

        if (sceneToLoadName != "")
            SceneManager.LoadScene (sceneToLoadName);
        else {
            Debug.LogError ("CUSTOM ERROR: No scene defined in " + gameObject);
            SceneManager.LoadScene ("MainMap");
        }
    }

	public void LoadScene(string name) {
        sceneToLoadName = name;
        LoadScene ();
	}

    public void LoadScene (Scene scene) {
        if (GameManager.Instance) {
            GameManager.Instance.CreateLoadingScreen ();
        }
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
        if (GameManager.Instance) {
            GameManager.Instance.CreateLoadingScreen ();
        }
        SceneManager.LoadScene (SceneManager.GetActiveScene().name);
    }
}
