using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GunControl : MonoBehaviour
{
    [Header ("GunVariables")]
    [SerializeField]PlayerMovement playerMovement;
    
    [SerializeField]string GunName;
    [SerializeField]float fireRate;
    [SerializeField] float recoilResetTime;
    [SerializeField]float reloadTime;
    [SerializeField] int damageAmount;
    [SerializeField] Transform firePoint;
    [SerializeField] Camera cam;
    [SerializeField] float maxSpreadAmount;
    float currentSpreadAmount=0f;
    public int maxBullets;
    public int currentBullets;
    public GameObject gunObject;
    

    public PhotonView PV;
    [SerializeField] GameObject hitFlash;
    

    bool canFire=true;
    public bool canReload=false;
    Player localPlayer;

    void Start()
    {
        
        currentBullets=maxBullets;
        PV=GetComponent<PhotonView>();
         for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].IsLocal)
            {
                localPlayer=PhotonNetwork.PlayerList[i];
                Debug.Log("FoundLocal");
            }
            
        }
        
         
    }
    

    

    public void Fire()
    {
        
        if(canFire)
        {
            
            if(currentBullets>0)
            {
                Vector2 spreadDirection = Random.insideUnitCircle.normalized;
                 Vector3 offsetDirection = new Vector3(cam.transform.right.x * spreadDirection.x, cam.transform.up.y * spreadDirection.y, 0);
                 float offsetMagnitude = Random.Range(0f, currentSpreadAmount); 
                 offsetMagnitude = Mathf.Tan(offsetMagnitude);
                  Vector3 bulletTrajectory = cam.transform.forward + (offsetDirection * offsetMagnitude);
                Ray ray= cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
                ray.origin=firePoint.position;
                if(Physics.Raycast(firePoint.position, bulletTrajectory, out RaycastHit hit))
                {
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damageAmount,playerMovement.PV.ViewID);
                    Debug.LogWarning("SentFrom Shooter"+ playerMovement.PV.ViewID);
                    // PV.RPC("RPC_Shoot",RpcTarget.All,hit.point);
                }
                 currentBullets--;
                if(currentBullets!=maxBullets )
                {
                   canReload=true;
                }
                StartCoroutine(FireRate());
               
            }
                 
            else 
                StartCoroutine(Reload());
           
        }
    }


    IEnumerator FireRate()
    {
       
            canFire=false;
            yield return new WaitForSeconds(1/fireRate);
            if(canReload)
        {
            StartCoroutine(SpreadReset());
            canFire=true;
        }
        
        
    }
    IEnumerator SpreadReset()
    {
        currentSpreadAmount=maxSpreadAmount;
        float timer = recoilResetTime;
     while (timer >=0 ) {
         currentSpreadAmount = Mathf.SmoothStep(currentSpreadAmount, 0f, timer);
         timer -= Time.deltaTime;
         yield return null;
        }
        currentSpreadAmount=0f;
    }


     public IEnumerator Reload()
    {
        canFire=false;
        canReload=false;
        yield return new WaitForSeconds(reloadTime);
        currentBullets=maxBullets;
        yield return new WaitForSeconds(1/fireRate);
        
        canFire=true;
        
        
    }

    

}
