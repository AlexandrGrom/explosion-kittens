using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;

//[RequireComponent(typeof(PhotonView))]
public class PlayerUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _name;
    public string s => _name.text;
    public bool IsMyTurn { get;  set; }
    private Game _game;

    public void SetUpGame(Game game)
    {
        _game = game;
    }

    public void SetName(string name)
    {
        _name.text += name;
    }

    public void TakeCard(CardInfo card, bool isMine)
    {
        card.SetInfo(isMine);

        if (isMine)
        {
            _game.ReadyToEndTurn();
            IsMyTurn = false;
            
            if (card.Info == 1)
            {
                _game.HandleEndGame(false);
            }
        }
        else
        {
            if (card.Info == 1)
            {
                _game.HandleEndGame(true);
            }
        }
        
        var targetIdx = (_game.TurnIndex + PhotonNetwork.LocalPlayer.ActorNumber - 1) % PhotonNetwork.PlayerList.Length;
        card.transform.DOMove(_game.positions[targetIdx].transform.position, 1);
    }
}