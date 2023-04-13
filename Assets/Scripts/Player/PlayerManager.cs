using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    Player localPlayer;
    GameObject controller;
    [SerializeField]
    public GameObject PlayerUiPrefab;
    [Header("Kills and Deaths")]
    
    public int killsCount;

    public int deathsCount;
     GameObject scoreBoard;

    
    void Awake()
    {
        deathsCount=0;
        killsCount=0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].IsLocal)
            {
                localPlayer=PhotonNetwork.PlayerList[i];
                Debug.Log("FoundLocal");
            }
            
        }
        PV=GetComponent<PhotonView>();
        scoreBoard=GameObject.Find("ScoreBoardCanvas");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       UpdateScoreBoard();
    }
    void Start() {
        {
            if(PV.IsMine)
            {
                CreateController();
            }
        }
    }
    void CreateController()
    {
        Transform spawnPoint=SpawnManager.instance.GetSpawnPoint();
        
       
        
        controller=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Player"),spawnPoint.position,spawnPoint.rotation,0,new object[]{PV.ViewID});
        
    }
    public void Die()
    {
        deathsCount++;
        
        
        
         UpdateScoreBoard();
        //  UpdateKills(attackPlayer);
        PhotonNetwork.Destroy(controller);
        CreateController();
        
    }
    public void UpdateKills()
    {
        killsCount++;
        UpdateScoreBoard();
    }

     public void UpdateScoreBoard()
    {
       
        PV.RPC("RPC_UpdateScoreBoard",RpcTarget.All,localPlayer,deathsCount,killsCount);
       
    }
    // void UpdateKills(Player attackPlayer)
    // {   
    //     if(attackPlayer.IsLocal)
    //     {
    //         killsCount++;
    //     }
    // }

   

    [PunRPC] void RPC_UpdateScoreBoard(Player player,int deaths,int kills)
    {
        

        
        scoreBoard.GetComponentInChildren<ScoreBoardManager>().UpdateKD(player,deaths,kills);
    }
    
            
        
        
            
            
        
    

}
