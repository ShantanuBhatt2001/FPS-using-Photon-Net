using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    [SerializeField] InputField usernameInput;

    void Start()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            usernameInput.text=PlayerPrefs.GetString("username");
            PhotonNetwork.NickName=usernameInput.text;
        }
        else{
            usernameInput.text="Player"+Random.Range(0,10000).ToString("0000");
            onUserNameInputValueChanged();
        }
    }
    public void onUserNameInputValueChanged()
    {
        PhotonNetwork.NickName=usernameInput.text;
        PlayerPrefs.SetString("username",usernameInput.text);
    }
}
