using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] private float power = 0.05f;
    [SerializeField] private float duration = 0.1f;
    private Tween animation;


    private void Reset()
    {
        button = GetComponent<Button>();
    }

    protected virtual void Awake()
    {
        button.onClick.AddListener(Animation);
    }

    private void Animation()
    {
        animation?.Kill();
        transform.localScale = Vector3.one;
        animation = transform.DOPunchScale(Vector3.one * power, duration);
    }
}
