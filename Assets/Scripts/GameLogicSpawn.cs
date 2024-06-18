using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicSpawn : NetworkBehaviour
{
    public GameObject[] animalGameObjects;
    

    GameObject animalInstance;
    public GameObject statePanel;
    public GameObject menuPanel;
    [SyncVar (hook = nameof(EnableBoolInstance))]private bool InstantiateBool = false;


    public void EnableBoolInstance(bool oldBool, bool newBool) 
    {
        statePanel.SetActive(InstantiateBool);
        menuPanel.SetActive(!InstantiateBool);
    }

    public void TestSpawn() 
    {
        animalInstance = animalGameObjects[0];
        animalInstance = Instantiate(animalInstance);
        NetworkServer.Spawn(animalInstance);
    }

    public void ButtonPressed(int index) 
    {
        if (isServer)
        {
            if (index >= 0 && index < animalGameObjects.Length)
            {
                GameObject selectedPrefab = animalGameObjects[index];

                if (selectedPrefab != null && InstantiateBool == false)
                {
                    animalInstance = Instantiate(selectedPrefab);
                    NetworkServer.Spawn(animalInstance);
                    InstantiateBool = true;
                }
                else
                {
                    Debug.LogWarning("Selected prefab is null.");
                }

            }
        }
        else 
        {
            Debug.LogWarning("Invalid index provided.");
        }
        
    }

    public void ReturnPressed()
    {
        if(isServer) 
        { 
            NetworkServer.Destroy(animalInstance);
            InstantiateBool = false;
        }
       

    }

   
    
}
