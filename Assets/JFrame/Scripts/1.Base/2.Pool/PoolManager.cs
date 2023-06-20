using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ManagerBase<PoolManager>
{

    //根节点
    [SerializeField]
    private GameObject poolRootObj;

    /// <summary>
    /// GameObject容器
    /// </summary>
    public Dictionary<string, GameObjectPoolData> gameObjectPoolDic = new Dictionary<string, GameObjectPoolData>();

    /// <summary>
    /// Object容器
    /// </summary>
    public Dictionary<string, ObjectPoolData> objectPoolDic = new Dictionary<string, ObjectPoolData>();

    public override void Init()
    {
        base.Init();
    }


    #region GameObject对象相关操作
    /// <summary>
    /// 获取 GameObject
    /// </summary>
    /// <typeparam name="T">最终需要的组件</typeparam>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public T GetGameObject<T>(GameObject prefab, Transform parent = null) where T : UnityEngine.Object
    {

        GameObject obj = GetGameObject(prefab, parent);
        if (obj != null)
        {
            return obj.GetComponent<T>();
        }

        return null;
    }

    /// <summary>
    /// 获取 GameObject
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject GetGameObject(GameObject prefab, Transform parent = null)
    {

        GameObject obj = null;

        string name = prefab.name;
        //检查有没有这一层
        if (CheckGameObjectCache(prefab))
        {
            obj = gameObjectPoolDic[name].GetObj(parent);
        }
        // 没有的话实例化一个
        else
        {
            // 确保实例化后的游戏物体和预制体名称一致
            obj = GameObject.Instantiate(prefab, parent);
            obj.name = name;
        }

        return obj;
    }

    /// <summary>
    /// GameObject 放进对象池
    /// </summary>
    /// <param name="obj"></param>
    public void PushGameObject(GameObject obj)
    {
        string name = obj.name;
        // 现在有没有这一层
        if (gameObjectPoolDic.ContainsKey(name))
        {
            gameObjectPoolDic[name].PushObj(obj);
        }
        else
        {
            gameObjectPoolDic.Add(name, new GameObjectPoolData(obj, poolRootObj));
        }
    }

    /// <summary>
    /// 检查有没有某一层对象池数据
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public bool CheckGameObjectCache(GameObject prefab)
    {
        string name = prefab.name;
        return gameObjectPoolDic.ContainsKey(name) && gameObjectPoolDic[name].poolQueue.Count > 0;
    }
    #endregion

    #region 普通对象相关操作

    /// <summary>
    /// 获取普通对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetObject<T>() where T : class,new()
    {

        T obj;
        if (CheckObjectCache<T>())
        {
            string name = typeof(T).FullName;
            obj =(T) objectPoolDic[name].GetObj();

            return obj;
        }
        else
        {
            return new T();
        }

    }

    /// <summary>
    /// Object 放进对象池
    /// </summary>
    /// <param name="obj"></param>
    public void PushObject(object obj)
    {
        string name = obj.GetType().FullName;
        // 现在有没有这一层
        if (objectPoolDic.ContainsKey(name))
        {
            objectPoolDic[name].PushObj(obj);
        }
        else
        {
            objectPoolDic.Add(name, new ObjectPoolData(obj));
        }
    }

    public bool CheckObjectCache<T>()
    {
        string name = typeof(T).FullName;
        return objectPoolDic.ContainsKey(name) && objectPoolDic[name].poolQueue.Count > 0;

    }
    #endregion



    #region 删除

    /// <summary>
    /// 删除全部
    /// </summary>
    /// <param name="clearGameObject">是否删除游戏物体</param>
    /// <param name="wantClearCObject">是否删除普通C#对象</param>
    public void Clear(bool clearGameObject=true, bool wantClearCObject=true)
    {
        if (clearGameObject)
        {
            for (int i = 0; i < poolRootObj.transform.childCount; i++)
            {
                Destroy(poolRootObj.transform.GetChild(i).gameObject);
            }
            gameObjectPoolDic.Clear();
        }

        if (wantClearCObject)
        {
            objectPoolDic.Clear();
        }
    }


    public void ClearAllGameObject()
    {
        Clear(true, false);
    }

    public void ClearGameObject(string prefabName)
    {
        GameObject go = poolRootObj.transform.Find(prefabName).gameObject;
        if (go != null)
        {
            Destroy(go);
            gameObjectPoolDic.Remove(prefabName);
        }
    }

    public void ClearGameObject(GameObject prefab)
    {
        ClearGameObject(prefab.name);
    }

    public void ClearAllObject()
    {
        Clear(false, true);
    }

    public void ClearObject<T>()
    {
        objectPoolDic.Remove(typeof(T).FullName);
    }

    public void ClearObject(Type type)
    {
        objectPoolDic.Remove(type.FullName);
    }

    #endregion
}
