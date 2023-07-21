using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foo : MonoBehaviour
{
    [SerializeField] private Transform[] positions;
    [SerializeField] private Transform start;
    [SerializeField] private CardTest testPrefab;
    [SerializeField] private List<CardTest> tests;

    private void Awake()
    {
        tests = new List<CardTest>();
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var v = Instantiate(testPrefab, start.position, start.rotation, start.parent);
            tests.Add(v);
            var v1 = positions[tests.Count-1];
            v1.gameObject.SetActive(true);
            v.SetTarget(v1);
        }
    }
}
