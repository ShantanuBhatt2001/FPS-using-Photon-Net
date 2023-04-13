using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
   [SerializeField] Transform orientation;

   [Header ("Wallrunning")]
   [SerializeField] float wallDistance=0.5f;
   [SerializeField] float minimumJumpHeight=40f;
   [SerializeField]float normalGravity=90f;
    [SerializeField]float wallGravity=10f;
    [SerializeField]float wallJumpForce=60f;
    [SerializeField]LayerMask wallRunLayer;
    
    [Header("Camera Tilt")]
    [SerializeField] Camera cam;
    [SerializeField] float normalFOV;
    [SerializeField] float wallFOV;
    [SerializeField] float wallFOVTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;
    [SerializeField] float wallSmooth;

    public float tilt;

    public bool isWallrunning=false;
    public bool wallLeft=false;
    public bool wallRight=false;
    public bool wallFront=false;
    public bool wallBack=false;
     

    public RaycastHit leftWallHit,rightWallHit,frontWallHit,backWallHit;

   Rigidbody rb;

   PlayerMovement playerMovement;

   

   


    bool canWallRun()
    {
        Debug.Log("Called");
        return !Physics.Raycast(transform.position,Vector3.down,minimumJumpHeight);
    }
   void CheckWall()
   {
       wallLeft=Physics.Raycast(transform.position,-orientation.right,out leftWallHit, wallDistance,wallRunLayer);
       wallRight=Physics.Raycast(transform.position,orientation.right,out rightWallHit, wallDistance,wallRunLayer);
       wallFront=Physics.Raycast(transform.position,orientation.forward,out frontWallHit, wallDistance,wallRunLayer);
       wallBack=Physics.Raycast(transform.position,-orientation.forward,out backWallHit, wallDistance,wallRunLayer);
   }


    void Start()
    {
        rb=GetComponent<Rigidbody>();
        playerMovement=GetComponent<PlayerMovement>();
    }

   void Update()
   {
       if(!playerMovement.PV.IsMine)
       return;
       
       CheckWall();
       if(canWallRun())
       {
           Debug.Log("CanWallRun");
           if(wallLeft || wallBack || wallFront ||wallRight)
           {
               
               StartWallRun();
           }
           
           else{
               StopWallRun();
           }
       }
       else{
           StopWallRun();
       }

        if(!isWallrunning || (Vector3.Angle(orientation.forward,playerMovement.orientation.forward)>90 ||Vector3.Angle(orientation.forward,playerMovement.orientation.forward)<90))
       orientation.rotation=playerMovement.orientation.rotation;
       
   }

   void StartWallRun()
   {
       playerMovement.jumps=1;
       rb.velocity= Vector3.Lerp(rb.velocity,new Vector3(rb.velocity.x,0,rb.velocity.z),wallSmooth*Time.deltaTime);
       Debug.Log("Wallrun Rest");
       if(wallLeft )
       {
           tilt=Mathf.Lerp(tilt,-camTilt,camTiltTime*Time.deltaTime);
       }
       else if(wallRight )
       {
           tilt=Mathf.Lerp(tilt,camTilt,camTiltTime*Time.deltaTime);
       }
       else if(wallFront || wallBack)
       {
           tilt=Mathf.Lerp(tilt,0,camTiltTime*Time.deltaTime);
       }
       cam.fieldOfView=Mathf.Lerp(cam.fieldOfView,wallFOV,wallFOVTime*Time.deltaTime);
       isWallrunning=true;
       playerMovement.gravityScale=wallGravity;
       
       
       if(Input.GetKeyDown(playerMovement.jumpKey))
       {
           
           if(wallLeft)
           {
               Vector3 wallRunJumpDirection=(orientation.transform.up+leftWallHit.normal).normalized;
               rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);
               rb.AddForce(wallRunJumpDirection*wallJumpForce,ForceMode.Impulse);
           }
           else if(wallRight)
           {
               Vector3 wallRunJumpDirection=(orientation.transform.up+rightWallHit.normal).normalized;
               rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);
               rb.AddForce(wallRunJumpDirection*wallJumpForce,ForceMode.Impulse);
           }
           else if(wallBack)
           {
               Vector3 wallRunJumpDirection=(orientation.transform.up+backWallHit.normal).normalized;
               rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);
               rb.AddForce(wallRunJumpDirection*wallJumpForce,ForceMode.Impulse);
           }
           else if(wallFront)
           {
               Vector3 wallRunJumpDirection=(orientation.transform.up+frontWallHit.normal).normalized;
               rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);
               rb.AddForce(wallRunJumpDirection*wallJumpForce,ForceMode.Impulse);
           }
       }
   }

   void StopWallRun()
   {
       
       cam.fieldOfView=Mathf.Lerp(cam.fieldOfView,normalFOV,wallFOVTime*Time.deltaTime);
       isWallrunning=false;
       playerMovement.gravityScale=normalGravity;
        tilt=Mathf.Lerp(tilt,0,camTiltTime*Time.deltaTime);
   }
}
