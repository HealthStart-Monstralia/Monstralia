using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesTutorialManager : MonoBehaviour {
    public bool isRunningTutorial = false;
    public Coroutine tutorialCoroutine;
    public GameObject instructionPopup;
    public CreateMonster monsterCreator;

    [SerializeField] private GameObject see;
    [SerializeField] private GameObject hear;
    [SerializeField] private GameObject touch;
    [SerializeField] private GameObject smell;
    [SerializeField] private GameObject taste;
    [SerializeField] private GameObject learnSenses;
    [SerializeField] private GameObject welcomeSign;

    [SerializeField] private SensesFactory senseFactory;
    [SerializeField] private SensesFactory senseFactory2;
    [SerializeField] private SensesFactory senseFactory3;
    [SerializeField] private GameObject objectToRequest;
    [SerializeField] private GameObject objectToRequest2;
    [SerializeField] private GameObject objectToRequest3;
    [SerializeField] private GameObject hand;
    [SerializeField] private Transform handSpawn;
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


        isRunningTutorial = true;
        sensesManager.IsInputAllowed = false;
        instructionPopup.SetActive (true);
        senseText.gameObject.SetActive (false);
        yield return new WaitForSeconds (1.0f);

        AudioClip tutorial1Clip = voData.FindVO ("1_tutorial_start");
        AudioClip tutorial2Clip = voData.FindVO ("2_use_senses");
        AudioClip tutorial3Clip = voData.FindVO ("3_taste");
        AudioClip tutorial4Clip = voData.FindVO ("4_hear");
        AudioClip tutorial5Clip = voData.FindVO ("5_see");
        AudioClip tutorial6Clip = voData.FindVO ("6_smell");
        AudioClip tutorial7aClip = voData.FindVO ("7a_tap");
        AudioClip tutorial7bClip = voData.FindVO ("7b_tap");
        AudioClip tutorial7cClip = voData.FindVO ("7c_tap");
        AudioClip tutorial8Clip = voData.FindVO ("8_letmeshow");
        SubtitlePanel.Instance.Display ("Welcome to Senses Beach!", tutorial1Clip);
        yield return new WaitForSeconds (tutorial1Clip.length);

        monster = monsterCreator.SpawnPlayerMonster ();
        SensesGameManager.Instance.playerMonster = monster;
        sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Joyous);

        float waitTime = tutorial2Clip.length + tutorial3Clip.length + tutorial4Clip.length + tutorial5Clip.length + tutorial6Clip.length;
        SubtitlePanel.Instance.Display ("Let's use our five senses to touch, taste, hear, see, and smell the items on the beach!", tutorial2Clip, false, waitTime);
        yield return new WaitForSeconds (tutorial2Clip.length - 1f);

        sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        LeanTween.scale (touch, Vector3.one, 0.25f).setEaseOutBack();
        yield return new WaitForSeconds (1f);

        SoundManager.Instance.PlayVoiceOverClip (tutorial3Clip);
        LeanTween.scale (taste, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial3Clip.length);

        SoundManager.Instance.PlayVoiceOverClip (tutorial4Clip);
        LeanTween.scale (hear, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial4Clip.length);

        SoundManager.Instance.PlayVoiceOverClip (tutorial5Clip);
        LeanTween.scale (see, Vector3.one, 0.25f).setEaseOutBack ();
        yield return new WaitForSeconds (tutorial5Clip.length);

        LeanTween.scale (smell, Vector3.one, 0.25f).setEaseOutBack ();
        SoundManager.Instance.PlayVoiceOverClip (tutorial6Clip);
        yield return new WaitForSeconds (tutorial6Clip.length);

        SubtitlePanel.Instance.Display ("Our senses help keep us safe!", tutorial7aClip, false, tutorial7aClip.length);
        yield return new WaitForSeconds (tutorial7aClip.length);

        SubtitlePanel.Instance.Display ("I will ask you to find something with one of your senses.", tutorial7bClip, false, tutorial7bClip.length);
        yield return new WaitForSeconds (tutorial7bClip.length);

        SubtitlePanel.Instance.Display ("You tap the correct item on the screen.", tutorial7cClip, false, tutorial7cClip.length);
        yield return new WaitForSeconds (tutorial7cClip.length);


        SubtitlePanel.Instance.Display ("Let me show you!", tutorial8Clip, false, tutorial8Clip.length);
        yield return new WaitForSeconds (tutorial8Clip.length - 1f);

        LeanTween.scale (learnSenses, Vector3.zero, 0.25f).setEaseInBack ();
        LeanTween.scale (welcomeSign, Vector3.zero, 0.3f).setEaseInBack ();
        yield return new WaitForSeconds (1f);

        learnSenses.SetActive (false);
        welcomeSign.SetActive (false);
        senseText.gameObject.SetActive (true);
        selectedSense = DataType.Senses.Taste;
        senseText.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale (senseText.gameObject, Vector3.one, 0.3f).setEaseOutBack ();

        GameObject product = senseFactory.Manufacture (objectToRequest);
        product.AddComponent<SensesClickInput> ();
        GameObject product2 = senseFactory2.Manufacture (objectToRequest2);
        product2.AddComponent<SensesClickInput> ();
        GameObject product3 = senseFactory3.Manufacture (objectToRequest3);
        product3.AddComponent<SensesClickInput> ();
        sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Thoughtful);
        yield return new WaitForSeconds (1f);

        hand.gameObject.SetActive (true);
        Vector3 originalScale = hand.transform.localScale;

        LeanTween.move (hand.gameObject, product.transform.position, 1.5f).setEaseInOutCubic ();
        yield return new WaitForSeconds (1.5f);

        LeanTween.scale (hand.gameObject, originalScale * 0.9f, 0.2f);
        yield return new WaitForSeconds (0.2f);

        LeanTween.scale (hand.gameObject, originalScale, 0.2f);
        SoundManager.Instance.PlaySFXClip (SensesGameManager.Instance.correctSfx);
        yield return new WaitForSeconds (0.35f);

        LeanTween.move (hand.gameObject, handSpawn.transform.position, 1f).setEaseInBack ();
        yield return new WaitForSeconds (1.25f);

        AudioClip tutorial9Clip = voData.FindVO ("9_nowyoutry");
        SubtitlePanel.Instance.Display ("Now you try!");
        SoundManager.Instance.PlayVoiceOverClip (tutorial9Clip);
        sensesManager.IsInputAllowed = true;
        sensesManager.playerMonster.ChangeEmotions (DataType.MonsterEmotions.Happy);
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

    public bool DoesObjectHaveSense (SensesItem item) {
        if (SensesGameManager.Instance.IsInputAllowed) {
            if (selectedSense != DataType.Senses.NONE) {
                foreach (DataType.Senses sense in item.validSenses) {
                    if (sense == selectedSense) {
                        OnCorrect ();
                        return true;
                    }
                }

                OnIncorrect ();
            }
        }

        return false;
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
