using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSaveSystem : Singleton<DeleteSaveSystem> {
    public Prompt currentPrompt;
    [SerializeField] private Prompt deleteSavePrompt;
    [SerializeField] private Prompt restartPrompt;
    public static bool deleteAndRestart = false;

    private Animator anim;
    private bool isRestarting = false;
    private bool isTransitioning = false;
    private bool saveDeleted = false;

    private new void Awake () {
        base.Awake ();
        anim = GetComponent<Animator> ();
        currentPrompt = deleteSavePrompt;
        deleteSavePrompt.gameObject.SetActive (true);
        restartPrompt.gameObject.SetActive (false);
    }

    public void DisableFromAnim () {
        Destroy (gameObject);
    }

    public void CallDeleteSave () {
        GameManager.Instance.DeleteSave ();
        saveDeleted = true;
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            CloseNotification ();
        }
    }

    public void OnDeleteSaveYes () {
        CallDeleteSave ();
        currentPrompt = restartPrompt;
        restartPrompt.gameObject.SetActive (true);
        deleteSavePrompt.gameObject.SetActive (false);
    }

    public void OnDeleteSaveNo () {
        isRestarting = false;
        StartCoroutine (CloseNotification ());
    }

    public void OnRestartYes () {
        isRestarting = true;
        if (saveDeleted) {
            deleteAndRestart = true;
        }

        StartCoroutine (CloseNotification ());
    }

    public void OnRestartNo () {
        isRestarting = false;
        StartCoroutine (CloseNotification ());
    }

    IEnumerator CloseNotification () {
        print ("Close");
        if (!isTransitioning) {
            isTransitioning = true;
            currentPrompt.OnClose ();

            if (isRestarting) {
                RestartGame ();
                if (anim)
                    GetComponent<Animator> ().Play ("PopupFadeOut", -1, 0f);
            }
            else {
                Destroy (gameObject);
            }
            yield return new WaitForSeconds (0.5f);
            isTransitioning = false;
        }
    }

    private void RestartGame () {
        Destroy (GameManager.Instance.gameObject);
        Destroy (SoundManager.Instance.gameObject);
        Destroy (ReviewManager.Instance.gameObject);
        StartCoroutine (WaitToRestart ());
    }

    IEnumerator WaitToRestart () {
        yield return new WaitForSeconds (0.1f);
        gameObject.AddComponent<SwitchScene> ().LoadScene ("Start");
    }
}
