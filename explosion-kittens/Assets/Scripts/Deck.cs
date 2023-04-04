using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Deck : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshPro text;

    private bool _playerTurn;
    
    private TextMeshProUGUI _mine;
    private TextMeshProUGUI _other;

    private List<int> deckDataList;

    private void Awake()
    {
        var game = FindObjectOfType<Game>();
        _mine = game._mime;
        _other = game._other;
    }


    public void Initialize()
    {
        int[] deckData = { 1, 2, 3, 4, 5 };

        Random random = new Random();
        deckData = deckData.OrderBy(x => random.Next()).ToArray();

        SetTurn(true);
        SetUpDeck(deckData);
        

        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others};
        SendOptions sendOptions = new SendOptions { Reliability = true };
        
        PhotonNetwork.RaiseEvent((byte)nameof(SetUpDeck).GetHashCode() , deckData , options, sendOptions);
        PhotonNetwork.RaiseEvent((byte)nameof(SetTurn).GetHashCode() , false , options, sendOptions);
    }
    
    
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)nameof(SetUpDeck).GetHashCode())
        {
            SetUpDeck((int[])photonEvent.CustomData);
        }
        else if(photonEvent.Code == (byte)nameof(SetTurn).GetHashCode())
        {
            SetTurn((bool)photonEvent.CustomData);
        }
    }
    
    
    private void SetUpDeck(int[] customData)
    {
        deckDataList = customData.ToList();
        text.text = "";
            
        foreach (var data in deckDataList)
        {
            text.text += data.ToString();
            text.text += "";
        }
    }

    private void SetTurn(bool value)
    {
        _playerTurn = value;
    }
    

    private void OnMouseDown()
    {
        if (_playerTurn)
        {
            var v = UnityEngine.Random.Range(0, deckDataList.Count);
            photonView.RPC(nameof(Draw), RpcTarget.All, v);
        }
    }
    
    [PunRPC]
    public void Draw(int value)
    {
        int card = deckDataList[value];

        if (_playerTurn)
        {
            _mine.text += card + " ";
        }
        else
        {
            _other.text += "? ";
        }
        
        _playerTurn = !_playerTurn; // next playter turn
        deckDataList.Remove(card);
        
        text.text = "";
        foreach (var data in deckDataList)
        {
            text.text += data.ToString();
            text.text += "";
        }
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

}
