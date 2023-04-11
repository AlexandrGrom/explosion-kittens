using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private NameSetUp _nameSetUp;
    
    private void Start()
    {
        PhotonNetwork.NickName = _nameSetUp.PlayerName;
        
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        Log(PhotonNetwork.NickName);
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("eu");
    }

    public override void OnConnectedToMaster()
    {
        Log("\nconnected to master");
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
        PhotonNetwork.LoadLevel("Game");
    }

    public void Log(string log)
    {
        Debug.Log(log);
        logText.text += log;
    }
}
