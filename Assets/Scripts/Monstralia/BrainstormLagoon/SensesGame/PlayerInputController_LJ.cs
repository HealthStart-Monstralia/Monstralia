/* PlayerInputController.cs
 * Description: This cs script receives touch (iPad) and mouse (PC) input from the player. 
 *              It also handles raycasting using the main camera.
 * Author: Lance C. Jasper
 * Created: 15JUNE2017
 * Last Modified: 08AUGUST2017 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController_LJ : MonoBehaviour
{
    //-----PUBLIC FIELDS-----//
    [Header("Add The Camera")]
    [Tooltip("Select and drag the scene's camera from the hieracrchy window to the slot.")]
    public Camera cameraObject;

    [Header("Add The Layer")]
    [Tooltip("Select the layer meant to receive input.")]
    public LayerMask touchInputMask;

    //Prevent player from rapidly tapping/clicking objects when inappropriate
    public float inputLockingTime = 1.6f;


    //-----PRIVATE FIELDS-----//
    private RaycastHit raycastHit;
    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private bool gameReady;
    private float timeIsUp =15f;
    private bool inputLocked;


    //-----ON LOADING-----//
    void Awake()
    {
        gameReady = false;
    }


    //-----ON GAME START-----//
	void Start ()
	{
        //Get the camera that was set as the scene's camera; needed for raycast information
	    cameraObject = (Camera) GameObject.FindObjectOfType(typeof(Camera));
	    StartCoroutine(GameIsReady());
	}


	//-----ON EVERY FRAME-----//
	void Update ()
    {
		MouseInput();
        TouchInput();
        TimeCountdown();
    }


    //-----LOCK INPUT-----//
    void UnlockInput()
    {
        inputLocked = false;
    }

    void LockInput()
    {
        inputLocked = true;
        Invoke("UnlockInput", inputLockingTime);
    }

    public bool isInputLocked()
    {
        return inputLocked;
    }


    //-----GET MOUSE CLICK INPUT-----//
    //Compiler will only compile code block between "#if" and "#endif" in UNITY EDITOR but not iOS/Android BUILD
    void MouseInput()
    {
        //Is the mouse button pressed?
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            if (gameReady)
            {
                if (timeIsUp >= 0)
                {
                    if (isInputLocked())
                    {
                        //Debug.LogWarning("Input locked");
                    }
                    else
                    {
                        //Debug.Log("Input unlocked");
                        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition);

                        //Is raycastHit hitting an object on the "touchInputMask" layer?
                        if (Physics.Raycast(ray, out raycastHit, touchInputMask))
                        {
                            //Temporarily save the GAMEOBJECT hit by the raycast as "recipient"
                            GameObject recipient = raycastHit.transform.gameObject;

                            if (Input.GetMouseButtonDown(0))
                            {
                                //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchDown"
                                recipient.SendMessage("OnTouchDown", raycastHit.point,
                                    SendMessageOptions.DontRequireReceiver);
                            }

                            if (Input.GetMouseButtonUp(0))
                            {
                                //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchUp"
                                recipient.SendMessage("OnTouchUp", raycastHit.point,
                                    SendMessageOptions.DontRequireReceiver);
                            }

                            if (Input.GetMouseButton(0))
                            {
                                //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchStay"
                                recipient.SendMessage("OnTouchStay", raycastHit.point,
                                    SendMessageOptions.DontRequireReceiver);
                            }

                            //Will lock input for inputLockingTime then invoke UnlockInput()
                            LockInput();
                        }
                    }
                }
            }
        }
    }


    //-----GET TOUCH INPUT-----//
    void TouchInput()
    {
        //Has the countdown finished and the game is ready?
        if (gameReady)
        {
            if (timeIsUp >= 0)
            {
                if (isInputLocked())
                {
                    //Debug.LogWarning("Input locked");
                }
                else
                {
                    //Is the user touching the touchscreen?
                    if (Input.touchCount > 0)
                    {
                        touchesOld = new GameObject[touchList.Count];
                        touchList.CopyTo(touchesOld);
                        touchList.Clear();

                        foreach (Touch touch in Input.touches)
                        {
                            Ray ray = cameraObject.ScreenPointToRay(touch.position);

                            //Is raycastHit hitting an object on the "touchInputMask" layer?
                            if (Physics.Raycast(ray, out raycastHit, touchInputMask))
                            {
                                //Temporarily save the GAMEOBJECT hit by the raycast as "recipient"
                                GameObject recipient = raycastHit.transform.gameObject;

                                //Add touched GAMEOBJECT to list
                                touchList.Add(recipient);

                                if (touch.phase == TouchPhase.Began)
                                {
                                    //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchDown"
                                    recipient.SendMessage("OnTouchDown", raycastHit.point,
                                        SendMessageOptions.DontRequireReceiver);
                                }

                                if (touch.phase == TouchPhase.Ended)
                                {
                                    //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchUp"
                                    recipient.SendMessage("OnTouchUp", raycastHit.point,
                                        SendMessageOptions.DontRequireReceiver);
                                }

                                if (touch.phase == TouchPhase.Stationary)
                                {
                                    //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchStay"
                                    recipient.SendMessage("OnTouchStay", raycastHit.point,
                                        SendMessageOptions.DontRequireReceiver);
                                }

                                if (touch.phase == TouchPhase.Canceled)
                                {
                                    //Message sent to any GAMEOBJECT containing a METHOD called "OnTouchExit"
                                    recipient.SendMessage("OnTouchExit", raycastHit.point,
                                        SendMessageOptions.DontRequireReceiver);
                                }
                            }
                        }

                        //Release previous GAMEOBJECTS in private list
                        foreach (GameObject g in touchesOld)
                        {
                            if (!touchList.Contains(g))
                            {
                                g.SendMessage("OnTouchExit", raycastHit.point,
                                    SendMessageOptions.DontRequireReceiver);
                            }
                        }
                    }
                }
            }
        }
    }

    //Timer will not begin until the game is ready
    void TimeCountdown()
    {
        if (gameReady)
        {
            timeIsUp -= Time.deltaTime;
        }
    }

    //Player cannot click until the game is ready
    public void InputLockOut()
    {
        gameReady = false;
    }

    //-----COROUTINES-----//
    //Was initally set to a 4 sec delay to allow countdown animation to complete on start, but not MasterHandler_LJ.cs activates the level when the player presses the UI PLAY button
    public IEnumerator GameIsReady()
    {
        yield return new WaitForSeconds(0f);
        gameReady = true;
        //Debug.Log("Game Ready Input: " + gameReady);
    }
}

