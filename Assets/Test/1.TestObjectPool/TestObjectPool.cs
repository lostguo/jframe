using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectPool
{
    public void Init()
    {
        Debug.Log("我产生了？");
    }

    public void Dead()
    {

        Debug.Log("我销毁了.");
        PoolManager.Instance.PushObject(this);
    }
}
