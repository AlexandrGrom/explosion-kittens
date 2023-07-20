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

    public void TakeCard(int cardData, bool isMine)
    {
        if (isMine)
        {
            _game.ReadyToEndTurn();
            IsMyTurn = false;
            
            if (cardData == 1)
            {
                _game.HandleEndGame(false);
            }
            else
            {
                SetName(cardData.ToString());
            }
        }
        else
        {
            if (cardData == 1)
            {
                _game.HandleEndGame(true);
            }
            else
            {
                SetName("?");
            }
        }
    }
}