using System.IO;
using UnityEditor;
using UnityEngine;

namespace GFrame
{
    public class Setting : ScriptableObject
    {
        [Header("命名空间(默认GFrame)")]
        [SerializeField] string m_NameSpace = "GFrame";
        [Header("UI脚本生成路径")]
        [SerializeField] string m_UIScriptGeneratePath = "Project/Scripts/UIScripts";
        [Header("Pool脚本生成路径")]
        [SerializeField] string m_PoolScriptGeneratePath = "Project/Scripts/UIScripts/Pools";

        public string UIScriptGeneratePath { get { return m_UIScriptGeneratePath; } set { m_UIScriptGeneratePath = value; } }
        public string NameSpace { get { return m_NameSpace; } set { m_NameSpace = value; } }
        public string PoolScriptGeneratePath { get { return m_PoolScriptGeneratePath; } set { m_PoolScriptGeneratePath = value; } }
    }

    public class SettingEditorWindow : EditorWindow
    {
        Setting m_Setting;

        [MenuItem("GFrame/Settings")]
        static public void CreateWindow()
        {
            SettingEditorWindow window = (SettingEditorWindow)EditorWindow.GetWindow(typeof(SettingEditorWindow), true);
            window.titleContent = new GUIContent("GFrame Settings");
            window.Show();
            window.init();
        }

        private void init()
        {
            m_Setting = ConfigUtil.LoadConfig<Setting>();
        }

        private void OnGUI()
        {
            m_Setting.NameSpace = EditorGUILayout.TextField("命名空间", m_Setting.NameSpace);
            m_Setting.UIScriptGeneratePath = EditorGUILayout.TextField("UI脚本生成路径", m_Setting.UIScriptGeneratePath);
            m_Setting.PoolScriptGeneratePath = EditorGUILayout.TextField("Pool脚本生成路径", m_Setting.PoolScriptGeneratePath);

            if (GUILayout.Button("保存修改"))
            {
                ConfigUtil.SaveConfig(m_Setting);
            }
        }
    }
}
