/* SpawnPrefabs.cs
 * Description: This script instantiates prefabs from public arrays.
 *              It also generates a list of game objects that were spawned so that the game will only ask a question based on available items to select.
 * Author: Lance C. Jasper
 * Created: 15JUNE2017
 * Last Modified: 11AUGUST2017 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabs_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    [Header("Spawn Locations")]
    [Tooltip("Add empty game objects to use their Transform as a spawn location.")]
    public Transform spawnLocation1;
    public Transform spawnLocation2;
    public Transform spawnLocation3;
    public Transform spawnLocation4;

    public Transform[] waterLocations;
    public Transform[] nonTowelLocations;
    public Transform[] towelLocations;
    public Transform[] senseInstructionLocations;
    public Transform[] FireWorksSpawnLocations;
    public Transform FireWorksMonsterLocation;

    [Header("Prefabs To Spawn")]
    [Tooltip("A few of these prefabs will be randomly selected to spawn during runtime.")]
    public GameObject[] whatToSpawn;
    public GameObject[] waterSpawnPrefab;
    public GameObject[] nonTowelSpawnPrefab;
    public GameObject[] towelSpawnPrefab;
    public GameObject[] senseInstructionPrefabs;
    public GameObject[] multiSenseInstructionPrefabs;
    public GameObject[] fireWorksPrefab;

    //This list collects items spawned
    [HideInInspector]
    public List<InteractableObject_LJ> spawnedItems; 
    
    //This list comprises all InteractableObject_LJ.cs items at the end of the spawn function
    [Header("Full Spawned List")]
    [Tooltip("Automatically populates at runtime.")]
    public List<InteractableObject_LJ> fullSpawnedList;

    public GameObject SceneManagerScript;
    public static string whichSenseToWin;


    //-----PRIVATE FIELDS-----//
    private int randInt1;
    private int randInt2;
    private int randInt3;
    private int randInt4;
    private int randInt5;
    private int randInt6;
    private int randInt7;
    private int randInt8;
    private int instructionIndex;
    private int instructionIndexMultiSense;
    private int numOfStarsSenses;


    //-----GAME OBJECT PREFABS-----//
    private GameObject[] forLoopItem;
    private GameObject forLoopPrefab1;
    private GameObject forLoopPrefab2;
    private GameObject forLoopPrefab3;
    private GameObject forLoopPrefab4;
    private GameObject waterPrefab;
    private GameObject nonTowelPrefab;
    private GameObject towelPrefab;
    private GameObject instructionsPrefab;
    private GameObject fireWorkPrefab1;
    private GameObject fireWorkPrefab2;
    private GameObject fireWorkPrefab3;
    private GameObject fireWorkPrefab4;
    private GameObject fireWorkMonsterPrefab;



    //-----ON GAME LOADING-----// 
    void Awake()
    {
        spawnedItems = new List<InteractableObject_LJ>();
    }


    //-----ON GAME START-----// 
    void Start()
    {
        //Finds the InteractableObject_LJ component attached to the game object that was instantiated
        InteractableObject_LJ referencedObject = GameObject.FindObjectOfType<InteractableObject_LJ>();
        numOfStarsSenses = GameManager.GetInstance().GetNumStars(MinigameData.Minigame.MonsterSenses);
        StartUpMonsterFireWorks();
    }


    //-----SPAWN OBJECTS LOGIC-----//
    public void spawnPrefab()
    {
        randInt1 = Random.Range(0, whatToSpawn.Length);
        randInt2 = Random.Range(0, whatToSpawn.Length);
        randInt3 = Random.Range(0, whatToSpawn.Length);
        randInt4 = Random.Range(0, whatToSpawn.Length);

        randInt5 = Random.Range(0, fireWorksPrefab.Length);
        randInt6 = Random.Range(0, fireWorksPrefab.Length);
        randInt7 = Random.Range(0, fireWorksPrefab.Length);
        randInt8 = Random.Range(0, fireWorksPrefab.Length);


        //LEVEL ONE
        if (numOfStarsSenses == 0)
        {
            //Spawn Interactable Object
            forLoopPrefab1 = Instantiate(whatToSpawn[randInt1], spawnLocation1.transform.position, Quaternion.Euler(0, 0, 0));
            spawnedItems.Add(forLoopPrefab1.GetComponent<InteractableObject_LJ>());

            //Spawn Fireworks FX slightly in front of Interactable Object
            fireWorkPrefab1 = Instantiate(fireWorksPrefab[randInt5], FireWorksSpawnLocations[0].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab2 = Instantiate(fireWorksPrefab[randInt6], FireWorksSpawnLocations[1].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab3 = Instantiate(fireWorksPrefab[randInt7], FireWorksSpawnLocations[2].transform.position, Quaternion.Euler(0, 0, 0));
        }

        //LEVEL TWO
        if (numOfStarsSenses == 1)
        {
            //Spawn Interactable Object
            forLoopPrefab1 = Instantiate(whatToSpawn[randInt1], spawnLocation1.transform.position, Quaternion.Euler(0, 0, 0));
            forLoopPrefab2 = Instantiate(whatToSpawn[randInt2], spawnLocation2.transform.position, Quaternion.Euler(0, 0, 0));
            spawnedItems.Add(forLoopPrefab1.GetComponent<InteractableObject_LJ>());
            spawnedItems.Add(forLoopPrefab2.GetComponent<InteractableObject_LJ>());

            //Spawn Fireworks FX slightly in front of Interactable Object
            fireWorkPrefab1 = Instantiate(fireWorksPrefab[randInt5], FireWorksSpawnLocations[0].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab2 = Instantiate(fireWorksPrefab[randInt6], FireWorksSpawnLocations[1].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab3 = Instantiate(fireWorksPrefab[randInt7], FireWorksSpawnLocations[2].transform.position, Quaternion.Euler(0, 0, 0));
        }

        //LEVEL THREE
        if (numOfStarsSenses == 2 || numOfStarsSenses >2)
        {
            //Spawn Interactable Object
            forLoopPrefab1 = Instantiate(whatToSpawn[randInt1], spawnLocation1.transform.position, Quaternion.Euler(0, 0, 0));
            forLoopPrefab2 = Instantiate(whatToSpawn[randInt2], spawnLocation2.transform.position, Quaternion.Euler(0, 0, 0));
            forLoopPrefab3 = Instantiate(whatToSpawn[randInt3], spawnLocation3.transform.position, Quaternion.Euler(0, 0, 0));
            forLoopPrefab4 = Instantiate(whatToSpawn[randInt4], spawnLocation4.transform.position, Quaternion.Euler(0, 0, 0));
            spawnedItems.Add(forLoopPrefab1.GetComponent<InteractableObject_LJ>());
            spawnedItems.Add(forLoopPrefab2.GetComponent<InteractableObject_LJ>());
            spawnedItems.Add(forLoopPrefab3.GetComponent<InteractableObject_LJ>());
            spawnedItems.Add(forLoopPrefab4.GetComponent<InteractableObject_LJ>());

            //Spawn Fireworks FX slightly in front of Interactable Object
            fireWorkPrefab1 = Instantiate(fireWorksPrefab[randInt5], FireWorksSpawnLocations[0].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab2 = Instantiate(fireWorksPrefab[randInt6], FireWorksSpawnLocations[1].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab3 = Instantiate(fireWorksPrefab[randInt7], FireWorksSpawnLocations[2].transform.position, Quaternion.Euler(0, 0, 0));
            fireWorkPrefab4 = Instantiate(fireWorksPrefab[randInt8], FireWorksSpawnLocations[3].transform.position, Quaternion.Euler(0, 0, 0));
        }

        //Ensure items spawned are in a list for reference to determining an index to pull
        fullSpawnedList = spawnedItems;
    }

    //Fireworks plays once when Monster first comes into scene
    public void StartUpMonsterFireWorks()
    {
        fireWorkMonsterPrefab = Instantiate(fireWorksPrefab[Random.Range(0, fireWorksPrefab.Length)], FireWorksMonsterLocation.transform.position, Quaternion.Euler(0, 0, 0));
    }

    //Object that spawns in the water
    public void waterSpawn()
    {
        for (int i = 0; i < waterLocations.Length; i++)
        {
            waterPrefab = Instantiate(waterSpawnPrefab[Random.Range(0, waterSpawnPrefab.Length)], waterLocations[i].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }

    //Objects that are not appropriate to spawn on the towel
    public void nonTowelSpawn()
    {
        for (int i = 0; i < nonTowelLocations.Length; i++)
        {
            nonTowelPrefab = Instantiate(nonTowelSpawnPrefab[Random.Range(0, nonTowelSpawnPrefab.Length)], nonTowelLocations[i].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }

    //Objects that are okay to place on the towel
    public void towelSpawn()
    {
        for (int i = 0; i < towelLocations.Length; i++)
        {
            towelPrefab = Instantiate(towelSpawnPrefab[Random.Range(0, towelSpawnPrefab.Length)], towelLocations[i].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }

    //Determines which instruction to call and passes the index that allows the player to win
    public void spriteInstructionsSpawner()
    {
        //The index of the array matches the senses enum in InteractableObject_LJ.cs
        //Get a reference of the game object which holds a certain script
        GameObject spawnerScript = GameObject.Find("SpawnPrefabScript");

        //Make an instance to the referenced game object to access its data
        SpawnPrefabs_LJ spawner = spawnerScript.GetComponent<SpawnPrefabs_LJ>();

        //Since item spawned at element 0 is random, instructionIndex is inherently random
        //LEVEL ONE and TWO
        if (numOfStarsSenses == 0 || numOfStarsSenses == 1)
        {
            Debug.Log("Running Single Sense Question");
            int indexOfTarget = Random.Range(0, fullSpawnedList.Count);
            //Get and Store Index of Sense
            instructionIndex = (int) spawner.fullSpawnedList[indexOfTarget].assignSenses[Random.Range(0, fullSpawnedList[indexOfTarget].assignSenses.Count)];
            instructionsPrefab = Instantiate(senseInstructionPrefabs[instructionIndex], senseInstructionLocations[0].transform.position, Quaternion.Euler(0, 0, 0));
        }

        //LEVEL THREE
        if (numOfStarsSenses == 2 || numOfStarsSenses > 2)
        {
            Debug.Log("Running Multi Sense Question");
            int indexOfTargetMultiSense = Random.Range(0, fullSpawnedList.Count);
            //Get and Store Index of MultiSense
            instructionIndexMultiSense = (int) spawner.fullSpawnedList[indexOfTargetMultiSense].AssignMultiSenses[Random.Range(0, fullSpawnedList[indexOfTargetMultiSense].AssignMultiSenses.Count)];
            instructionsPrefab = Instantiate(multiSenseInstructionPrefabs[instructionIndexMultiSense], senseInstructionLocations[0].transform.position, Quaternion.Euler(0, 0, 0));
        }

        //---debugs---//
        //Debug.Log("whichSenseToWin is: " + instructionIndex);
        //Debug.Log(senseInstructionPrefabs[instructionIndex].name + " from SpawnPrefabs.cs");
    }

    public void DestroyPrefabs()
    {
        //Clearing out the existing objects for new set of objects and a new question
        Destroy(forLoopPrefab1);
        Destroy(forLoopPrefab2);
        Destroy(forLoopPrefab3);
        Destroy(forLoopPrefab4);
        Destroy(waterPrefab);
        Destroy(nonTowelPrefab);
        Destroy(instructionsPrefab);

        //Clear the lists
        spawnedItems.Clear();
        fullSpawnedList.Clear();
    }

    //Called by SceneManager_LJ.cs to collect the sense index int that will allow the player to match and win
    public int GetConditionToWin()
    {
        return instructionIndex;
    }

    //Called by SceneManager_LJ.cs to collect the multi sense index int that will allow the player to match and win in the last level
    public int GetConditionToWinMultiSense()
    {
        return instructionIndexMultiSense;
    }
}

