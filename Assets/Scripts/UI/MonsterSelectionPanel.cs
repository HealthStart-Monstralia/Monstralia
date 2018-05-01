using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSelectionPanel : MonoBehaviour {
    public AudioClip voiceOverToPlayOnStart;
    public Transform blueSpawnpoint;
    public Transform greenSpawnpoint;
    public Transform redSpawnpoint;
    public Transform yellowSpawnpoint;
    public Button yesButton;
    public Button noButton;
    public Text monsterNameText;
    private Outline monsterNameOutline;
    public Text instructionLabel;

    public Color blueTextColor;
    public Color greenTextColor;
    public Color redTextColor;
    public Color yellowTextColor;
    public Color blueTextOutlineColor;
    public Color greenTextOutlineColor;
    public Color redTextOutlineColor;
    public Color yellowTextOutlineColor;

    private Vector3 originalScale;
    private Coroutine buttonCoroutine;
    private int yesScaleTweenID;
    private int noScaleTweenID;
    private bool isTransitioning = false;

    private void Start () {
        SoundManager.Instance.PlayVoiceOverClip (voiceOverToPlayOnStart);

        monsterNameOutline = monsterNameText.GetComponent<Outline> ();
        originalScale = yesButton.transform.localScale;
        HideButtons ();
        instructionLabel.text = "Choose a monster to help grow!";
        monsterNameText.gameObject.SetActive (false);
    }

    public void ShowButtons () {
        if (isTransitioning)
            StopCoroutine (buttonCoroutine);
        buttonCoroutine = StartCoroutine (SetButtons (true));
        instructionLabel.text = "Adopt this monster?";
        monsterNameText.gameObject.SetActive (true);
        SelectTextColor ();
    }

    public void HideButtons () {
        if (isTransitioning)
            StopCoroutine (buttonCoroutine);
        buttonCoroutine = StartCoroutine (SetButtons (false));
        instructionLabel.text = "Choose a monster to help grow!";
        monsterNameText.gameObject.SetActive (false);
    }

    public IEnumerator SetButtons (bool state) {
        isTransitioning = true;
        LeanTween.cancel (yesScaleTweenID);
        LeanTween.cancel (noScaleTweenID);

        // Showing buttons
        if (state) {
            yesButton.gameObject.SetActive (state);
            noButton.gameObject.SetActive (state);
            yesScaleTweenID = LeanTween.scale (yesButton.gameObject, originalScale, 0.5f).setEaseOutBack ().id;
            noScaleTweenID = LeanTween.scale (noButton.gameObject, originalScale, 0.5f).setEaseOutBack ().id;
        }

        // Hiding buttons
        else {
            yesScaleTweenID = LeanTween.scale (yesButton.gameObject, Vector2.zero, 0.25f).id;
            noScaleTweenID = LeanTween.scale (noButton.gameObject, Vector2.zero, 0.25f).id;
        }

        yesButton.interactable = state;
        noButton.interactable = state;

        yield return new WaitForSeconds (0.5f);

        if (!state) {
            yesButton.gameObject.SetActive (state);
            noButton.gameObject.SetActive (state);
        }
        isTransitioning = false;
    }

    void SelectTextColor () {
        switch (StartManager.Instance.selectedMonster) {
            case DataType.MonsterType.Blue:
                monsterNameText.color = blueTextColor;
                monsterNameOutline.effectColor = blueTextOutlineColor;
                monsterNameText.text = Constants.MONSTER_BLUE_NAME;
                break;
            case DataType.MonsterType.Green:
                monsterNameText.color = greenTextColor;
                monsterNameOutline.effectColor = greenTextOutlineColor;
                monsterNameText.text = Constants.MONSTER_GREEN_NAME;
                break;
            case DataType.MonsterType.Red:
                monsterNameText.color = redTextColor;
                monsterNameOutline.effectColor = redTextOutlineColor;
                monsterNameText.text = Constants.MONSTER_RED_NAME;
                break;
            case DataType.MonsterType.Yellow:
                monsterNameText.color = yellowTextColor;
                monsterNameOutline.effectColor = yellowTextOutlineColor;
                monsterNameText.text = Constants.MONSTER_YELLOW_NAME;
                break;
        }
    }

}
