using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// 所有游戏中（非框架）配置，游戏运行时只有一个
/// 包含所有的配置文件
/// </summary>
[CreateAssetMenu(fileName ="ConfigSetting",menuName = "JFrame/ConfigSetting")]
public class ConfigSetting : ConfigBase
{

    /// <summary>
    /// 所有配置的容器
    /// <配置类型的名称，<id,具体配置>>
    /// </summary>
    [DictionaryDrawerSettings(KeyLabel ="类型",ValueLabel = "列表")]
    public Dictionary<string, Dictionary<int, ConfigBase>> configDic;


    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="T">具体配置类型</typeparam>
    /// <param name="configTypeName">配置类型名称</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetConfig<T>(string configTypeName,int id)where T : ConfigBase
    {
        // 检查类型
        if (!configDic.ContainsKey(configTypeName))
        {
            throw new System.Exception($"J:配置设置中不包含这个Key:{configTypeName}");
        }

        // 检查ID
        if (configDic[configTypeName].ContainsKey(id))
        {
            throw new System.Exception($"J:配置设置中{configTypeName}不包含这个id:{id}");
        }

        return configDic[configTypeName][id] as T;
    }
}
