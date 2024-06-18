using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class NetworkVisibility : NetworkBehaviour
{
    [SerializeField] private GameObject AnimalPanel; 
    // Start is called before the first frame update
    void Start()
    {
        AnimalPanel = this.gameObject;   
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkServer.activeHost) AnimalPanel.SetActive(true);
        else AnimalPanel.SetActive(false);
    }


}
