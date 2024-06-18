using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RotateInputActionHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Input Action to handle")]
    
    public float speed = 0.5f;
  
    public void Rotate() 
    {
        Vector2 thumbstickAngles = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        transform.Rotate(0, -thumbstickAngles.x*speed, 0);
    }
}
