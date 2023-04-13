using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOrb : MonoBehaviour
{
   public float XrotateSpeed = 10f;
   public float YrotateSpeed = 10f;
   public float ZrotateSpeed = 10f;
   
 
 void Update()
 {
     
     transform.Rotate( XrotateSpeed * Time.deltaTime,YrotateSpeed * Time.deltaTime,ZrotateSpeed * Time.deltaTime);
     
 }

 
}
