using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private Deck _deck;
    [SerializeField] private PlayerUI playerUIPrefab;

    [SerializeField] private RectTransform[] positions;

    private List<PlayerUI> players = new List<PlayerUI>();
    public List<PlayerUI> Players => players;


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
        
        var targetRT = positions[targetIdx];
            
        Player PhotonPlayer = PhotonNetwork.PlayerList[otherPlayerIndex];
            
            
        var player = Instantiate(playerUIPrefab, transform);
        player.SetName(PhotonPlayer.NickName);
        player.transform.SetParent(transform);
        player.transform.localScale = Vector3.one;
        
        
        var playerRT = player.GetComponent<RectTransform>();
        
        playerRT.anchorMin = targetRT.anchorMin;
        playerRT.anchorMax =  targetRT.anchorMax;
        playerRT.pivot =  targetRT.pivot;
        playerRT.anchoredPosition = targetRT.anchoredPosition;
        
        
        players.Insert(PhotonPlayer.ActorNumber - 1, player);
    }
    
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var myIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        CratePlayer(newPlayer.ActorNumber - 1, myIndex);
            
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
