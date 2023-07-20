using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private Deck _deck;
    [SerializeField] private EndTurn _endTurn;
    [SerializeField] private PlayerUI playerUIPrefab;

    [SerializeField] private RectTransform[] positions;
    
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject win;

    private List<PlayerUI> players = new List<PlayerUI>();
    public List<PlayerUI> Players => players;

    public int TurnIndex { get; private set; }

    public PlayerUI CurrentPlayer { get; private set; }

    public bool readyToEndTurn { get; private set; }

    public void ReadyToEndTurn()
    {
        _endTurn.Activate();
        readyToEndTurn = true;
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void IncrementTurnIndex()
    {
        readyToEndTurn = false;

        int diedPlayers = 0;
        TurnIndex += 1 + diedPlayers;
        
        int currentPlayerIndex = TurnIndex % PhotonNetwork.PlayerList.Length;
        
        CurrentPlayer = players[currentPlayerIndex];
        CurrentPlayer.IsMyTurn = true;
    }

    private void Awake()
    {
        _deck.SetUpGame(this);
        
        var myIdx = PhotonNetwork.LocalPlayer.ActorNumber-1;
        
        for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            CratePlayer(i, myIdx);
        }
        
        _endTurn.SetUpGame(this);
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
        player.SetUpGame(this);
        player.SetName(PhotonPlayer.NickName);
        
        
        int currentPlayerIndex = TurnIndex % PhotonNetwork.PlayerList.Length;
        player.IsMyTurn = currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1;
        
        if (player.IsMyTurn)
        {
            CurrentPlayer = player;
        }
        
        
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
            
        if (PhotonNetwork.CurrentRoom.MaxPlayers < PhotonNetwork.PlayerList.Length) return;
        
        if (PhotonNetwork.IsMasterClient)
        {
            _deck.Initialize();
            _endTurn.Initialize();
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "disconnect");
    }

    public void HandleEndGame(bool winGame)
    {
        if (winGame)
        {
            win.SetActive(true);
        }
        else
        {
            lose.SetActive(true);
        }
    }
}