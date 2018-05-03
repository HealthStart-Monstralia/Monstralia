using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : Singleton<StartManager> {
    public GameObject introObject;
    public bool playIntro = true;
    public Button[] buttonsToDisable;
    public Fader fader;
    public MonsterSelectionPanel monsterPanel;
    public DataType.MonsterType selectedMonster;

    // Events
    public delegate void ChooseMonster (DataType.MonsterType monsterType);
    public delegate void GameAction ();
    public static event ChooseMonster OnMonsterSelected, OnMonsterUnselected;

    [SerializeField] private CreateMonster monsterSpawn;
    private SwitchScene sceneLoader;

    new void Awake () {
        base.Awake ();
        fader.gameObject.SetActive (true);
        fader.FadeIn ();
    }

    private void OnEnable () {
        IntroManager.StartIntro += DisableButtons;
        IntroManager.EndIntro += EnableButtons;
    }

    private void OnDisable () {
        IntroManager.StartIntro -= DisableButtons;
        IntroManager.EndIntro -= EnableButtons;
    }

    void Start () {
        SoundManager.Instance.PlayBackgroundMusic ();
        sceneLoader = GetComponent<SwitchScene> ();

        // Remove when Release is ready for save system
#if TEST_BUILD
        if (!DeleteSaveSystem.deleteAndRestart) {
            GameManager.Instance.LoadGame ();
            DeleteSaveSystem.deleteAndRestart = false;
        }
#endif

        monsterSpawn.gameObject.SetActive (false);

        if (playIntro && !GameManager.Instance.isIntroShown) {
            GameManager.Instance.isIntroShown = true;
            DisableButtons ();
            StartCoroutine (PlayIntro ());
        } else if (GameManager.Instance.GetIsMonsterSelected ()) {
            monsterSpawn.gameObject.SetActive (true);
            CreateMonsterOnMap ();
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
            monsterPanel.gameObject.SetActive (true);
        }
        else {
            SwitchScene ();
        }
    }

    public void OnSelectedMonster (DataType.MonsterType monsterType) {
        selectedMonster = monsterType;
        OnMonsterSelected (monsterType);
        monsterPanel.ShowButtons ();
    }

    public void OnCancelSelection () {
        OnMonsterUnselected (selectedMonster);
        monsterPanel.HideButtons ();
    }

    public void StartGame () {
        GameManager.Instance.SetPlayerMonsterType (selectedMonster);
        SwitchScene ();
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
