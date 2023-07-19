using System;
using System.Collections;
using System.Collections.Generic;
using Lobby;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _playersCount;
    private const string playersCount = "Players Count: ";

    [SerializeField] private Button createRoom;

    [SerializeField] private InputFieldSetUp _fieldSetUp;
    private int maxPlayers = 2;

    private void Awake()
    {
        _playersCount.text = $"{playersCount}{(int)_slider.value}";
        _slider.onValueChanged.AddListener(value =>
        {
            _playersCount.text = $"{playersCount}{(int)value}";
            maxPlayers = (int)value;
            Debug.Log((int)value);
        });
        
        createRoom.onClick.AddListener(OnCreateRoom);

    }
    
    public void OnCreateRoom()
    {
        byte bMaxPlayers = Convert.ToByte(maxPlayers);
        PhotonNetwork.CreateRoom(_fieldSetUp.Name, new RoomOptions {MaxPlayers = bMaxPlayers});
    }
}
