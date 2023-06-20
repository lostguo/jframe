using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public void OnEnable()
    {
        Debug.Log("I am coming");
        Invoke("Dead", 3);
    }

    void Dead()
    {
        PoolManager.Instance.PushGameObject(gameObject);
    }
}
