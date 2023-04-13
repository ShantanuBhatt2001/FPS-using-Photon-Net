using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class ScoreBoardManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreItemPrefab;
    Dictionary<Player,ScoreBoardItem> scoreboardItems=new Dictionary<Player, ScoreBoardItem>();



    private void Start() {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            AddScoreBoardItem(player);
        }
    }


    void AddScoreBoardItem(Player player)
    {
        ScoreBoardItem item=Instantiate(scoreItemPrefab,container).GetComponent<ScoreBoardItem>();
        item.Initialize(player);
        scoreboardItems[player]=item;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreBoardItem(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      RemoveScoreBoardItem(otherPlayer);
    }

    void RemoveScoreBoardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }


    public void UpdateKD(Player player,int deaths,int kills)
    {
        scoreboardItems[player].deathsText.text=deaths.ToString();
        scoreboardItems[player].killsText.text=kills.ToString();
        
    }
   
}
