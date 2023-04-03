using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Deck : MonoBehaviour, IPunObservable
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private TextMeshPro text;

    private int seed;
    
    public void Initialize()
    {
        seed = new Random().Next();
    }

    private void OnMouseDown()
    {
        photonView.RPC(nameof(Drowe), RpcTarget.All,true);
    }

    private void OnMouseUp()
    {
        photonView.RPC(nameof(Drowe), RpcTarget.All,false);
    }


    [PunRPC]
    public void Drowe(bool b)
    {
        Color c = b ? Color.black : Color.white;
        _renderer.material.color = c;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(seed);
        }
        else
        {
            seed = (int)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        text.text = seed.ToString();
    }
}
