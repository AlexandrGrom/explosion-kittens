using Photon.Pun;
using TMPro;
using UnityEngine;

//[RequireComponent(typeof(PhotonView))]
public class PlayerUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _name;

    public void SetName(string name)
    {
        _name.text = name;
    }
}