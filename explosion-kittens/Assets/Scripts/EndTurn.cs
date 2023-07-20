using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    private Button _button;
    private PhotonView photonView;
    [SerializeField] private TextMeshProUGUI textInfo;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        photonView = GetComponent<PhotonView>();
        
        _button.onClick.AddListener(() =>
        {
            _button.interactable = false;
            photonView.RPC(nameof(IncrementTurn), RpcTarget.All);
        });
    }
    
    
    private Game _game;

    public void SetUpGame(Game game)
    {
        _game = game;
    }

    public void Initialize()
    {
        photonView.RPC(nameof(SetUpEndGame), RpcTarget.All);
    }

    [PunRPC]
    private void SetUpEndGame()
    {
        if (_game.TurnIndex % PhotonNetwork.PlayerList.Length == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        {
            textInfo.text = "My Turn";
        }
        else
        {
            textInfo.text = "Not my Turn";
        }
    }
    
    
    [PunRPC]
    private void IncrementTurn()
    {
        _game.IncrementTurnIndex();
        
        int currentPlayerIndex = _game.TurnIndex % PhotonNetwork.PlayerList.Length;

        transform.DOLocalRotate(Vector3.right * 90, 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            if (currentPlayerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1)
            {
                textInfo.text = "My Turn";
            }
            else
            {          
                textInfo.text = "Not my Turn";
            }
            
            transform.transform.eulerAngles = new Vector3(-90, 0, 0);
            transform.transform.DOLocalRotate(Vector3.zero, 0.3f).SetEase(Ease.InOutSine);
        });
    }

    public void Activate()
    {
        _button.interactable = true;
        textInfo.text = "End Turn";
    }
}
