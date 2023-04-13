using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
public class ScoreBoardItem : MonoBehaviour
{
    public Text userName;
    public Text killsText;
    public Text deathsText;


    public void Initialize(Player player)
    {
        userName.text=player.NickName;
    }
    
}
