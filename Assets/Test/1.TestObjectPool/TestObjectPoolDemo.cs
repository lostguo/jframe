using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectPoolDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private TestObjectPool p;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            p = PoolManager.Instance.GetObject<TestObjectPool>();

            p.Init();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            p.Dead();
        }
    }
}
