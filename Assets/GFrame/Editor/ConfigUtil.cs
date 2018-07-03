using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GFrame
{
    public class ConfigUtil
    {
        public static T LoadConfig<T>()
            where T : ScriptableObject
        {
            var config = AssetDatabase.LoadAssetAtPath<T>(string.Format("Assets/GFrameSettings/{0}.asset", typeof(T).ToString()));
            if (config == null)
                config = ScriptableObject.CreateInstance<T>();

            return config;
        }
        public static void SaveConfig<T>(T config)
            where T : ScriptableObject
        {
            string path = Application.dataPath + "/GFrameSettings";
            // 如果项目总不包含该路径，创建一个
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = string.Format("Assets/GFrameSettings/{0}.asset", typeof(T).ToString());

            // 生成自定义资源到指定路径
            AssetDatabase.CreateAsset(config, path);
        }
    }
}
