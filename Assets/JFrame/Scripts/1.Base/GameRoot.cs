using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : SingletonMono<GameRoot>
{

    protected override void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
        DontDestroyOnLoad(gameObject);


        InitManagers();
    }

    private void InitManagers()
    {
        ManagerBase[] managers = GetComponents<ManagerBase>();

        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Init(); 
        }
    }
}
