using System;
using Lobby;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private InputFieldSetUp _nameSetUp;

    [SerializeField] private Button joinRoom;
    [SerializeField] private Button showRoomsList;
    [SerializeField] private Button playWithBot;

    private void Awake()
    {
        joinRoom.onClick.AddListener(JoinRoom);
        //showRoomsList.onClick.AddListener(JoinRoom);
        //playWithBot.onClick.AddListener(JoinRoom);
    }


    private void Start()
    {
        PhotonNetwork.NickName = _nameSetUp.Name;
        
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
