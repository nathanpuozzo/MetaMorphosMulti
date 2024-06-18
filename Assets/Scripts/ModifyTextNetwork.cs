using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;


public class ModifyTextNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))] private string nameTag = "";
    [SyncVar(hook = nameof(OnPositionChanged))] private Vector3 tooltipPosition;

    public TMP_Text myText;
    public Vector3 tooltipStartPosition;
    void OnNameChanged(string oldValue, string value) => myText.text = nameTag;
    void OnPositionChanged(Vector3 oldValue, Vector3 value) { }
    // Start is called before the first frame update
    void Start()
    {

        myText = GetComponentInChildren<TMP_Text>();
        tooltipPosition = transform.position;
    }

    public void OnChangeName(string objectName) 
    {
        nameTag = objectName;
        myText.text = nameTag;
    }

    public void OnChangePosition(Vector3 newTooltipPosition)
    { 
        tooltipPosition = newTooltipPosition;
    } 
    
}
