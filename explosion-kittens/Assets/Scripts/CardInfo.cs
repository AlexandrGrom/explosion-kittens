using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI data;
    public int Info { get; private set; }

    public void Initialize(int info)
    {
        Info = info;
    }

    public void SetInfo(bool isMine)
    {
        data.text = isMine ? Info.ToString() : "?";
    }
}
