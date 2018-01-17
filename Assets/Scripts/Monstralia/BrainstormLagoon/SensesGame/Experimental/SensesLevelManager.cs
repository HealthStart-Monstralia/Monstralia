using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesLevelManager : MonoBehaviour {
    public float timeLimit = 15f;
    public int scoreGoal = 3;
    public UnityStandardAssets.ImageEffects.BlurOptimized blur;
    public CreateMonster[] monsterCreators;
    public AudioClip transitionSfx;
    [HideInInspector] public Monster monster;

    [Header ("References")]
    [SerializeField] private SensesFactory senseFactory;
    [SerializeField] private List<DataType.Senses> senseList = new List<DataType.Senses>();
    [SerializeField] private Text senseText, commentText;
    [SerializeField] private string[] correctLines;
    [SerializeField] private string[] wrongLines;
    private bool isCommentHiding = false;
    private Coroutine commentCoroutine;
    private DataType.Senses selectedSense;
    private GameObject selectedObject;

    private void Awake () {
        foreach (DataType.Senses sense in System.Enum.GetValues (typeof (DataType.Senses))) {
            senseList.Add (sense);
        }
        senseList.Remove (DataType.Senses.NONE);
    }

    public void SetupGame() {
        StopAllCoroutines ();
        StartCoroutine (TransitionBlur ());
    }

    IEnumerator TransitionBlur() {
        float t = 3.0f;
        while (t > 0.0f) {
            t -= Time.deltaTime * 2;
            blur.blurSize = t;
            yield return null;
        }

        SoundManager.GetInstance ().PlaySFXClip (transitionSfx);
        SensesGameManager.GetInstance ().fireworksSystem.ActivateFireworks ();
        yield return new WaitForSeconds(0.1f);

        blur.enabled = false;
        StartCoroutine(PrepareToStartGame ());
    }

    IEnumerator PrepareToStartGame () {
        monster = monsterCreators[0].SpawnPlayerMonster ();
        SensesGameManager.GetInstance ().ActivateHUD(true);
        GameManager.GetInstance ().StartCountdown ();
        yield return new WaitForSeconds (3.5f);

        SensesGameManager.GetInstance ().fireworksSystem.ActivateFireworks ();
        yield return new WaitForSeconds (0.5f);

        StartGame ();
    }

    private DataType.Senses SelectRandomSense() {
        DataType.Senses sense = senseList.RandomItem();
        return sense;
    }

    void StartGame() {
        SensesGameManager.GetInstance ().OnGameStart ();
        NextQuestion ();
    }

    public void NextQuestion() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        selectedSense = SelectRandomSense ();
        ResetComment ();

        Destroy (selectedObject);
        selectedObject = senseFactory.ManufactureRandomPrefab ();
        senseText.text = string.Format ("What do I use to {0} this {1}?", selectedSense.ToString ().ToLower(), selectedObject.name);
    }

    public void ResetComment() {
        commentText.text = "";
    }

    public void EndGame() {
        SensesGameManager.GetInstance ().OnGameEnd ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    public bool IsSenseCorrect (DataType.Senses sense) {
        if (selectedSense != DataType.Senses.NONE) {
            if (sense == selectedSense) {
                ShowComment(correctLines.RandomItem ());
                monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
                return true;
            } else {
                ShowComment (wrongLines.RandomItem ());
                monster.ChangeEmotions (DataType.MonsterEmotions.Sad);
                return false;
            }

        }
        
        else return false;
    }

    void ShowComment (string text) {
        commentText.text = text;
        if (isCommentHiding)
            StopCoroutine (commentCoroutine);
        commentCoroutine = StartCoroutine (HideComment());
    }

    IEnumerator HideComment() {
        isCommentHiding = true;
        yield return new WaitForSeconds (4f);
        commentText.text = "";
        isCommentHiding = false;
    }
}
