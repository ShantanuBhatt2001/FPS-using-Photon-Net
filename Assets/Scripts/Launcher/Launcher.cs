using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;

    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject StartRoomBtn;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Awake()
    {
        instance=this;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("JoinedMaster");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene=true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("Title");
        Debug.Log("JoinedLobby");
        
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text=PhotonNetwork.CurrentRoom.Name;
        Player[] players=PhotonNetwork.PlayerList;
        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        for(int i=0;i<players.Length;i++)
        {
            Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        StartRoomBtn.SetActive(PhotonNetwork.IsMasterClient);
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartRoomBtn.SetActive(PhotonNetwork.IsMasterClient);
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("Loading");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message) 
    {
        MenuManager.instance.OpenMenu("ErrorMenu");
        errorText.text="Room Creation failed: "+message;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }



    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i=0;i<roomList.Count;i++)
        {
            if(roomList[i].RemovedFromList)
            continue;
            Instantiate(roomListItemPrefab,roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }



    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

}
