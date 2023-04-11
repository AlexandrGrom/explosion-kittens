using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _playersCount;
    private const string playersCount = "Players Count: ";

    private void Awake()
    {
        _playersCount.text = $"{playersCount}{(int)_slider.value}";
        _slider.onValueChanged.AddListener(value =>
        {
            _playersCount.text = $"{playersCount}{(int)value}";
            Debug.Log((int)value);
        });
    }
}
