/* SceneManager.cs
 * Description: This script manages the components Levels 1, 2, 3 which were originally built for a scene-based structure; later converted to activating/deactivates entire levels as game objects 
 *              It needs SpawnPrefabs_LJ.cs and InteractableObject_LJ.cs to run properly.
 *              MasterHandler_LJ.cs was created to help manage this script and the objects it is attached to.
 * Author: Lance C. Jasper
 * Edited by: Colby Tang
 * Created: 15JUNE2017
 * Last Modified: 24AUGUST2017 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager_LJ : MonoBehaviour {
    //-----PUBLIC FIELDS-----//
    [Header("Add SpawnPrefabScript")]
    [Tooltip("SpawnPrefab_LJ.cs must be attached to a game object and placed here.")]
    public GameObject spawnPrefabScript;
    public GameObject audioPrefabScript;

    //Holds the random assignSense value from SpawnPrefabs_LJ.cs
    public string[] senseInstructions;
    public Image goodJobScreen;
    public GameObject gameOverScreen;
    public ScoreGauge scoreGauge;

    [Header("Secs Before GoodJobScreen")]
    public float requestedTime = 1.5f;

    [Header("Delay Before Feedback Text Erase")]
    public float requestedTime2 = 1.5f;
    public Text feedbackText;


    [Header("Timer Text")]
    public Text timeCountdownText;
    public Timer timeCountdownSystem;
    public float timeIsUp = 15f;

    public bool starAdded;
    public bool roundIsOver;
    public bool gameOver;

    [Header ("Voiceovers")]
    public AudioClip[] rightChoiceVO;
    public AudioClip[] wrongChoiceVO;

    //-----PRIVATE FIELDS-----//
    private SpawnPrefabs_LJ spawner;
    private AudioManager_LJ getAudio;
    private ButtonAudioSource_LJ getButtonAudio;
    private IntroChimesAudioSource_LJ getChimesAudio;
    private PlayerInputController_LJ getInput;
    private List<int> receivedSensesOnTouch;
    private List<int> receivedMultiSensesOnTouch;
    private bool receivedSensesDone = false;
    private bool gameReady;
    private bool GaugeIncremented;
    private bool goodChoice;
    private bool PlayingLevel1;
    private int brainScore = 0;
    private int numOfStarsSenses;
    private int numOfStars;
    private float wipeTextDelay = 1.3f;
    private bool WhiteColor;
    private bool OrangeColor;
    private bool RedColor;
    private int singleSenseWinRan;
    private int multiSenseWinRan;

    private string[] wrongChoice =
    {
        //Lines picked from list randomly if player selects wrong item
        "Oops, it wasn't that...", "Hmmm, try another...", "Hmmm, not quite...", "Nope, not that...", "Give it another try..."
    };

    private string[] rightChoice =
    {
        //Lines picked from list randomly if player selects correct item
        "Yay! You got it!", "Way to go!", "You got it right!", "Nicely done!", "Well done, you got it!", "You're getting it!"
    };


    //-----ON GAME LOADING-----//
    void Awake()
    {
        //Initial states to avoid accidental repeats/issue caused by clicking, repeated actions, duplication, etc.
        starAdded = false;
        roundIsOver = false;
        goodJobScreen.enabled = false;
        gameOver = false;
        gameOverScreen.SetActive(false);
        GaugeIncremented = false;
        goodChoice = false;

        //Get a reference of the game object which holds a certain script
        GameObject spawnerScript = GameObject.Find("SpawnPrefabScript");
        GameObject audioScript = GameObject.Find("AudioPrefab");
        GameObject buttonAudioScript = GameObject.Find("ButtonClickedAudioSource");
        GameObject chimesAudioScript = GameObject.Find("PlayChimeAudioSource");
        GameObject inputScript = GameObject.Find("PlayerInputControllerScript");

        //Make an instance to the referenced game object to access its data
        spawner = spawnerScript.GetComponent<SpawnPrefabs_LJ>();
        getAudio = audioScript.GetComponent<AudioManager_LJ>();
        getButtonAudio = buttonAudioScript.GetComponent<ButtonAudioSource_LJ>();
        getChimesAudio = chimesAudioScript.GetComponent<IntroChimesAudioSource_LJ>();
        getInput = inputScript.GetComponent<PlayerInputController_LJ>();

        //Use "spawner." to call from the SpawnPrefabs_LJ.cs script
        spawner.spawnPrefab();
        spawner.nonTowelSpawn();
        spawner.towelSpawn();
        spawner.waterSpawn();
        spawner.spriteInstructionsSpawner();
    }


    //-----ON GAME START-----//
    void Start()
    {
        //Was having issue where Level 2/3 don't spawn all objects on first question; workaround to destroy first spawn, erase, and refresh objects
        //Level 1 has no spawn issue and no Spawn Location 2 which causes a null exception; ClearAndSpawn checks this and runs only on Level 2/3
        ClearAndSpawn();

        //Spawns countdown animation from GameManager script
        //Moved to MasterHandler_LJ.cs
        //GameManager.GetInstance().Countdown();

        //Set parameters for UI Slider that represents the green score gauge in the UI
        brainScore = 0;

        //A list of ints collected from the Interactable Object that is touched/clicked
        receivedSensesOnTouch = new List<int>();
        receivedMultiSensesOnTouch = new List<int>();

        //Set other parameters for game
        StartCoroutine(GameIsReady()); //Wait For Seconds Coroutine
        numOfStarsSenses = GameManager.GetInstance().GetNumStars(DataType.Minigame.MonsterSenses);
        numOfStars = numOfStarsSenses;
        Debug.Log(string.Format("I have {0} stars!", numOfStarsSenses));
    }

    //-----ON EVERY FRAME-----//
    void FixedUpdate()
    {
        TimeCountdown();
        timerColorSetter();
        timerColor();
    }


    //-----CANVAS UI TIMER-----//
    void TimeCountdown()
    {
        if (gameReady && !gameOver)
        {
            timeIsUp -= Time.deltaTime;
            //Debug.Log("TimeIsUp at: " + TimeIsUp.ToString("f2"));
            timeCountdownText.text = "Time: " + timeIsUp.ToString("f0");

            if (timeIsUp <= 0)
            {
                gameOver = true;
                timeCountdownText.text = "Time's Up!";
                GameIsOver();
            }

        }
    }


    //Dirty method to set timer color change based on time left
    private void timerColorSetter()
    {
        if (timeIsUp >= 9)
        {
            WhiteColor = true;
        }
        if (timeIsUp <= 8.999)
        {
            OrangeColor = true;
        }
        if (timeIsUp <= 0)
        {
            RedColor = true;
        }
    }

    //Set the timer color
    private void timerColor()
    {
        if (WhiteColor)
        {
            timeCountdownText.color = Color.white;
        }
        if (OrangeColor)
        {
            timeCountdownText.color = Color.yellow;
        }
        if (RedColor)
        {
            timeCountdownText.color = Color.red;
        }
    }


    //-----VICTORY CONDITION LOGIC-----//
    //Called from InteractableObject_LJ.cs
    public void WinCondition()
    {
        //-----Reminder-----//
        //see, hear, smell, taste, touch     
        //enumerates to elements 0, 1, 2, 3, 4
        //so see is 0, hear is 1, smell is 2, taste is 3, touch is 4 and those are the 5 senses

        //This sets the bool to FALSE so that the methods in "if (goodChoice)" won't be called until set to TRUE if/when 
        //the player selects an item that happens to be a match that answers the question the game is asking
        goodChoice = false;

        //Check each element in receivedSensesOnTouch and see if it matches sense to win or is a correct answer
        if (numOfStars == 0 || numOfStars == 1)
        {
            //winSense is an int that corresponds with the "sense" associated with enum in InteractableObject_LJ.cs "public enum Senses"
            //int winSense = spawner.GetConditionToWin(); calls to SpawnPrefabs_LJ.cs to collect the index AKA sense that has spawned into the scene
            //from this the game will only ever generate a question based on what is available in the scene
            int winSense = spawner.GetConditionToWin();
            foreach (int i in receivedSensesOnTouch)
            {
                if (i == winSense)
                {
                    //Now that the player has selected a good answer, goodChoice is set to true so that the methods in "if (goodChoice)" will run
                    goodChoice = true;
                    break;
                }
            }
            GaugeIncremented = false;
            singleSenseWinRan++;
            //Debugs-----
            //Debug.Log("Single Sense Win Test Ran: " + singleSenseWinRan);
        }

        else
        {
            //winSense is an int that corresponds with the "sense" associated with enum in InteractableObject_LJ.cs "public enum Senses"
            //int winSense = spawner.GetConditionToWin(); calls to SpawnPrefabs_LJ.cs to collect the index AKA sense that has spawned into the scene
            //from this the game will only ever generate a question based on what is available in the scene
            int winMultiSense = spawner.GetConditionToWinMultiSense();
            foreach (int i in receivedMultiSensesOnTouch)
            {
                if (i == winMultiSense)
                {
                    //Now that the player has selected a good answer, goodChoice is set to true so that the methods in "if (goodChoice)" will run
                    goodChoice = true;
                    break;
                }
            }
            GaugeIncremented = false;
            multiSenseWinRan++;
            //Debugs-----
            //Debug.Log("Multi Sense Win Test Ran: " + multiSenseWinRan);
        }

        //If the player touches the right object
        if (goodChoice)
        {
            BrainGaugeIncrement();
            IncrementBrainScore();
            GaugeIncremented = true;
            getButtonAudio.PlayPositiveSound();
            feedbackText.text = rightChoice[Random.Range(0, rightChoice.Length)];
        }

        //If the player must try again
        else
        {
            SoundManager.GetInstance ().PlayVoiceOverClip (wrongChoiceVO[Random.Range (0, wrongChoiceVO.Length)] );
            feedbackText.text = wrongChoice[Random.Range(0, wrongChoice.Length)];
            Invoke("WipeText", wipeTextDelay);
        }
    }


    //-----BRAIN METER-----//
    //Called when object clicked was good and a point is to be earned
    public void BrainGaugeIncrement()
    {
        if (!GaugeIncremented)
        {
            goodChoice = false;
            brainScore += 1;

            //If the player's last click is a winning choice, the game is won
            if (brainScore == 3)
            {
                WonGame();
            }
            else
            {
                //Player must try again, clear the slate
                Invoke("ClearExistingObjects", 1.5f);
                Invoke("SpawnNewObjects", 1.501f);
                Invoke("WipeText", wipeTextDelay);
            }
        }
    }


    //Increase Brain Meter in UI
    public void IncrementBrainScore() {
        if (scoreGauge.gameObject.activeSelf)
            scoreGauge.SetProgressTransition ((float)brainScore / 3);
    }


    //Spawn any necessary items
    //Calls to SpawnPrefabs_LC.cs
    public void SpawnNewPrefabs()
    {
        spawner.nonTowelSpawn();
        spawner.spawnPrefab();
        spawner.waterSpawn();
        spawner.spriteInstructionsSpawner();
        PlayChime();
    }


    //Plays sound when new objects spawn
    public void PlayChime()
    {
        //Checks if GameOver so chime won't hinder GameOver jingle (i.e. player clicks correct item .01 secs before time is up)
        if (!gameOver)
        {
            getChimesAudio.PlayChime();
        }
    }


    //-----IF WON GAME-----//
    public void WonGame()
    {
        getInput.InputLockOut();
        Invoke("LevelComplete", 1.5f);
        //Invoke("LoadHubWorld", 3f);
        roundIsOver = true;
        LevelUpMonster();
        gameOver = true;
    }


    //-----IG GAME LOST-----//
    void GameIsOver()
    {
        StartCoroutine(GameOverDelay());
        if (gameOver)
        {
            getInput.InputLockOut();
            gameOverScreen.SetActive(true);
            getAudio.PlayGameOverAudio();
        }
    }

    //-----ADD A STAR TO SENSES GAME-----//
    public void LevelUpMonster()
    {
        //Star not added yet, prevents star from being added unintentionally
        if (!starAdded)
        {
            if (roundIsOver)
            {
                GameManager.GetInstance().LevelUp(DataType.Minigame.MonsterSenses);
                starAdded = true;
            }
        }
    }


    //Workaround for not all objects showing at first. Clears them and respawns them. Unless playing level 1
    public void ClearAndSpawn()
    {
        if (spawner.spawnLocation2 == null)
        {
            PlayingLevel1 = true;
        }
        else
        {
            PlayingLevel1 = false;
        }
        if (!PlayingLevel1)
        {
            Invoke("ClearExistingObjects", .001f);
            Invoke("SpawnNewObjects", .002f);
            //Debug.Log(string.Format("Playing Level 1 is {0}, so ClearAndSpawn has been called", PlayingLevel1));
        }
    }


    //Called from InteractableObject_LJ.cs
    //Sends the senses properties of the interactable object that was clicked, to this script and adds them to a list to be checked in WinCondition
    public void SendSenses(int senseIndex)
    {
        receivedSensesOnTouch.Add(senseIndex);
    }

    //Called from InteractableObject_LJ.cs
    //Sends the MULTI senses properties of the interactable object that was clicked, to this script and adds them to a list to be checked in WinCondition
    public void SendMultiSenses(int multiSenseIndex)
    {
        receivedMultiSensesOnTouch.Add(multiSenseIndex);
    }

    //InteractableObject_LJ.cs runs through its forloop and when it comes out, it tells this script that it is done sending senses
    public void DoneSendingSenses()
    {
        receivedSensesDone = true;
    }


    //-----INVOKED METHODS-----//
    void ClearExistingObjects()
    {
        spawner.DestroyPrefabs();
        receivedSensesOnTouch.Clear();
        receivedMultiSensesOnTouch.Clear();
    }

    void SpawnNewObjects()
    {
        SpawnNewPrefabs();
    }

    void LevelComplete()
    {
        //GoodJobScreen.enabled = true;
        getAudio.PlayVictoryJingle();

        // Unlock corresponding sticker if the first level was completed
        if (PlayingLevel1) {
            MasterHandler_LJ.GetInstance ().GameOver ();
        }
        else {
            GameManager.GetInstance ().CreateEndScreen (MasterHandler_LJ.GetInstance ().typeOfGame, EndScreen.EndScreenType.CompletedLevel);
        }
    }

    void WipeText()
    {
        feedbackText.text = "";
    }


    //-----COROUTINES-----//
    public IEnumerator GameIsReady()
    {
        yield return new WaitForSeconds(0f);
        gameReady = true;
    }

    public IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2.6f);
        SceneManager.LoadScene("BrainstormLagoon");
    }
}
