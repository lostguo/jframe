using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using System.Reflection;

/// <summary>
/// 框架层面的游戏设置
/// 对象池缓存设置、UI元素设置
/// </summary>
[CreateAssetMenu(fileName ="GameSetting",menuName ="JFrame/Config/GameSetting")]
public class GameSetting : ConfigBase
{

    //需要缓存的类型
    [LabelText("对象池设置")]
    [DictionaryDrawerSettings(KeyLabel ="类型",ValueLabel = "皆可缓存")]
    public Dictionary<Type, bool> cacheDic = new Dictionary<Type, bool>();


#if UNITY_EDITOR
    [Button(Name ="初始化游戏配置",ButtonHeight =50)]
    [GUIColor(0,1,0)]
    private void Init()
    {
        PoolAttributeOnEditor();
    }

    /// <summary>
    /// 编辑器前执行函数
    /// </summary>
    [InitializeOnLoadMethod]
    private static void LoadForEditor()
    {
        GameObject.Find("GameRoot").GetComponent<GameRoot>().GameSetting.Init();
    }

    /// <summary>
    /// 将带有 pool 特性的类型加入缓存池字典
    /// </summary>
    private void PoolAttributeOnEditor()
    {
        cacheDic.Clear();

        // 获取所有程序集
        System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
        foreach (System.Reflection.Assembly assembly in asms)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                PoolAttribute pool = type.GetCustomAttribute<PoolAttribute>();
                if (pool != null)
                {
                    cacheDic.Add(type, true);
                }
            }
        }
    }
#endif
}
