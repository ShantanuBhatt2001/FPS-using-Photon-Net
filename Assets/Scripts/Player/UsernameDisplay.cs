using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] Text nameText;

    void Start()
    {
        if(playerPV.IsMine)
        gameObject.SetActive(false);
        nameText.text=playerPV.Owner.NickName;
    }
}
