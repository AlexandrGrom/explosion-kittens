using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private int speed = 10;
    public Vector3 offset = Vector3.zero;
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position + offset, Time.deltaTime * speed);
        }
    }
}
