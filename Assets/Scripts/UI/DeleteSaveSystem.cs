using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSaveSystem : Singleton<DeleteSaveSystem> {
    public Prompt currentPrompt;

    [SerializeField] private Prompt deleteSavePrompt;
    [SerializeField] private Prompt restartPrompt;

    private Animator anim;
    private bool isRestarting = false;
    private bool isTransitioning = false;

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
            if (anim)
                GetComponent<Animator> ().Play ("PopupFadeOut", -1, 0f);

            currentPrompt.OnClose ();
            yield return new WaitForSeconds (0.5f);

            if (isRestarting) {
                RestartGame ();
            }
            else {
                Destroy (gameObject);
            }

            isTransitioning = false;
        }
    }

    private void RestartGame () {
        Destroy (GameManager.Instance.gameObject);
        Destroy (SoundManager.Instance.gameObject);
        Destroy (ReviewManager.Instance.gameObject);
        gameObject.AddComponent<SwitchScene> ().LoadScene ("Start");
    }
}
