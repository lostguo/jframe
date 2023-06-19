using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    public GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PoolManager.Instance.GetGameObject<TestPool>(Cube);
        }
    }
}
