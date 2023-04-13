using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text playerName;
    public void Setup(Player _player)
    {
        player=_player;
        playerName.text=_player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player==otherPlayer)
        {
           Destroy(gameObject); 
        }
    }
}