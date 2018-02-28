using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesTutorialManager : MonoBehaviour {
    public bool isRunningTutorial = false;
    public Coroutine tutorialCoroutine;
    public Canvas instructionPopup;

    private SensesGameManager sensesManager;
    private VoiceOversData voData;

    private void Awake () {
        instructionPopup.gameObject.SetActive (false);
        voData = SensesGameManager.Instance.voData;
    }

    public void StartTutorial () {
        if (!SensesGameManager.Instance.hasGameStarted) {
            sensesManager = SensesGameManager.Instance;
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        }
    }

    IEnumerator RunTutorial () {
        isRunningTutorial = true;
        sensesManager.IsInputAllowed = false;
        instructionPopup.gameObject.SetActive (true);
        yield return new WaitForSeconds (1.0f);

        AudioClip tutorial1Clip = voData.FindVO ("1_tutorial_start");
        SoundManager.Instance.PlayVoiceOverClip (tutorial1Clip);
        yield return new WaitForSeconds (tutorial1Clip.length);

        AudioClip tutorial2Clip = voData.FindVO ("2_use_senses");
        SoundManager.Instance.PlayVoiceOverClip (tutorial2Clip);
        yield return new WaitForSeconds (tutorial2Clip.length);

        AudioClip tutorial3Clip = voData.FindVO ("3_taste");
        SoundManager.Instance.PlayVoiceOverClip (tutorial3Clip);
        yield return new WaitForSeconds (tutorial3Clip.length);

        AudioClip tutorial4Clip = voData.FindVO ("4_hear");
        SoundManager.Instance.PlayVoiceOverClip (tutorial4Clip);
        yield return new WaitForSeconds (tutorial4Clip.length);

        AudioClip tutorial5Clip = voData.FindVO ("5_see");
        SoundManager.Instance.PlayVoiceOverClip (tutorial5Clip);
        yield return new WaitForSeconds (tutorial5Clip.length);

        AudioClip tutorial6Clip = voData.FindVO ("6_smell");
        SoundManager.Instance.PlayVoiceOverClip (tutorial6Clip);
        yield return new WaitForSeconds (tutorial6Clip.length);

        if (isRunningTutorial) {
            StartCoroutine (TutorialEnd ());
        }
    }

    public IEnumerator TutorialEnd () {
        StopCoroutine (tutorialCoroutine);
        sensesManager.IsInputAllowed = false;
        AudioClip letsPlay = voData.FindVO ("letsplay");
        SubtitlePanel.Instance.Display ("Perfect! Let's play!");
        SoundManager.Instance.PlayVoiceOverClip (letsPlay);

        yield return new WaitForSeconds (0.5f);
        if (sensesManager.playerMonster)
            sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        yield return new WaitForSeconds (letsPlay.length - 0.5f);
        StartCoroutine (TutorialTearDown ());
    }

    IEnumerator TutorialTearDown () {
        StopCoroutine (tutorialCoroutine);
        GameManager.Instance.CompleteTutorial (DataType.Minigame.MonsterSenses);
        yield return new WaitForSeconds (0.5f);

        sensesManager.IsInputAllowed = false;
        isRunningTutorial = false;
        SubtitlePanel.Instance.Hide ();
        if (sensesManager.playerMonster)
            sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        yield return new WaitForSeconds (1.25f);

        instructionPopup.gameObject.SetActive (false);
        SensesGameManager.Instance.StartLevel ();
    }

    public void SkipTutorialButton (GameObject button) {
        SkipTutorial ();
        Destroy (button);
    }

    public void SkipTutorial () {
        StopCoroutine (tutorialCoroutine);
        StartCoroutine (TutorialTearDown ());
    }
}
