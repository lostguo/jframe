using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ManagerBase<ResourceManager>
{
    // 需要缓存的类型
    private Dictionary<Type, bool> wantCacheDic;

    public override void Init()
    {
        base.Init();

        // TODO: 替换成真实配置
        wantCacheDic = new Dictionary<Type, bool>();
    }

    /// <summary>
    /// 检查一个类型是否需要缓存
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool CheckCacheDic(Type type) {
        return wantCacheDic.ContainsKey(type);
    }

    /// <summary>
    /// 加载 Unity 资源，例：AudioClip
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 加载实例 - 普通class
    /// 如果类型需要缓存，会从对象池获取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>() where T:class, new()
    {
        if (CheckCacheDic(typeof(T)))
        {
            return PoolManager.Instance.GetObject<T>();
        }
        else
        {
            return new T();
        }
    }

    /// <summary>
    /// 获取实例 - 组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Load<T>(string path,Transform parent=null) where T : Component
    {
        if (CheckCacheDic(typeof(T)))
        {
            return PoolManager.Instance.GetGameObject<T>(GetPrefab(path), parent);
        }
        else
        {
            return InstantiateForPrefab(path).GetComponent<T>();
        }
    }


    public void LoadGameObjectAsync<T>(string path, Action<T> callBack = null,Transform parent = null) where T : UnityEngine.Object
    {

        if (CheckCacheDic(typeof(T)))
        {
            GameObject go = PoolManager.Instance.CheckCacheAndLoadGameObject(path, parent);

            if (go != null)
            {
                callBack?.Invoke(go.GetComponent<T>());
            }
            else
            {
                //对象池没有
                StartCoroutine(DoLoadGameObjectAsync<T>(path, callBack, parent));
            }
        }
        else
        {
            //对象池没有
            StartCoroutine(DoLoadGameObjectAsync<T>(path, callBack, parent));
        }

    }


    /// <summary>
    /// 异步加载游戏物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callBack"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    IEnumerator DoLoadGameObjectAsync<T>(string path, Action<T> callBack = null, Transform parent = null) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        yield return request;
        GameObject go = InstantiateForPrefab(request.asset as GameObject,parent);
        callBack?.Invoke(go.GetComponent<T>());
    }


    /// <summary>
    /// 异步加载 untiy 资源，AudiClip Sprite
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="callBack"></param>
    public void LoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
    {
        StartCoroutine(DoLoadAssetAsync(path, callBack));
    }

    IEnumerator DoLoadAssetAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;

        callBack?.Invoke(request.asset as T);
    }


    /// <summary>
    /// 获取预制体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public GameObject GetPrefab(string path)
    {
        GameObject prefab = Resources.Load<GameObject>(path);//PS：assetBundle

        if(prefab!=null)
        {
            return prefab;
        }
        else
        {
            throw new Exception($"J:预制体路径有误，没有找到预制体{path}");
        }
    }

    /// <summary>
    /// 基于预制体实例化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject InstantiateForPrefab(string path,Transform parent = null)
    {
        return InstantiateForPrefab(GetPrefab(path), parent);
    }

    /// <summary>
    /// 基于预制体实例化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject InstantiateForPrefab(GameObject prefab, Transform parent = null)
    {
        GameObject go = GameObject.Instantiate<GameObject>(prefab, parent);
        go.name = prefab.name;

        return go;
    }
}
