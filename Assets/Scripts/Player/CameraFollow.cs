using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform cameraPos;
    [SerializeField] PlayerMovement playerMovement;
    void Update()
    {
        
        transform.position=cameraPos.position;
        
    }
}
