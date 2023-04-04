using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private Deck _deckPrefab;
    
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.Log(newPlayer.NickName + "connected");
        
        if (PhotonNetwork.IsMasterClient)
        {
            var deck = PhotonNetwork.Instantiate(_deckPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Deck>();
            deck.Initialize();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "disconnect");
    }
}
