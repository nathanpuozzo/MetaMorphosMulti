using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnGameObject : NetworkBehaviour
{
    public GameObject animalPrefab;
    private GameObject animalInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        if (isServer)
        {
            for (int i = 0; i < 1; i++) 
            { 
                animalInstance = Instantiate(animalPrefab);
                NetworkServer.Spawn(animalInstance);
            }
            
        }
        
    }
}
