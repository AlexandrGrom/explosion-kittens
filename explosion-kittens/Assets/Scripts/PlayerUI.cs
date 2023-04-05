using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _name;

    public void SetName(string name)
    {
        _name.text = name;
        
        if (photonView.IsMine)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -900);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 900);
        }
    }
}