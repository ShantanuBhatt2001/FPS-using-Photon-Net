using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class OrbSpawn : MonoBehaviour
{
    [SerializeField] string[] Orbs;
    [SerializeField]KeyCode[] OrbKeys;
    [SerializeField] Transform OrbSpawnPos;
    [SerializeField] float AttractionForce;
    [SerializeField]KeyCode ultimateKey=KeyCode.X;
    [SerializeField] float UltimateForce;
    [SerializeField] Transform orientation;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask FireLayer;
    [SerializeField]PlayerMovement playerMovement;
    GameObject Orb;
    float distance;
    bool hasOrb;
    int OrbNo=0; 
    Vector3 orbMove;
    void Start()
    {
        
    }

    
    void Update()
    {

    if(!playerMovement.PV.IsMine)
    return;
        //For Debug Purposes

        if(Input.GetKeyDown(OrbKeys[0]))
        {
            OrbNo=0;
        }
        if(Input.GetKeyDown(OrbKeys[1]))
        {
            OrbNo=1;
        }
        if(Input.GetKeyDown(OrbKeys[2]))
        {
            OrbNo=2;
        }
        if(Input.GetKeyDown(OrbKeys[3]))
        {
            OrbNo=3;
        }


        if(Input.GetKeyDown(ultimateKey) && !hasOrb)
        {
             Orb=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",Orbs[OrbNo]),OrbSpawnPos.position,Quaternion.identity);
            
            hasOrb=true;
            playerMovement.canUse=false;
        }

        if( hasOrb && Input.GetMouseButtonDown(0))
        {
            FireOrb();
        }
         if(hasOrb)
        {
            Orb.transform.position=Vector3.Lerp(Orb.transform.position,OrbSpawnPos.position,AttractionForce*Time.deltaTime);
        }
        
        
    }

    void FireOrb()
    {
        if(Physics.Raycast(cam.position,cam.forward,out RaycastHit Hit, 1000f,FireLayer))
        Orb.GetComponent<Rigidbody>().AddForce(-(Orb.transform.position-Hit.point).normalized*UltimateForce,ForceMode.Impulse);
        else{
            Orb.GetComponent<Rigidbody>().AddForce(cam.forward*UltimateForce,ForceMode.Impulse);
        }
        hasOrb=false;
        playerMovement.canUse=true;
    }

    void FixedUpdate()
    {
       
    }
}
        

        

        
    

    

