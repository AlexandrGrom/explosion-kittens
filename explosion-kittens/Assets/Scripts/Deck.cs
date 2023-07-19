using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(PhotonView))]
public class Deck : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI deckCount;
    [SerializeField] private Button deckInteract;
    [SerializeField] private Button endTurn;
    [SerializeField] private TextMeshProUGUI endTurnText;
    
    private int turnIndex = 0;
    public bool IsMyTurn { get; private set; }
    private List<int> deckDataList;
    private Game _game;
    private bool once;


    private void Awake()
    {
        endTurn.onClick.AddListener(() =>
        {
            endTurn.interactable = false;
            once = false;
            photonView.RPC(nameof(IncrementTurn), RpcTarget.All);
        });
    }
    
    [PunRPC]
    private void IncrementTurn()
    {
        //endTurn.transform.DOPunchScale(Vector3.one, 1);
        int diedPlayers = 0;
        turnIndex += 1 + diedPlayers;
        
        int currentPlayerIndex = turnIndex % PhotonNetwork.PlayerList.Length;
        IsMyTurn = currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1;
        
        endTurn.transform.DOLocalRotate(Vector3.right * 90, 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            if (IsMyTurn)
            {
                endTurnText.text = "My Turn";
            }
            else
            {          
                endTurnText.text = "Not my Turn";
            }
            
            endTurn.transform.eulerAngles = new Vector3(-90, 0, 0);
            endTurn.transform.DOLocalRotate(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
        });
    }

    public void Initialize()
    {
        int[] deckData = new int[30];
        for (int i = 0; i < 30; i++)
        {
            deckData[i] = i + 1;
        }

        Random random = new Random();
        deckData = deckData.OrderBy(x => random.Next()).ToArray();

        photonView.RPC(nameof(SetUpDeck), RpcTarget.All, deckData);
    }

    public void SetUpGame(Game game)
    {
        _game = game;
    }

    [PunRPC]
    private void SetUpDeck(int[] customData)
    {
        deckInteract.onClick.AddListener(ClickOnDeck);

        deckDataList = customData.ToList();
        deckCount.text = deckDataList.Count.ToString();
        
        int currentPlayerIndex = turnIndex % PhotonNetwork.PlayerList.Length;
        IsMyTurn = currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1;
        

        if (IsMyTurn)
        {
            endTurnText.text = "My Turn";
        }
        else
        {          
            endTurnText.text = "Not my Turn";
        }
    }
    
    
    
    private void ClickOnDeck()
    {
        if (once) return;

        if (turnIndex % PhotonNetwork.PlayerList.Length  == PhotonNetwork.LocalPlayer.ActorNumber-1)
        {
            var randomValue = UnityEngine.Random.Range(0, deckDataList.Count);
            photonView.RPC(nameof(Draw), RpcTarget.All, randomValue);
        }
    }
    //
    // public void Players(List<PlayerUI> players)
    // {
    //     for (var i = 0; i < players.Count; i++)
    //     {
    //         var p = players[i];
    //     }
    // }
    
    [PunRPC]
    public void Draw(int value)
    {
        int card = deckDataList[value];

        int currentPlayerIndex = turnIndex % PhotonNetwork.PlayerList.Length;
        //IsMyTurn = currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1;
            

        
        PlayerUI player = _game.Players[currentPlayerIndex];


        // todo: hand should deteck whitch card was taken and send event if lose
        if (IsMyTurn)
        {
            once = true;
            endTurn.interactable = IsMyTurn;
            endTurnText.text = "End Turn";
            IsMyTurn = false;
            
            if (card == 1)
            {
                _game.HandleEndGame(false);
            }
            else
            {
                player.SetName(card.ToString());
            }
        }
        else
        {
            if (card == 1)
            {
                _game.HandleEndGame(true);
            }
            else
            {
                player.SetName("?");
            }
        }

        deckDataList.Remove(card);
        
        deckCount.text = deckDataList.Count.ToString();
    }
}
