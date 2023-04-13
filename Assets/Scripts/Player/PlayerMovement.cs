using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable=ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks,IDamageable
{
    [Header("Movement")]
    public float moveSpeed=6f;
    public float movementMultiplier=10f;
    public float playerHeight=0.2f;
    [SerializeField]float airMultiplier=0.2f;
    public Transform orientation;
    [SerializeField] float maxSprintSpeed=100f;
    [SerializeField] float maxWalkSpeed=80f;


    [Header("KeyBinds")]
    public KeyCode jumpKey=KeyCode.Space;
    [SerializeField] KeyCode sprintKey=KeyCode.LeftShift;
    
    

    [Header("Jump & GroundDetection")]
    [SerializeField] float jumpForce=10f;
    public float gravityScale;
    [SerializeField]float groudDistance=0.4f;
    [SerializeField]LayerMask groundMask;
    [SerializeField]int jumpCount=2;
     public int jumps;
    



    [Header("Sprinting")]
    [SerializeField] float walkingSpeed=4f;
    [SerializeField] float sprintSpeed=6f;
    [SerializeField] float acceleration=10f;
     bool isSprinting;

    



    [Header("Slopes")]
    RaycastHit slopeHit;

    Wallrunning wallrun;

    float horizontalMovement;
    float verticalMovement;
    
    float groundDrag=6f;
    float airDrag=0f;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 wallrunDirection;
    Rigidbody rb;
    bool isGrounded;
     public PhotonView PV;


     [Header("Items")]
     
     int gunIndex,previousGunIndex=-1;
     [SerializeField] KeyCode reloadKey=KeyCode.R;
     [HideInInspector] public bool canUse=true;


     [Header("Health")]

     const int maxHealth=200;
     int currentHealth=maxHealth;

     public PlayerManager playerManager;

     [Header("CanvasManager")]
     [SerializeField]CanvasManager canvasManager;

     [Header("ScoreBoard")]
     GameObject scoreBoard;
     [SerializeField] KeyCode scoreboardKey=KeyCode.Tab;

     [Header("Guns")]

     [SerializeField] GunControl[] Guns;


     



    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, playerHeight/2+0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            return false;
        }
        else{
            return false;
        }
    }
    void Awake()
    {
        PV=GetComponent<PhotonView>();
        Debug.LogWarning(PV.InstantiationData[0]);
        playerManager=PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
         scoreBoard=GameObject.Find("ScoreBoardCanvas");
         
         
         
         
    }
    void Start()
    {
         
        wallrun=GetComponent<Wallrunning>();
        playerHeight=GetComponent<Collider>().bounds.extents.y+0.1f;
        rb= GetComponent<Rigidbody>();
        rb.freezeRotation=true;
        if(photonView.IsMine)
        {
            EquipGun(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(canvasManager.gameObject);
        }
        
    }

    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }
        isGrounded=Physics.CheckSphere(transform.position-new Vector3(0,playerHeight/2,0),groudDistance,groundMask);
        
        Debug.Log(isGrounded);
        if(Input.GetKeyDown(jumpKey) && jumps<jumpCount && !wallrun.isWallrunning)
        {
           
            Jump();
            jumps++;
        }
        
        if(OnSlope())
        slopeMoveDirection=Vector3.ProjectOnPlane(moveDirection,slopeHit.normal);
        if(wallrun.isWallrunning)
        {
            moveDirection= new Vector3(moveDirection.x,0,moveDirection.z);
            if(wallrun.wallLeft)
            {
                wallrunDirection=moveDirection-  wallrun.leftWallHit.normal;
            }
            else if(wallrun.wallRight)
            {
                wallrunDirection=moveDirection-  wallrun.rightWallHit.normal;
            }
            else if(wallrun.wallBack)
            {
                wallrunDirection=moveDirection- wallrun.backWallHit.normal;
            }
            else if(wallrun.wallFront)
            {
                wallrunDirection=moveDirection-  wallrun.frontWallHit.normal;
            }

        }
        if(transform.position.y<=-50f)
        {
            Die();
        }
        Inputs();
       
        // for(int i=0;i<Guns.Length;i++)
        // {
        //     if(Input.GetKeyDown((i+1).ToString()))
        //     {
        //         EquipGun(i);
        //         break;
        //     }
        // }
        if(Input.GetAxisRaw("Mouse ScrollWheel")>0f)
        {
            
            if(gunIndex>=Guns.Length-1)
            {
                EquipGun(0);
            }
            else{
                EquipGun(gunIndex+1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel")<0f)
        {
            if(gunIndex<=0)
            {
                EquipGun(Guns.Length-1);
            }
            else{
                EquipGun(gunIndex-1);
            }


            
        }



        if(Input.GetMouseButtonDown(0) && canUse)
        {
            
           Guns[gunIndex].Fire();
           Debug.Log("SentFire");
        }

        

        if(Input.GetKey(scoreboardKey))
        {
             scoreBoard.GetComponent<CanvasGroup>().alpha=1f;
        }
        else
        scoreBoard.GetComponent<CanvasGroup>().alpha=0f;


        if(Input.GetKeyDown(reloadKey) &&  Guns[gunIndex].canReload)
        {
            Debug.Log("StartReaload");
            StartCoroutine(Guns[gunIndex].Reload()); 
        }
        



        ControlDrag();
        ControlSpeed();

        CanvasUpdate();
    }


void CanvasUpdate()
{
   int currentBullets= Guns[gunIndex].GetComponent<GunControl>().currentBullets;
   int maxBullets= Guns[gunIndex].GetComponent<GunControl>().maxBullets;
   canvasManager.SetCanvas(currentHealth,maxHealth,currentBullets,maxBullets);
}
    void ControlSpeed()
    {
        if(((Input.GetKey(sprintKey) || wallrun.isWallrunning) )|| !isGrounded)
        {
            isSprinting=true;
            moveSpeed=Mathf.Lerp(moveSpeed,sprintSpeed,acceleration*Time.deltaTime);
            
                var v = rb.velocity;
                var f = v.y;
                v.y = 0.0f;
                v = Vector3.ClampMagnitude(v,maxSprintSpeed);
                v.y = f;
                rb.velocity = v;
             
        }
        
        else 
        {
            isSprinting=false;
            moveSpeed=Mathf.Lerp(moveSpeed,walkingSpeed,acceleration*Time.deltaTime);
            var v = rb.velocity;
                var f = v.y;
                v.y = 0.0f;
                v = Vector3.ClampMagnitude(v,maxWalkSpeed);
                v.y = f;
                rb.velocity = v;
        }
    }


    void Jump()
    {
        rb.velocity=new Vector3(rb.velocity.x,0,rb.velocity.z);
        rb.AddForce(transform.up*jumpForce,ForceMode.Acceleration);
        
    }

    void Inputs()
    {
        horizontalMovement=Input.GetAxisRaw("Horizontal");
        verticalMovement=Input.GetAxisRaw("Vertical");
        
        moveDirection=orientation.forward*verticalMovement+orientation.right*horizontalMovement;
    }


    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag=groundDrag;
        }
        else
        {
            rb.drag=airDrag;
        }
    }

    void FixedUpdate()
    {
        if(!PV.IsMine)
        return;
        
        Vector3 gravity =  -gravityScale * Vector3.up;
        if(isGrounded)
        {
            jumps=1;
        }
        rb.AddForce(gravity, ForceMode.Acceleration);
        MovePlayer();
    }


    void MovePlayer()
    {
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized*moveSpeed,ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized*moveSpeed,ForceMode.Acceleration);
        }
        else if(!isGrounded && !wallrun.isWallrunning)
        {
            rb.AddForce(moveDirection.normalized*moveSpeed*airMultiplier,ForceMode.Acceleration);
        }
        else if(wallrun.isWallrunning)
        {
            rb.AddForce(moveDirection.normalized*moveSpeed,ForceMode.Acceleration);
        }
        
    }




    void EquipGun( int _index)
    {
        
        if(_index==previousGunIndex)
        {
            return;
        }
        gunIndex=_index;
        Guns[gunIndex].gunObject.SetActive(true);
        if(previousGunIndex!=-1)
        {
            Guns[previousGunIndex].gunObject.SetActive(false);
        }
        previousGunIndex=gunIndex;
        Debug.Log("Equip Item: "+ _index);

        if(PV.IsMine)
        {
            Hashtable hash=new Hashtable();
            hash.Add("itemIndex",gunIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer==PV.Owner)
        {
            EquipGun((int)changedProps["itemIndex"]);
        }
    }
    public void TakeDamage(int damage,int viewId)
    {
        PV.RPC("RPC_TakeDamage",RpcTarget.All,damage,viewId);
        
    }


    [PunRPC]
    void RPC_TakeDamage(int damage,int viewId)
    {
        if(!PV.IsMine)
        return;
        
        Debug.Log("tookDamage"+damage);
        currentHealth-=damage;
        if(currentHealth<=0)
        {
            PV.RPC("RPC_IncreaseKills",RpcTarget.All,viewId);
            Debug.LogError("Recieved toDamage Taker" +viewId);
           Die();
        }
    }

   
    [PunRPC]
    void RPC_IncreaseKills(int viewId)
    {
        
        
        Debug.LogWarning("Recievd increase kills"+viewId+" Local PV "+PV.ViewID);
        PlayerMovement[] PVs= GameObject.FindObjectsOfType<PlayerMovement>();
        for(int i=0;i<PVs.Length;i++)
        {
            if(PVs[i].PV.ViewID==viewId)
            {
                PVs[i].gameObject.GetComponent<PlayerMovement>().playerManager.UpdateKills();
            }
        }
    }

    void Die()
        {
            
        
            
        
        playerManager.Die();
        Destroy(canvasManager.gameObject);
        Debug.Log("U ded");
    }
}
