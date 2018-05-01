using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Based on Brackeys YouTube tutorial: https://youtu.be/YMj2qPq9CP8 */

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;

    public void LoadAsyncLevel (int sceneIndex) {
        StartCoroutine (LoadAsynchronously (sceneIndex));
    }

    public void LoadAsyncLevel (string sceneName) {
        StartCoroutine (LoadAsynchronously (SceneManager.GetSceneByName (sceneName).buildIndex));
    }

    public IEnumerator LoadAsynchronously (int sceneIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync (sceneIndex);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01 (operation.progress / 0.9f);

            if (slider)
                slider.value = progress;
            if (progressText)
                progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

    }
}
