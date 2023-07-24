using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Test
{
    public class Foo : MonoBehaviour
    {
        [SerializeField] private Transform[] positions;
        [SerializeField] private Transform start;
        [SerializeField] private CardTest testPrefab;
        [SerializeField] private List<CardTest> tests;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Transform parent;
        [SerializeField] private Transform world;

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
                if (tests.Count >= positions.Length) return;
                
                var v = Instantiate(testPrefab, parent);
                v.transform.position = start.position;
                tests.Add(v);
                var v1 = positions[tests.Count-1];
                v1.gameObject.SetActive(true);
                v.SetTarget(v1, canvas, world);

            
                float f = tests.Count - 1;
                float f1 = positions.Length - 1;
                float step;
                if (f == 0)
                {
                    step = 0;
                }
                else
                {
                    step = f1 / f;
                }
                for (int i = 0; i < tests.Count; i++)
                {
                
                    float targetRot = 10 - step * i;
                    if (f == 0)
                    {
                        targetRot = 0;
                    }

                    float position = (10 - targetRot) / 20;
                
                    tests[i].transform.localEulerAngles = new Vector3(0,0,targetRot);
                    tests[i].rotation = new Vector3(0,0,targetRot);
                    
                    
                    tests[i].offset = new Vector3(0,curve.Evaluate(position),0);
                }
            }
        }
    }
}
