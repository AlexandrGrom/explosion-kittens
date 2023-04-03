using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI info;

    private void Awake()
    {
        //initGame
        if (PhotonNetwork.IsMasterClient)
        {
            var v = PhotonNetwork.Instantiate("deck", Vector3.zero, Quaternion.identity);
            v.GetComponent<Deck>().Initialize();
        }
    }
    
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
        Debug.Log(newPlayer.NickName + "connected");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "disconnect");
    }
}
