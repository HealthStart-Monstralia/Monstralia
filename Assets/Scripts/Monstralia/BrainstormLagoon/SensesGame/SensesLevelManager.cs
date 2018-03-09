using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesLevelManager : MonoBehaviour {
    public enum SensesLevelType {
        SensePanel,
        ObjectSelect
    }

    public float timeLimit = 15f;
    public int scoreGoal = 3;
    public CreateMonster[] monsterCreators;
    public AudioClip transitionSfx;
    [HideInInspector] public Monster monster;

    [Header ("References")]
    public SensesLevelType typeOfLevel;
    [SerializeField] private SensesFactory senseFactory;
    [SerializeField] private SensesFactory[] senseFactories;
    [SerializeField] private Text senseText;
    [SerializeField] private string[] correctLines;
    [SerializeField] private string[] wrongLines;

    private DataType.Senses selectedSense;
    private List<DataType.Senses> senseList = new List<DataType.Senses>();
    private GameObject selectedObject;

    private void Awake () {
        foreach (DataType.Senses sense in System.Enum.GetValues (typeof (DataType.Senses))) {
            senseList.Add (sense);
        }
    }

    public void SetupGame() {
        StopAllCoroutines ();
        StartCoroutine (PrepareToStart ());
    }

    IEnumerator PrepareToStart() {
        yield return new WaitForSeconds (0.5f);

        if (typeOfLevel == SensesLevelType.SensePanel) {
            SoundManager.Instance.PlaySFXClip (transitionSfx);
            SensesGameManager.Instance.fireworksSystem.ActivateFireworks ();
            yield return new WaitForSeconds (0.1f);
        }
        else {
            CreateSenseItems ();
            yield return new WaitForSeconds (0.1f);
        }


        monster = monsterCreators[0].SpawnPlayerMonster ();
        SensesGameManager.Instance.ActivateHUD (true);
        SensesGameManager.Instance.StartCountdown (StartGame);
    }

    private void CreateSenseItems () {
        foreach (SensesFactory factory in senseFactories) {
            GameObject product = factory.ManufactureRandom ();
            product.AddComponent<SensesClickInput> ();
        }
    }

    private DataType.Senses SelectRandomSenseFromItem(SensesItem item) {
        DataType.Senses[] senseItemList = item.validSenses;
        return senseItemList.GetRandomItem ();
    }

    private DataType.Senses SelectRandomSense () {
        return senseList.GetRandomItem ();
    }

    void StartGame() {
        SensesGameManager.Instance.OnGameStart ();
        NextQuestion ();
    }

    public void NextQuestion() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        SubtitlePanel.Instance.Hide ();

        if (typeOfLevel == SensesLevelType.SensePanel) {
            Destroy (selectedObject);

            // Tell factory to instantiate a random prefab and remove it from the list to prevent duplicates
            selectedObject = senseFactory.ManufactureRandomAndRemove ();

            // Select a random valid sense to ask the player
            selectedSense = SelectRandomSenseFromItem (selectedObject.GetComponent<SensesItem> ());
            senseText.text = string.Format ("What do I use to {0} the {1}?", selectedSense.ToString ().ToLower (), selectedObject.name);
        }
        else {
            selectedSense = SelectRandomSense ();
            senseText.text = string.Format ("What can I {0}?", selectedSense.ToString ().ToLower ());
        }
    }

    public void EndGame() {
        SensesGameManager.Instance.OnGameEnd ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    public bool DoesObjectHaveSense (SensesItem item) {
        if (selectedSense != DataType.Senses.NONE) {
            foreach (DataType.Senses sense in item.validSenses) {
                if (sense == selectedSense) {
                    OnCorrect ();
                    return true;
                }
            }

            OnIncorrect ();
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

    void OnCorrect() {
        SubtitlePanel.Instance.Display (correctLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    void OnIncorrect () {
        SubtitlePanel.Instance.Display (wrongLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Sad);
    }

}