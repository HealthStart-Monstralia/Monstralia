/* SceneManager.cs
 * Description: This script manages the components Levels 1, 2, 3 which were originally built for a scene-based structure; later converted to activating/deactivates entire levels as game objects 
 *              It needs SpawnPrefabs_LJ.cs and InteractableObject_LJ.cs to run properly.
 *              MasterHandler_LJ.cs was created to help manage this script and the objects it is attached to.
 * Author: Lance C. Jasper
 * Created: 15JUNE2017
 * Last Modified: 11AUGUST2017 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    [Header("Add SpawnPrefabScript")]
    [Tooltip("SpawnPrefab_LJ.cs must be attached to a game object and placed here.")]
    public GameObject SpawnPrefabScript;
    public GameObject AudioPrefabScript;

    //Holds the random assignSense value from SpawnPrefabs_LJ.cs
    public string[] senseInstructions;
    public Image GoodJobScreen;
    public GameObject GameOverScreen;
    public Slider BrainGauge;

    [Header("Secs Before GoodJobScreen")]
    public float requestedTime = 1.5f;

    [Header("Delay Before Feedback Text Erase")]
    public float requestedTime2 = 1.5f;
    public Text FeedbackText;


    [Header("Timer Text")]
    public Text TimeCountdownText;
    public Timer TimeCountdownSystem;
    public float TimeIsUp = 15f;

    public bool StarAdded;
    public bool RoundIsOver;
    public bool GameOver;


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
    private int BrainScore = 0;
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
        StarAdded = false;
        RoundIsOver = false;
        GoodJobScreen.enabled = false;
        GameOver = false;
        GameOverScreen.SetActive(false);
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
        BrainGauge.minValue = 0;
        BrainScore = 0;

        //A list of ints collected from the Interactable Object that is touched/clicked
        receivedSensesOnTouch = new List<int>();
        receivedMultiSensesOnTouch = new List<int>();

        //Set other parameters for game
        StartCoroutine(GameIsReady()); //Wait For Seconds Coroutine
        numOfStarsSenses = GameManager.GetInstance().GetNumStars(MinigameData.Minigame.MonsterSenses);
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
        if (gameReady && !GameOver)
        {
            TimeIsUp -= Time.deltaTime;
            //Debug.Log("TimeIsUp at: " + TimeIsUp.ToString("f2"));
            TimeCountdownText.text = "Time: " + TimeIsUp.ToString("f0");

            if (TimeIsUp <= 0)
            {
                GameOver = true;
                TimeCountdownText.text = "Time's Up!";
                GameIsOver();
            }

        }
    }


    //Dirty method to set timer color change based on time left
    private void timerColorSetter()
    {
        if (TimeIsUp >= 9)
        {
            WhiteColor = true;
        }
        if (TimeIsUp <= 8.999)
        {
            OrangeColor = true;
        }
        if (TimeIsUp <= 0)
        {
            RedColor = true;
        }
    }

    //Set the timer color
    private void timerColor()
    {
        if (WhiteColor)
        {
            TimeCountdownText.color = Color.white;
        }
        if (OrangeColor)
        {
            TimeCountdownText.color = Color.yellow;
        }
        if (RedColor)
        {
            TimeCountdownText.color = Color.red;
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
            FeedbackText.text = rightChoice[Random.Range(0, rightChoice.Length)];
        }

        //If the player must try again
        else
        {
            FeedbackText.text = wrongChoice[Random.Range(0, wrongChoice.Length)];
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
            BrainScore += 1;

            //If the player's last click is a winning choice, the game is won
            if (BrainScore == 3)
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
    public void IncrementBrainScore()
    {
        BrainGauge.value = BrainScore;
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
        if (!GameOver)
        {
            getChimesAudio.PlayChime();
        }
    }


    //-----IF WON GAME-----//
    public void WonGame()
    {
        getInput.InputLockOut();
        Invoke("LevelComplete", 1.5f);
        Invoke("LoadHubWorld", 3f);
        RoundIsOver = true;
        LevelUpMonster();
        GameOver = true;
    }


    //-----IG GAME LOST-----//
    public void GameIsOver()
    {
        StartCoroutine(GameOverDelay());
        if (GameOver)
        {
            getInput.InputLockOut();
            GameOverScreen.SetActive(true);
            getAudio.PlayGameOverAudio();
        }
    }

    //-----ADD A STAR TO SENSES GAME-----//
    public void LevelUpMonster()
    {
        //Star not added yet, prevents star from being added unintentionally
        if (!StarAdded)
        {
            if (RoundIsOver)
            {
                GameManager.GetInstance().LevelUp(MinigameData.Minigame.MonsterSenses);
                StarAdded = true;
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

    // Unlock corresponding sticker if the first level was completed
    void UnlockSticker() {
        if (PlayingLevel1) {
            GameManager.GetInstance ().ActivateSticker (MinigameData.Minigame.MonsterSenses);
        }
    }

    void LevelComplete()
    {
        GoodJobScreen.enabled = true;
        getAudio.PlayVictoryJingle();
        UnlockSticker ();
    }

    void LoadHubWorld()
    {
        SceneManager.LoadScene("BrainstormLagoon");
    }

    void WipeText()
    {
        FeedbackText.text = "";
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
