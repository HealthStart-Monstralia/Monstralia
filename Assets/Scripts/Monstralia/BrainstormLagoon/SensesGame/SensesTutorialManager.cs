using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesTutorialManager : MonoBehaviour {
    public bool isRunningTutorial = false;
    public Coroutine tutorialCoroutine;
    public GameObject instructionPopup;

    [SerializeField] private GameObject see;
    [SerializeField] private GameObject hear;
    [SerializeField] private GameObject touch;
    [SerializeField] private GameObject smell;
    [SerializeField] private GameObject taste;

    [SerializeField] private SensesFactory senseFactory;
    [SerializeField] private Text senseText;
    [SerializeField] private string[] correctLines;
    [SerializeField] private string[] wrongLines;

    private SensesGameManager sensesManager;
    private VoiceOversData voData;
    private Monster monster;
    private GameObject selectedObject;
    private DataType.Senses selectedSense;

    private void Awake () {
        instructionPopup.SetActive (false);
        voData = SensesGameManager.Instance.voData;
    }

    public void StartTutorial () {
        if (!SensesGameManager.Instance.hasGameStarted) {
            sensesManager = SensesGameManager.Instance;
            tutorialCoroutine = StartCoroutine (RunTutorial ());
        }
    }

    IEnumerator RunTutorial () {
        see.transform.localScale = Vector3.zero;
        hear.transform.localScale = Vector3.zero;
        touch.transform.localScale = Vector3.zero;
        smell.transform.localScale = Vector3.zero;
        taste.transform.localScale = Vector3.zero;

        monster = SensesGameManager.Instance.playerMonster;
        isRunningTutorial = true;
        sensesManager.IsInputAllowed = false;
        instructionPopup.SetActive (true);
        yield return new WaitForSeconds (1.0f);

        AudioClip tutorial1Clip = voData.FindVO ("1_tutorial_start");
        SoundManager.Instance.PlayVoiceOverClip (tutorial1Clip);
        yield return new WaitForSeconds (tutorial1Clip.length);

        AudioClip tutorial2Clip = voData.FindVO ("2_use_senses");
        SoundManager.Instance.PlayVoiceOverClip (tutorial2Clip);
        yield return new WaitForSeconds (tutorial2Clip.length - 1f);

        LeanTween.scale (touch, Vector3.one, 0.25f).setEaseOutBack();
        yield return new WaitForSeconds (1f);

        AudioClip tutorial3Clip = voData.FindVO ("3_taste");
        SoundManager.Instance.PlayVoiceOverClip (tutorial3Clip);
        LeanTween.scale (taste, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial3Clip.length);

        AudioClip tutorial4Clip = voData.FindVO ("4_hear");
        SoundManager.Instance.PlayVoiceOverClip (tutorial4Clip);
        LeanTween.scale (hear, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial4Clip.length);

        AudioClip tutorial5Clip = voData.FindVO ("5_see");
        SoundManager.Instance.PlayVoiceOverClip (tutorial5Clip);
        LeanTween.scale (see, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial5Clip.length);

        AudioClip tutorial6Clip = voData.FindVO ("6_smell");
        LeanTween.scale (smell, Vector3.one, 0.25f).setEaseOutBack ();
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

        instructionPopup.SetActive (false);
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

    public void NextQuestion () {
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        SubtitlePanel.Instance.Hide ();

        if (selectedObject)
            Destroy (selectedObject);

        // Tell factory to instantiate a random prefab and remove it from the list to prevent duplicates
        selectedObject = senseFactory.ManufactureRandomAndRemove ();

        // Select a random valid sense to ask the player
        selectedSense = SelectRandomSenseFromItem (selectedObject.GetComponent<SensesItem> ());
        senseText.text = string.Format ("What do I use to {0} the {1}?", selectedSense.ToString ().ToLower (), selectedObject.name);
    }

    private DataType.Senses SelectRandomSenseFromItem (SensesItem item) {
        DataType.Senses[] senseItemList = item.validSenses;
        return senseItemList.GetRandomItem ();
    }

    void OnCorrect () {
        SubtitlePanel.Instance.Display (correctLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    void OnIncorrect () {
        SubtitlePanel.Instance.Display (wrongLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Sad);
    }

    public bool IsSenseCorrect (DataType.Senses sense) {
        if (selectedSense != DataType.Senses.NONE) {
            if (sense == selectedSense) {
                OnCorrect ();
                return true;
            }

            OnIncorrect ();
        }

        return false;
    }
}
