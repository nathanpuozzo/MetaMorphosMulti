using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonsControllerManager : NetworkBehaviour 
{
    
    public GameLogicSpawn GameLogicScript;
    //Arrays for Buttons & Controllers
    public OVRInput.Button[] button;
    public OVRInput.Controller[] controller;

    //Vector2 for coordinates of RThumbstick
    private Vector2 rightThumbstick;
    
    //Vector2 for coordinates of LThumbstick
    private Vector2 leftThumbstick;

    //GameObject & component for passthrough
    [SerializeField] private GameObject cameraGameObject;
    private OVRPassthroughLayer passthrough;

    //GameObject & variables for the main object to observe
    [SerializeField]  private GameObject mainGameObject;
    public float speed = 1f;
    private Vector3 startPos;
    private Quaternion originalRotationValue;
    private Vector3 startScale;
    private Vector3 newScale = new Vector3();

    //GameObject & variables for UI Buttons
    [SerializeField] private GameObject uiGameObject;
    private bool uiHide = false;

    [SerializeField] private Vector3 minScale = new Vector3(0.4f,0.4f,0.4f);
    [SerializeField] private Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f);

    // Start is called before the first frame update
    void Start()
    {
       
        //Attribute value of cameraGameObject and passthrough to components in scene
        cameraGameObject = GameObject.Find("OVRCameraRig");
        passthrough = GameObject.Find("Passtrough").GetComponent(typeof(OVRPassthroughLayer)) as OVRPassthroughLayer;
        GameLogicScript = GetComponent<GameLogicSpawn>();
    }

    [Server]
    public void AnimalSpawned() 
    {
        mainGameObject = GameObject.FindGameObjectWithTag("Respawn");

        if(mainGameObject != null ) 
        { 
            //Define start position, rotation and scale of main GameObject to observe
            startPos = mainGameObject.transform.position;
            originalRotationValue = mainGameObject.transform.rotation;
            startScale = mainGameObject.transform.localScale;

            uiGameObject = GameObject.FindGameObjectWithTag("UI_Server");


        }
        
    }

    [Server]
    // Update is called once per frame
    void Update()
    {
        if (OVRPlugin.GetHandTrackingEnabled() == false)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                GameLogicScript.TestSpawn();
            }
        }
        //If I put down the Button A from the Controller, I turn on/off passthrough mode
        if (OVRPlugin.GetHandTrackingEnabled() == false)
        {
            if (OVRInput.GetDown(button[0], controller[0]))
            {
                passthrough.hidden = !passthrough.hidden;
            }
        }
        if (mainGameObject != null)
        {
            //If I put down the Button B from the Controller, the main GO returns to its initial state (position,rotation,scale)
            if (OVRInput.GetDown(button[1], controller[1]))
            {
                mainGameObject.transform.position = Vector3.Lerp(transform.position, startPos, Time.time * speed);
                mainGameObject.transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * speed);
                mainGameObject.transform.localScale = startScale;
            }

            //récupération des coordonnées x,y du joystick droit
            rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            //Régles des coordonnées pour la rotation de l'object à visualiser
            if (rightThumbstick.x < 0)
            {
                mainGameObject.transform.Rotate(0, -rightThumbstick.x * speed, 0);
            }
            if (rightThumbstick.x > 0)
            {
                mainGameObject.transform.Rotate(0, -rightThumbstick.x * speed, 0);
            }
            if (rightThumbstick.y < 0)
            {
                mainGameObject.transform.Rotate(rightThumbstick.y * speed, 0, 0);
            }
            if (rightThumbstick.y > 0)
            {
                mainGameObject.transform.Rotate(rightThumbstick.y * speed, 0, 0);
            }

            newScale.x = Mathf.Clamp(leftThumbstick.y * 0.05f, minScale.x, maxScale.x);
            newScale.y = Mathf.Clamp(leftThumbstick.y * 0.05f, minScale.y, maxScale.y);
            newScale.z = Mathf.Clamp(leftThumbstick.y * 0.05f, minScale.z, maxScale.z);
            
            if (leftThumbstick.y < 0 && mainGameObject.transform.localScale != minScale)
            {
                mainGameObject.transform.localScale += new Vector3(leftThumbstick.y * 0.05f, leftThumbstick.y * 0.05f, leftThumbstick.y * 0.05f);
            }
            if (leftThumbstick.y > 0 && mainGameObject.transform.localScale != maxScale)
            {
                mainGameObject.transform.localScale += new Vector3(leftThumbstick.y * 0.05f, leftThumbstick.y * 0.05f, leftThumbstick.y * 0.05f);
            }

            //If I put down the Button X from the Controller, I hide/Unhide UI Buttons
            if (OVRInput.GetDown(button[2], controller[2]))
            {
                if (uiHide == false)
                {
                    uiGameObject.SetActive(false);
                    uiHide = true;
                }
                else 
                {
                    uiGameObject.SetActive(true);
                    uiHide = false;
                }
            }
        }
    }
 
}
