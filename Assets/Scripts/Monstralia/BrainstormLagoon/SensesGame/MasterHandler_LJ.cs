/* MasterHandler_LJ.cs
 * Description: This script remains active as a manager in the Senses Game scene. It activates/deactivates Level 1, Level 2, Level 3 game objects to act like scenes. (Initially built with scene-based structure)
 *              It also holds some shared/common items for use throughout the Senses Game. 
 * Author: Lance C. Jasper
 * Edited by: Colby Tang
 * Created: 6AUGUST2017
 * Last Modified: 13SEP2017 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MasterHandler_LJ : AbstractGameManager
{
    //-----PRIVATE FIELDS-----//
    private static MasterHandler_LJ instance;
    private AudioManager_LJ getAudio;
    private SceneManager_LJ sceneManager;
    private GameObject FireWorksSpawn1;
    private GameObject FireWorksSpawn2;
    private GameObject FireWorksSpawn3;
    private GameObject FireWorksSpawn4;
    private GameObject FireWorksSpawn5;
    private int randInt1;
    private int randInt2;
    private int randInt3;
    private int randInt4;
    private int randInt5;
    private int numOfStarsSenses;
    private int numOfStars;
    private float fadeTime = 0f;
    private float fadeDuration = 3f;
    private bool fadeButton;
    private bool playButtonPressed;


    //-----PUBLIC FIELDS-----//
    public VoiceOversData voData;
    public GameObject UICanvas;
    public GameObject LevelOne;
    public GameObject LevelTwo;
    public GameObject LevelThree;
    public Text Timer;
    public GameObject PlayButton;
    public GameObject BabyMonsters;
    public GameObject[] FireWorksPrefab;
    public GameObject[] LargeFireWorkPrefab;
    public Transform[] FireWorkTransform;
    public Transform LargeFireWorkTransform;


    //-----ON GAME LOADING-----//
    void Awake()
    {
        // Apply singleton property to MasterHandler_LJ
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }

        SoundManager.GetInstance ().ChangeBackgroundMusic (backgroundMusicArray[Random.Range (0, backgroundMusicArray.Length)]);


        //Get references to objects/scripts to call their function here in this script
        GameObject sceneScript = GameObject.Find("MasterHandlerScript");
        sceneManager = sceneScript.GetComponent<SceneManager_LJ>();

        GameObject audioScript = GameObject.Find("AudioPrefab");
        getAudio = audioScript.GetComponent<AudioManager_LJ>();
    }

    public static MasterHandler_LJ GetInstance () {
        return instance;
    }


    //-----ON GAME START-----//
    new void Start()
    {
        UICanvas.SetActive (false);
        //Set other parameters for game
        numOfStarsSenses = GameManager.GetInstance().GetNumStars(DataType.Minigame.MonsterSenses);
        numOfStars = numOfStarsSenses;
        randInt1 = Random.Range(0, FireWorksPrefab.Length);
        randInt2 = Random.Range(0, FireWorksPrefab.Length);
        randInt3 = Random.Range(0, FireWorksPrefab.Length);
        randInt4 = Random.Range(0, FireWorksPrefab.Length);
        randInt5 = Random.Range(0, LargeFireWorkPrefab.Length);
        base.Start (); // Calls Start() from AbstractGameManager
    }


    public override void PregameSetup()
    {
        UICanvas.SetActive (true);
        voData.PlayVO ("welcome");
        //Created this public void for AbstractGameManager to access. The Senses Game is managed by this MasterHandler_LJ.cs script and a script called SceneManager_LJ.cs. 
        //The MasterHandler remains active in the Senses Game scene and activates/deactivates game objects that act as Level 1, Level 2, and Level 3.
        Debug.Log("From MasterHandler_LJ.cs: The PregameSetup function for Senses Game has been called by ReviewGameWinLose.cs. ----- This method is temporarily empty.");
    }

    public override void GameOver () {
        UnlockSticker ();
    }

    void Update()
    {
        FadePlayButton();
        KeyPressLToLevelUp(); //For debugging, can comment out (go to line: 220)
        KeyPressRToResetScene(); //For debugging, can comment out (go to line: 220)
    }


    //Called by pressing/clicking Canvas UI "PLAY" button in Senses Game
    public void PlaySenseGame()
    {
        //If PLAY not already pressed, begin the Senses Game
        //**Will lead to game objects acting as Level 1, Level 2, Level 3 to be ACTIVE; scripts attached to the inactive game objects will then run
        //**When the game objects holding the SceneManager_LJ.cs become ACTIVE, that script will run and setup/initializes other dependent fields, etc.
        if (!playButtonPressed)
        {
            playButtonPressed = true;
            fadeButton = true;
            GameManager.GetInstance().Countdown();
            getAudio.PlayIntroJingle();
            Invoke("PlayMagicWoosh", 4.3f);
            Invoke("FireWorksClearOut", 4.45f);
            Invoke("HideIntroTimer", 4.999f);
            Invoke("ActivateScenes", 5f);
        }
    }

    //Fireworks FX for Senses Game intro screen, Monster exit (after countdown animation)
    //Royalty Free Prefabs "Simple FX - Cartoon Particles" used from Asset Store by: Synty Studios in folder "SimpleFX"
    //website: https://www.facebook.com/syntystudios
    public void FireworksPoof()
    {
        FireWorksSpawn1 = Instantiate(FireWorksPrefab[randInt1], FireWorkTransform[0].transform.position, Quaternion.Euler(0, 0, 0));
        FireWorksSpawn2 = Instantiate(FireWorksPrefab[randInt2], FireWorkTransform[1].transform.position, Quaternion.Euler(0, 0, 0));
        FireWorksSpawn3 = Instantiate(FireWorksPrefab[randInt3], FireWorkTransform[2].transform.position, Quaternion.Euler(0, 0, 0));
        FireWorksSpawn4 = Instantiate(FireWorksPrefab[randInt4], FireWorkTransform[3].transform.position, Quaternion.Euler(0, 0, 0));
        FireWorksSpawn5 = Instantiate(LargeFireWorkPrefab[randInt5], LargeFireWorkTransform.transform.position, Quaternion.Euler(0, 0, 0));
    }


    //Play woosh sound after countdown animation
    public void PlayMagicWoosh()
    {
        getAudio.PlayMagicWoosh();
    }


    //Intro screen tutorial timer becomes inactive
    public void HideIntroTimer()
    {
        Timer.enabled = false;
    }


    //PLAY button in Senses Game intro fades for effect
    public void FadePlayButton()
    {
        if (fadeButton)
        {
            PlayButton.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, fadeTime);
            if (fadeTime < 1)
            {
                fadeTime += Time.deltaTime / fadeDuration;
            }
        }
    }


    //After countdown animation, fireworks go off and the other UI elements become inactive
    public void FireWorksClearOut()
    {
        FireworksPoof();
        PlayButton.SetActive(false);
        BabyMonsters.SetActive(false);
    }


    //Is invoked with a delay long enough to cover the countdown animation, then Level 1/2/3 game object is activated and its scripts run
    public void ActivateScenes()
    {
        SensesLevelToLoad(numOfStars);
        Debug.Log("Activated Scenes");
    }


    //Calls for when Level to activate/deactivate
    public void SensesLevelToLoad(int numOfStar)
    {
        if (numOfStarsSenses == 0)
        {
            LoadLevel1();
        }
        if (numOfStarsSenses == 1)
        {
            LoadLevel2();
        }
        if (numOfStarsSenses == 2)
        {
            LoadLevel3();
        }

        if (numOfStarsSenses > 2)
        {
            LoadLevel3();
        }
    }


    //-----ACTIVATE LEVEL METHODS-----//
    //Manipulates layers/gameobjects in Senses scene to emulate a scene being loading
    private void LoadLevel1()
    {
        //LEVEL 1
        Debug.Log(string.Format("Level {0} loaded because you have {1} stars.", LevelOne.name, numOfStars));
        LevelOne.SetActive(true);
        LevelTwo.SetActive(false);
        LevelThree.SetActive(false);
    }
    private void LoadLevel2()
    {
        //LEVEL 2
        Debug.Log(string.Format("Level {0} loaded because you have {1} stars.", LevelTwo.name, numOfStars));
        LevelTwo.SetActive(true);
        LevelOne.SetActive(false);
        LevelThree.SetActive(false);
    }
    private void LoadLevel3()
    {
        //LEVEL 3
        Debug.Log(string.Format("Level {0} loaded because you have {1} stars.", LevelThree.name, numOfStars));
        LevelThree.SetActive(true);
        LevelOne.SetActive(false);
        LevelTwo.SetActive(false);
    }

    //-----DEBUGGING-----//    //-----DEBUGGING-----//    //-----DEBUGGING-----//    //-----DEBUGGING-----//
    //-----Used for deubbing, if R is pressed on main keyboard area the scene loads, can quickly generate new prefabs/questions-----//
    void KeyPressRToResetScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Senses");
        }
    }

    //-----Used for deubbing, if L is pressed on main keyboard area a star is added to the Sense Game, can quickly level up-----//
    void KeyPressLToLevelUp()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.GetInstance().LevelUp(DataType.Minigame.MonsterSenses);
        }
    }
    //-----DEBUGGING-----//    //-----DEBUGGING-----//    //-----DEBUGGING-----//    //-----DEBUGGING-----//
}