using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private TextMeshProUGUI logText;
    private void Awake()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        Log(PhotonNetwork.NickName);
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log("connected to master");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 2});
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("join room");
        PhotonNetwork.LoadLevel("Lobby 1");
    }

    public void Log(string log)
    {
        Debug.Log(log);
        logText.text += log;
    }
}
