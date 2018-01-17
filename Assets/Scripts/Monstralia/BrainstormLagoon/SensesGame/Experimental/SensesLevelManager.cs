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
    public DataType.Senses selectedSense;

    private List<DataType.Senses> senseList = new List<DataType.Senses>();
    [SerializeField] private Text textObject;

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
        monsterCreators[0].SpawnPlayerMonster ();
        SensesGameManager.GetInstance ().ActivateHUD(true);
        GameManager.GetInstance ().StartCountdown ();
        yield return new WaitForSeconds (3.5f);

        SensesGameManager.GetInstance ().fireworksSystem.ActivateFireworks ();
        yield return new WaitForSeconds (0.5f);

        StartGame ();
    }

    private DataType.Senses SelectRandomSense() {
        DataType.Senses sense = senseList[Random.Range (0, Constants.NUM_OF_SENSES)];
        textObject.text = sense.ToString();
        return sense;
    }

    void StartGame() {
        SensesGameManager.GetInstance ().OnGameStart ();
        selectedSense = SelectRandomSense ();
    }

    public void NextQuestion() {
        selectedSense = SelectRandomSense ();
    }

    public void EndGame() {
        SensesGameManager.GetInstance ().OnGameEnd ();
    }

    public bool IsSenseCorrect (DataType.Senses sense) {
        if (selectedSense != DataType.Senses.NONE) {
            if (sense == selectedSense) return true;
            else return false;
        }
        
        else return false;
    }
}
