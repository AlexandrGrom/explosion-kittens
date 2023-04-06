using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private Deck _deck;
    [SerializeField] private PlayerUI playerUIPrefab;

    [SerializeField] private Transform[] positions;
    
    private List<Player> players;
    

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        var myIdx = PhotonNetwork.LocalPlayer.ActorNumber-1;
        
        for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            CratePlayer(i, myIdx);
        }
    }

    private void CratePlayer(int otherPlayerIndex, int thisPlayerIndex)
    {
        var targetIdx = otherPlayerIndex - thisPlayerIndex;
            
        if (targetIdx < 0)
        {
            targetIdx = PhotonNetwork.PlayerList.Length + targetIdx;
        }
        
        var v = positions[targetIdx].position;
            
        var player1 = PhotonNetwork.PlayerList[otherPlayerIndex];
            
            
        var player = Instantiate(playerUIPrefab, transform);
        player.SetName(player1.NickName);
        player.transform.SetParent(transform);
        player.transform.localScale = Vector3.one;

        player.GetComponent<RectTransform>().anchoredPosition = v;
    }
    
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CratePlayer(newPlayer.ActorNumber - 1, PhotonNetwork.LocalPlayer.ActorNumber - 1);
            
        if (PhotonNetwork.CurrentRoom.MaxPlayers < PhotonNetwork.PlayerList.Length)
        {
            return;
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            _deck.Initialize();
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "disconnect");
    }
}
