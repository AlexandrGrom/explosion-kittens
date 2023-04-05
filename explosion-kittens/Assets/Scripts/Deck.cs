using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(PhotonView))]
public class Deck : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI deckCount;
    [SerializeField] private Button deckInteract;

    private int turnIndex = 0;
    
    private TextMeshProUGUI _mine;
    private TextMeshProUGUI _other;

    private List<int> deckDataList;


    public void Initialize()
    {
        int[] deckData = { 1, 2, 3, 4, 5 };
        Random random = new Random();
        deckData = deckData.OrderBy(x => random.Next()).ToArray();

        photonView.RPC(nameof(SetUpDeck), RpcTarget.All, deckData);
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
        Debug.Log(turnIndex % PhotonNetwork.PlayerList.Length);
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        
        //PhotonNetwork.LocalPlayer.GetPlayerNumber();
        if (turnIndex % PhotonNetwork.PlayerList.Length  == PhotonNetwork.LocalPlayer.ActorNumber-1)
        {
            var randomValue = UnityEngine.Random.Range(0, deckDataList.Count);
            photonView.RPC(nameof(Draw), RpcTarget.All, randomValue);
        }
    }
    
    [PunRPC]
    public void Draw(int value)
    {
        int card = deckDataList[value];

        if (card == 0)
        {
            Debug.Log("bum!");// end game
        }
        
        // draw logic (send card to players[PhotonNetwork.LocalPlayer.ActorNumber])

        //if ()
        //{
        //    _mine.text += card + " ";
        //}
        //else
        //{
        //    _other.text += "? ";
        //}
        
        
        turnIndex++;
        deckDataList.Remove(card);
        
        deckCount.text = deckDataList.Count.ToString();
    }
}
