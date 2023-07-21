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
    [SerializeField] private GameObject catCard;
    
    private List<int> deckDataList;
    private Game _game;
    private bool once;


    public void Initialize()
    {
        int count = 20;
        int[] deckData = new int[count];
        for (int i = 0; i < count; i++)
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
    }

    private void ClickOnDeck()
    {
        if (_game.readyToEndTurn) return;

        if (_game.TurnIndex % PhotonNetwork.PlayerList.Length == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        {
            var randomValue = UnityEngine.Random.Range(0, deckDataList.Count);
            photonView.RPC(nameof(Draw), RpcTarget.All, randomValue);
        }
    }

    [PunRPC]
    public void Draw(int value)
    {
        int card = deckDataList[value];

        int currentPlayerIndex = _game.TurnIndex % PhotonNetwork.PlayerList.Length;
        
        PlayerUI player = _game.Players[currentPlayerIndex];
        
        player.TakeCard(card,currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1);

        var v = Instantiate(catCard, transform.position, Quaternion.identity, transform.parent);
        v.transform.SetSiblingIndex(0);

        var v1 = (_game.TurnIndex + PhotonNetwork.LocalPlayer.ActorNumber - 1) % PhotonNetwork.PlayerList.Length;
        //var v1 = (currentPlayerIndex) % PhotonNetwork.PlayerList.Length;
        
        v.transform.DOMove(_game.positions[v1].transform.position, 1);
        
        
        
        deckDataList.Remove(card);
        
        deckCount.text = deckDataList.Count.ToString();
    }
}
