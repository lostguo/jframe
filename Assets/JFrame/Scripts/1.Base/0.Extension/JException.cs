using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 框架主要的拓展方法
/// </summary>
public static class JException
{


    #region 资源管理
    /// <summary>
    /// GameObject 放入对象池
    /// </summary>
    /// <param name="go"></param>
    public static void JGameObjectPushPool(this GameObject go)
    {
        PoolManager.Instance.PushGameObject(go);
    }

    /// <summary>
    /// GameObject 放入对象池
    /// </summary>
    /// <param name="com"></param>
    public static void JGameObjectPushPool(this Component com)
    {
        PoolManager.Instance.PushGameObject(com.gameObject);
    }

    /// <summary>
    /// 普通类放入对象池
    /// </summary>
    /// <param name="com"></param>
    public static void JObjectPushPool(this object obj)
    {
        PoolManager.Instance.PushObject(obj);
    }
    #endregion
}
