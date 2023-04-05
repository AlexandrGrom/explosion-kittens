using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private Deck _deck;
    [SerializeField] private GameObject playerUIPrefab;
    private List<Player> players;
    
    public TextMeshProUGUI _mime;
    public TextMeshProUGUI _other;    
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // crate player ui and data
        
        
        var player = PhotonNetwork.Instantiate(playerUIPrefab.name, transform.position, Quaternion.identity);
        Debug.Log(player.name, player);
        player.transform.SetParent(transform);
        var playerUI = player.GetComponent<PlayerUI>();
        playerUI.SetName(photonView.Owner.NickName);
        

        
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
