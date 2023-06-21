using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager:ManagerBase<ConfigManager>
{
    [SerializeField]
    private ConfigSetting configSetting;


    /// <summary>
    /// 获取配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configTypeName">具体配置类型</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    public  T GetConfig<T>(string configTypeName,int id) where T : ConfigBase
    {
        return configSetting.GetConfig<T>(configTypeName, id);
    }
}
