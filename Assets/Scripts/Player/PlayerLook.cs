using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{   [SerializeField] PlayerMovement playerMovement;
   [SerializeField] float sensX=1f;
   [SerializeField] float sensY=1f;
   [SerializeField]Transform cam;
   [SerializeField]Transform orientation;
   [SerializeField]Wallrunning wallrun;
    [SerializeField] float standHeight;
    [SerializeField] float crouchHeight;
    [SerializeField]float transitionTime;
    float camHeight;
   float mouseX;
   float mouseY;

   

   float xRotation;
   float yRotation;



   void Start()
   {
       
       Cursor.lockState=CursorLockMode.Locked;
       Cursor.visible=false;
       camHeight=standHeight;
       sensX=PlayerPrefs.GetFloat("sensitivity");
       sensY=PlayerPrefs.GetFloat("sensitivity");
       
   }


   void Update()
   {
       if(!playerMovement.PV.IsMine)
       return;
       Inputs();
       cam.transform.rotation = Quaternion.Euler(xRotation,yRotation, wallrun.tilt);
       orientation.rotation=Quaternion.Euler(0, yRotation, 0);
       
       cam.localPosition=new Vector3(cam.localPosition.x,camHeight,cam.localPosition.z);

       if(Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState==CursorLockMode.Locked )
       {
           Debug.Log("escape");
           Cursor.lockState=CursorLockMode.None;
           Cursor.visible=true;
       }
       else if(Input.GetKeyDown(KeyCode.Escape) && Cursor.lockState==CursorLockMode.None)
       {
           Cursor.lockState=CursorLockMode.Locked;
           Cursor.visible=false;
       }
      
   }

   void Inputs()
   {
       mouseX=Input.GetAxisRaw("Mouse X");
       mouseY=Input.GetAxisRaw("Mouse Y");

       yRotation+=mouseX*sensX;
       xRotation-=mouseY*sensY;

       xRotation=Mathf.Clamp(xRotation,-85f,85f);
   }
}
