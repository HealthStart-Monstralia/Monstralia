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
    [SerializeField] private SensesFactory[] senseFactories;
    [SerializeField] private Text senseText;
    [SerializeField] private string[] correctLines;
    [SerializeField] private string[] wrongLines;

    private DataType.Senses selectedSense;
    private List<DataType.Senses> senseList = new List<DataType.Senses>();

    private void Awake () {
        foreach (DataType.Senses sense in System.Enum.GetValues (typeof (DataType.Senses))) {
            if (sense != DataType.Senses.NONE) {
                senseList.Add (sense);
            }
        }
    }

    public void SetupGame() {
        StopAllCoroutines ();
        StartCoroutine (PrepareToStart ());
    }

    IEnumerator PrepareToStart() {
        yield return new WaitForSeconds (0.5f);
        SensesGameManager.Instance.fireworksSystem.ActivateFireworks ();
        SoundManager.Instance.PlaySFXClip (transitionSfx);
        yield return new WaitForSeconds (0.1f);

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

    private void DestroySenseItems () {
        foreach (SensesFactory factory in senseFactories) {
            factory.RemoveCurrentObject ();
        }
    }

    private DataType.Senses SelectRandomSense () {
        DataType.Senses randomSense = senseFactories.GetRandomItem ().currentItem.GetComponent<SensesItem> ().ChooseRandomSense ();
        return randomSense;
    }

    void StartGame() {
        SensesGameManager.Instance.OnGameStart ();
        NextQuestion ();
    }

    public void NextQuestion() {
        monster.ChangeEmotions (DataType.MonsterEmotions.Happy);
        SubtitlePanel.Instance.Hide ();
        DestroySenseItems ();
        CreateSenseItems ();
        selectedSense = SelectRandomSense ();
        senseText.text = string.Format ("What can I {0}?", selectedSense.ToString ().ToLower ());
    }

    public void EndGame() {
        SensesGameManager.Instance.OnGameEnd ();
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
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

    void OnCorrect() {
        SubtitlePanel.Instance.Display (correctLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Joyous);
    }

    void OnIncorrect () {
        SubtitlePanel.Instance.Display (wrongLines.GetRandomItem ());
        monster.ChangeEmotions (DataType.MonsterEmotions.Sad);
    }

}