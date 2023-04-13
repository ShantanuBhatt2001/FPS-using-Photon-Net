using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;
    


    public RoomInfo info;
    public void Setup(RoomInfo _info)
    {
        info=_info;
        RoomName.text=_info.Name;
    }
    public void OnClick()
    {
        Launcher.instance.JoinRoom(info);
    }

}
