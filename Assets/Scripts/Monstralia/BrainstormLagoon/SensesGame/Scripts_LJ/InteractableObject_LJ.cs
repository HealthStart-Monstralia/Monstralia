/* InteractableObject.cs
 * Description: This cs script receives a "message" from the lines of code containing "recipient.SendMessage" in PlayerInputController.cs 
 *              Also calls VICTORY CONDITION LOGIC in SceneManager_LJ.cs when player selects an object.
 * Author: Lance C. Jasper
 * Created: 15JUNE2017
 * Last Modified: 10AUGUST2017 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_LJ : MonoBehaviour
{
    //-----PRIVATE FIELDS-----//
    private bool doneGrowing = false;
    private Color defaultColor;
    private Color selectedColor;
    private Vector3 onPressScale;
    private Material material;
    private Vector3 resetScale;
    private SceneManager_LJ sceneManagerScript;
    private ButtonAudioSource_LJ requestAudioManager;
    private float delayLerpSecs = 1.1f;


    //-----PUBLIC FIELDS-----//
    [Header("OnTouch Rate of Growth")]
    [Tooltip("Small number = slower growth rate. \nLarger number = faster growth rate.")]
    public float growthMultiplier = 3f;
    public string objectName;


    //-----ASSIGN SENSES TO INTERACTABLE OBJECTS-----//
    public enum Senses
    {
        //enumerates to elements 0, 1, 2, 3, 4
        see, hear, smell, taste, touch 
    }
    public List<Senses> assignSenses;

    public enum MultiSense
    {
        //enumerates to elements 0, 1, 2, 3
        SeeTouch, SeeHearTouch, SeeTasteTouch, SeeTasteSmellTouch
    }
    public List<MultiSense> AssignMultiSenses;

   
    //-----ON GAME START-----//
    void Start()
    {
        sceneManagerScript = GameObject.FindObjectOfType<SceneManager_LJ>(); 

        //Make object grow/shrink on select
        material = GetComponent<Renderer>().material;
        defaultColor = Color.white;
        selectedColor = Color.green;
        onPressScale = new Vector3(0.75f, 0.75f, 0);
        resetScale = transform.localScale;

        requestAudioManager = GameObject.FindObjectOfType<ButtonAudioSource_LJ>();
    }


    //Called by "PlayerInputController.cs"-----//
    //Message sent "on touch" or "mouse button down"
    IEnumerator OnTouchDown()
    {
        material.color = selectedColor;
        requestAudioManager.ButtonDown();

        //Check senses of object for use in Level 1 and Level 2 which asks for a single sense to be satisfied in SceneManager_LJ.cs
        foreach (Senses i in assignSenses)
        {
            sceneManagerScript.SendSenses((int)i);
        }

        //Check multi senses of object for use in Level 3 which asks for multiple senses to be satisfied in SceneManager_LJ.cs
        foreach (MultiSense i in AssignMultiSenses)
        {
            sceneManagerScript.SendMultiSenses((int)i);
        }

        //When done with for loop, tell SceneManager that senses are done being sent
        sceneManagerScript.DoneSendingSenses();

        //Check to see if the item clicked satisfies the win conditions in SceneManager_LJ.cs
        sceneManagerScript.WinCondition();

        //Begin procedural button animation
        yield return StartCoroutine(ButtonTouchDown());
    }


    //Procedural animation to let player know what the game thinks they clicked
    IEnumerator ButtonTouchDown()
    {
        yield return StartCoroutine(LerpScaleUp());
        yield return StartCoroutine(DelaySeconds());
        yield return StartCoroutine(LerpScaleDown());
    }


    //When finger is up
    IEnumerator OnTouchUp()
    {
        material.color = defaultColor;
        yield return StartCoroutine(LerpScaleDown());
    }

    //When finger stays down
    void OnTouchStay()
    {
        material.color = selectedColor;
    }

    //When multiple fingers are all up
    IEnumerator OnTouchExit()
    {
        material.color = defaultColor;
        yield return StartCoroutine(LerpScaleDown());
    }


    //-----PROCEDURAL BUTTON ANIMATION LOGIC-----//
    //Called in OnTouch IEnumerator Methods
    //Scales button up
    IEnumerator LerpScaleUp()
    {
        //Grows sprite IF doneGrowing is FALSE
        if (!doneGrowing)
        {
            //Increase scale over time
            while (transform.localScale.x < (resetScale.x + onPressScale.x))
            {
                transform.localScale += transform.localScale * Time.deltaTime * growthMultiplier;
                yield return null;
            }
            doneGrowing = true;
        }
    }

    //Scales button back down
    IEnumerator LerpScaleDown()
    {
        //Shrinks sprite IF donegrowing is TRUE
        if (doneGrowing)
        {
            //Decrease scale over time
            while (transform.localScale.x > (resetScale.x))
            {
                transform.localScale -= transform.localScale * Time.deltaTime * growthMultiplier;
                yield return null;
            }
            doneGrowing = false;
        }
    }

    //Delay between button scale up and down
    IEnumerator DelaySeconds()
    {
        yield return new WaitForSeconds(delayLerpSecs);
    }
}