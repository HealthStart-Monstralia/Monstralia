using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : Singleton<StartManager> {
    public GameObject introObject;
    public bool playIntro = true;
    public Button[] buttonsToDisable;
    public Fader fader;
    public GameObject monsterSelection;

    [SerializeField] private CreateMonster monsterSpawn;
    private SwitchScene sceneLoader;

    new void Awake () {
        base.Awake ();
        fader.gameObject.SetActive (true);
        fader.FadeIn ();
    }

    void Start () {
        SoundManager.Instance.PlayBackgroundMusic ();
        sceneLoader = GetComponent<SwitchScene> ();
        GameManager.Instance.LoadGame ();

        monsterSpawn.gameObject.SetActive (false);
        if (GameManager.Instance.GetIsMonsterSelected()) {
            monsterSpawn.gameObject.SetActive (true);
            CreateMonsterOnMap ();
        }

        if (playIntro && !GameManager.Instance.isIntroShown) {
            GameManager.Instance.isIntroShown = true;
            DisableButtons ();
            StartCoroutine (PlayIntro ());
        }
    }

    IEnumerator PlayIntro() {
        yield return new WaitForSeconds (0.5f);
        introObject.SetActive (true);
    }

    public void DisableButtons() {
        foreach (Button button in buttonsToDisable) {
            button.interactable = false;
        }
    }

    public void EnableButtons () {
        foreach (Button button in buttonsToDisable) {
            button.interactable = true;
        }
    }

    public void SwitchScene() {
        sceneLoader.LoadScene ();
    }

    public void SelectMonster () {
        GameManager.Instance.AllowSave ();
        if (!GameManager.Instance.GetIsMonsterSelected()) {
            monsterSelection.SetActive (true);
        }
        else {
            SwitchScene ();
        }
    }

    void CreateMonsterOnMap () {
        Monster monster = monsterSpawn.SpawnPlayerMonster ();
        monster.transform.SetParent (monsterSpawn.transform);
        monster.spriteRenderer.sortingLayerName = "UI";
        monster.spriteRenderer.sortingOrder = 0;
        monster.transform.localScale = Vector3.one * 30f;
        monster.AllowMonsterTickle = true;
        monster.IdleAnimationOn = true;
        monster.spawnAnimation = true;
    }
}
