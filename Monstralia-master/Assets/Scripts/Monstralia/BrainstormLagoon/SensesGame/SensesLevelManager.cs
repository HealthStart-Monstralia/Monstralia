using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesLevelManager : MonoBehaviour {
    public float timeLimit = 15f;
    public int scoreGoal = 3;
    public CreateMonster[] monsterCreators;
    public AudioClip transitionSfx;
    [HideInInspector] public Monster monster;

    [Header ("References")]
    [SerializeField] private SensesFactory senseFactory;
    [SerializeField] private Text senseText;
    [SerializeField] private string[] correctLines;
    [SerializeField] private string[] wrongLines;

    private DataType.Senses selectedSense;
    private GameObject selectedObject;

    public void SetupGame() {
        StopAllCoroutines ();
        StartCoroutine (PrepareToStart ());
    }

    IEnumerator PrepareToStart() {
        yield return new WaitForSeconds (0.5f);

        SoundManager.Instance.PlaySFXClip (transitionSfx);
        SensesGameManager.Instance.fireworksSystem.ActivateFireworks ();
        yield return new WaitForSeconds(0.1f);

        monster = monsterCreators[0].SpawnPlayerMonster ();
        SensesGameManager.Instance.ActivateHUD (true);
        SensesGameManager.Instance.StartCountdown (StartGame);
    }

    private DataType.Senses SelectRandomSense(SensesItem item) {
        DataType.Senses[] senseItemList = item.validSenses;
        return senseItemList.GetRandomItem ();
    }

    void StartGame() {
        SensesGameManager.Instance.OnGameStart ();
        NextQuestion ();
    }

    public void NextQuestion() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        SubtitlePanel.Instance.Hide ();
        Destroy (selectedObject);

        // Tell factory to instantiate a random prefab and remove it from the list to prevent duplicates
        selectedObject = senseFactory.ManufactureRandomAndRemove ();

        // Select a random valid sense to ask the player
        selectedSense = SelectRandomSense (selectedObject.GetComponent<SensesItem>());
        senseText.text = string.Format ("What do I use to {0} the {1}?", selectedSense.ToString ().ToLower(), selectedObject.name);
    }

    public void EndGame() {
        SensesGameManager.Instance.OnGameEnd ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
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

    void OnCorrect() {
        SubtitlePanel.Instance.Display (correctLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    void OnIncorrect () {
        SubtitlePanel.Instance.Display (wrongLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Sad);
    }

}